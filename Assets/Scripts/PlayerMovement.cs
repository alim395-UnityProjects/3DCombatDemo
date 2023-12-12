using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    CustomInputs inputs = null;
    PlayerAnimate playerAnimate = null;
    CharacterController characterController = null;

    Vector3 movementVector = Vector3.zero;

    Rigidbody rb = null;
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;
    private bool isSprinting;

    [SerializeField]
    private Transform cameraTransform;

    private void Awake()
    {
        inputs = new CustomInputs();
        playerAnimate = GetComponent<PlayerAnimate>();
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        inputs.Enable();
        inputs.Player.Movement.performed += Movement_performed;
        inputs.Player.Movement.canceled += Movement_canceled;
        inputs.Player.Sprint.performed += Sprint_performed;
        inputs.Player.Sprint.canceled += Sprint_canceled;
    }

    private void OnDisable()
    {
        inputs.Disable();
        inputs.Player.Movement.performed -= Movement_performed;
        inputs.Player.Movement.canceled -= Movement_canceled;
        inputs.Player.Movement.performed -= Sprint_performed;
        inputs.Player.Movement.canceled -= Sprint_canceled;
    }

    private void Update()
    {
        float magnitude = Mathf.Clamp01(movementVector.magnitude) * moveSpeed;
        float InputMagnitude = Mathf.Clamp01(movementVector.magnitude);
        if (movementVector != Vector3.zero)
        {
            //transform.forward = movementVector;

            Quaternion toRotation = Quaternion.LookRotation(movementVector, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        characterController.Move(movementVector * Time.deltaTime * magnitude);
        playerAnimate.MovePlayerAnimation(movementVector, InputMagnitude, isSprinting);
        //rb.velocity = movementVector * moveSpeed;
    }

    private void Movement_performed(InputAction.CallbackContext value)
    {
        Vector2 tempVector = value.ReadValue<Vector2>();
        movementVector = new Vector3(tempVector.x,0,tempVector.y);
        movementVector = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementVector;
        movementVector.Normalize();
        Debug.Log(movementVector);
    }

    private void Movement_canceled(InputAction.CallbackContext value)
    {
        movementVector = Vector3.zero;
    }

    private void Sprint_performed(InputAction.CallbackContext obj)
    {
        isSprinting = true;
        Debug.Log("Sprinting!");
    }

    private void Sprint_canceled(InputAction.CallbackContext obj)
    {
        isSprinting = false;
    }
}
