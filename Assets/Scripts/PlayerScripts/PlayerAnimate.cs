using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerAnimate : MonoBehaviour
{
    Animator animator;
    string currentState;

    // Animation States
    const string PLAYER_IDLE = "StandingIdle";
    const string PLAYER_MOVEMENT = "BasicMovement";
    const string COMBAT_IDLE = "FightIdle";

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("isCombat", false);
        changeState(PLAYER_IDLE);
    }

    void changeState(string state)
    {
        if (currentState == null)
        {
            animator.Play(state);
            currentState = state;
            return;
        }

        //Stops playing animation twice
        if (currentState == state)
        {
            return;
        }

        animator.Play(state);
        currentState = state;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            animator.SetBool("isCombat", true);
            animator.Play(COMBAT_IDLE);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetBool("isCombat", false);
            animator.Play(PLAYER_IDLE);
        }
    }

    public void MovePlayerAnimation(Vector3 movementVector, float inputMagnitude, bool isSprinting)
    {
        animator.SetBool("isSprinting", isSprinting);
        if(movementVector != Vector3.zero)
        {
            animator.SetBool("isWalking", true);
            if (!isSprinting)
            {
                inputMagnitude /= 2;
            }
            animator.SetFloat("InputMagnitude", inputMagnitude, 0.05f, Time.deltaTime);
            changeState(PLAYER_MOVEMENT);
        }
        else
        {
            animator.SetBool("isWalking", false);
            if (animator.GetBool("isCombat"))
            {
                changeState(COMBAT_IDLE);
            }
            else
            {
                changeState(PLAYER_IDLE);
            }
        }
    }
}
