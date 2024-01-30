using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speedOfPlayer;
    [SerializeField] Rigidbody playerRb;
    [SerializeField] CharacterController controller;

    InputControls inputControls;


    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        
    }

    private void Awake()
    {
        inputControls = new InputControls();

        inputControls.PlayerMovement.Movement.performed += Movement_performed;
    }

    public void Movement_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log(context);
            //Debug.Log("Player Is Moving");

            Vector3 playerInput = context.ReadValue<Vector3>();
            playerInput = transform.right + transform.forward;
            controller.Move(playerInput * speedOfPlayer);
        }   
    }
}
