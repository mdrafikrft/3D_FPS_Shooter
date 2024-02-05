using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    InputControls inputControls;

    [SerializeField] float mouseSensitivity;
    [SerializeField] private Transform cameraHolder;

    private Vector2 mouseLook;
    private float xRotation = 0f;
    private float yRotation = 0f;
    

    private void Awake()
    {
        inputControls = new InputControls();/*
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;*/
    }

    // Update is called once per frame
    void Update()
    {
        Look();
    }

    private void Look()
    {
        mouseLook = inputControls.Player.Look.ReadValue<Vector2>();

        float xInput = mouseLook.x * mouseSensitivity * Time.deltaTime;
        float yInput = mouseLook.y * mouseSensitivity * Time.deltaTime;

        xRotation -= yInput;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 60.0f);

        yRotation += xInput;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        cameraHolder.Rotate(Vector3.up * xInput);

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
