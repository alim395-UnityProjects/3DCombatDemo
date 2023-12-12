using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerAnimate : MonoBehaviour
{
    Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("isCombat", false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            animator.SetBool("isCombat", true);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetBool("isCombat", false);
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
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
}
