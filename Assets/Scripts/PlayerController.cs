using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    InputControls inputControls;

    [SerializeField] CharacterController characterController;
    [SerializeField] float playerSpeed;
    [SerializeField] Transform cam;
    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform ground;
    [SerializeField] float distanceToGround = 0.4f;

    private Vector2 playerMove;
    float smoothVelocity;
    float smoothRotateTime = 0.1f;

    private Vector3 velocity;
    private float gravity = -9.81f;
    private float jumpHeight;
    private bool isGrounded = true;
    

    private void Awake()
    {
        inputControls = new InputControls();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        GravityEffect();
    }

    private void PlayerMovement()
    {
        playerMove = inputControls.Player.Movement.ReadValue<Vector2>();

        Vector3 direction = new Vector3(playerMove.x, 0f, playerMove.y).normalized;
        if(direction.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVelocity, smoothRotateTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = ((Quaternion.Euler(0f, targetAngle, 0f)) * Vector3.forward).normalized;
            characterController.Move(moveDirection * playerSpeed * Time.deltaTime);
        }

    }

    private void Jump()
    {
        
    }

    private void GravityEffect()
    {
        isGrounded = Physics.CheckSphere(ground.position, distanceToGround);

        if(isGrounded && velocity.y < -1.0f)
        {
            velocity.y = -2.0f;
        }

        velocity.y += gravity;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void OnEnable()
    {
        inputControls.Enable();
    }

    private void OnDisable()
    {
        inputControls.Disable();
    }

}
