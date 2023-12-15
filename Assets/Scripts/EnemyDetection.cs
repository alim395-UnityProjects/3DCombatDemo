using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyDetection : MonoBehaviour
{
    private PlayerMovement movementInput;
    private CombatScript combatScript;

    public LayerMask layerMask;

    Vector3 inputDirection;
    [SerializeField] private EnemyScript currentTarget;

    public GameObject cam;

    private void Start()
    {
        movementInput = GetComponentInParent<PlayerMovement>();
        combatScript = GetComponentInParent<CombatScript>();
    }

    private void Update()
    {
        var camera = Camera.main;
        var forward = camera.transform.forward;
        var right = camera.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector2 inputVector = movementInput.moveAxis;

        if (inputVector != null )
        {
            inputDirection = forward * inputVector.y + right * inputVector.x;
            inputDirection = inputDirection.normalized;
            //Debug.Log(inputDirection);

            if(inputDirection != null){
                RaycastHit info;

                if (Physics.SphereCast(transform.position, 3f, inputDirection, out info, 10, layerMask))
                {
                    if (info.collider.transform.GetComponent<EnemyScript>().IsAttackable())
                    {
                        currentTarget = info.collider.transform.GetComponent<EnemyScript>();
                        //Debug.Log("Enemy Detected!");
                    }
                }
            }
        }
    }

    public EnemyScript CurrentTarget()
    {
        return currentTarget;
    }

    public void SetCurrentTarget(EnemyScript target)
    {
        currentTarget = target;
    }

    public float InputMagnitude()
    {
        return inputDirection.magnitude;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, inputDirection);
        Gizmos.DrawWireSphere(transform.position, 1);
        if (CurrentTarget() != null)
            Gizmos.DrawSphere(CurrentTarget().transform.position, .5f);
    }
}