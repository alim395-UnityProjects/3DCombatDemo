using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatScript : MonoBehaviour
{
    CustomInputs inputs = null;
    PlayerMovement playerMovement = null;
    PlayerAnimate playerAnimate = null;
    CharacterController characterController = null;
    EnemyDetection enemyDetection = null;
    Animator animator = null;

    [SerializeField] private float attackCooldown;

    //Coroutines
    private Coroutine counterCoroutine;
    private Coroutine attackCoroutine;
    private Coroutine damageCoroutine;

    // States
    bool isAttacking = false;
    bool isCountering = false;

    // Target
    EnemyScript targetEnemy = null;

    public List<AnimationClip> defaultCombatAnimations;

    private void Awake()
    {
        inputs = new CustomInputs();
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimate = GetComponent<PlayerAnimate>();
        characterController = GetComponent<CharacterController>();
        enemyDetection = GetComponent<EnemyDetection>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        inputs.Enable();
        inputs.Player.Attack.performed += Attack_performed;
    }

    private void OnDisable()
    {
        inputs.Disable();
        inputs.Player.Attack.performed -= Attack_performed;
    }

    private void Attack_performed(InputAction.CallbackContext value)
    {
        if (isAttacking)
        {
            return;
        }

        // Check for enemy target
        if(enemyDetection.CurrentTarget() == null)
        {
            // Attack the Air
            Attack(null, 0);
            return;
        }

        // Use Directional Input to determine target
        if (enemyDetection.InputMagnitude() > .2f)
        {
            targetEnemy = enemyDetection.CurrentTarget();
        }

        // ATTACK ALREADY
        Attack(targetEnemy, TargetDistance(targetEnemy));
    }

    public void Attack(EnemyScript target, float distance)
    {
        if(target == null || distance >= 15)
        {
            string attackString = defaultCombatAnimations[0].name;
            AttackMove(attackString, .2f, null, 0);
            return;
        }
        if (distance < 15)
        {
            //animationCount = (int)Mathf.Repeat((float)animationCount + 1, (float)attacks.Length);
            string attackString = defaultCombatAnimations[0].name;
            AttackMove(attackString, attackCooldown, target, .65f);
        }
        else
        {
            targetEnemy = null;
            AttackMove(defaultCombatAnimations[0].name, .2f, null, 0);
        }
    }

    private void AttackMove(string attackString, float cooldown, EnemyScript target, float movementDuration)
    {
        playerAnimate.changeState(attackString);

        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);
        attackCoroutine = StartCoroutine(AttackCoroutine(cooldown));

        if (target == null)
            return;

        //targetEnemy.StopMoving();
        MoveTorwardsTarget(target, movementDuration);

        IEnumerator AttackCoroutine(float duration){
            playerMovement.acceleration = 0;
            isAttacking = true;
            playerMovement.enabled = false;
            yield return new WaitForSeconds(duration);
            isAttacking = false;
            yield return new WaitForSeconds(.2f);
            playerMovement.enabled = true;
            LerpCharacterAcceleration();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Helper Methods

    float TargetDistance(EnemyScript target)
    {
        return Vector3.Distance(transform.position, target.transform.position);
    }

    public Vector3 TargetOffset(Transform target)
    {
        Vector3 position;
        position = target.position;
        return Vector3.MoveTowards(position, transform.position, .95f);
    }

    void MoveTorwardsTarget(EnemyScript target, float duration)
    {
        transform.DOLookAt(target.transform.position, .2f);
        transform.DOMove(TargetOffset(target.transform), duration);
    }

    void LerpCharacterAcceleration()
    {
        playerMovement.acceleration = 0;
        DOVirtual.Float(0, 1, .6f, ((acceleration) => playerMovement.acceleration = acceleration));
    }
}
