using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speedOfPlayer;
    [SerializeField] CharacterController characterController;
    [SerializeField] float PlayerRotationSpeed;

    InputControls inputControl;
    Vector2 MoveInput = Vector2.zero;
    float turnSmoothTime = 0.01f;
    float turnCurrentVelocity;

    private void Awake()
    {
        inputControl = new InputControls();
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(MoveInput.x, 0f, MoveInput.y) * speedOfPlayer * Time.fixedDeltaTime;
        
    }
    private void OnEnable()
    {
        inputControl.Enable();
        
        //Movement
        inputControl.PlayerMovement.Movement.performed += Movement_performed;
        inputControl.PlayerMovement.Movement.canceled += Movement_canceled;

        //Jump
        inputControl.PlayerMovement.Jump.performed += Jump_performed;
        inputControl.PlayerMovement.Jump.canceled += Jump_canceled;

    }

    private void OnDisable()
    {
        inputControl.Disable();
        inputControl.PlayerMovement.Movement.performed -= Movement_performed;
        inputControl.PlayerMovement.Movement.canceled -= Movement_canceled;
    }

    private void Movement_performed(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();

        Vector3 direction = new Vector3(MoveInput.x, 0f, MoveInput.y).normalized;
        
        if(direction.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + 90;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCurrentVelocity, turnSmoothTime);
            
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            characterController.Move(direction * Time.deltaTime * speedOfPlayer);
        }
    }

    private void Movement_canceled(InputAction.CallbackContext context)
    {
        MoveInput = Vector2.zero;
    }

    private void Jump_performed(InputAction.CallbackContext context)
    {
        Debug.Log("Jumpeddddddd");
    }

    private void Jump_canceled(InputAction.CallbackContext context)
    {

    }
}
