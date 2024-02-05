using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSway : MonoBehaviour
{
    InputControls inputControls;

    [SerializeField] float smooth;
    [SerializeField] float swayMultiplier;

    private void Awake()
    {
        inputControls = new InputControls();
    }
    private void Update()
    {
        Vector2 mouseAxis = inputControls.Player.Look.ReadValue<Vector2>();

        mouseAxis *= swayMultiplier;
        Quaternion rotationX = Quaternion.AngleAxis(mouseAxis.x, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(-mouseAxis.y, Vector3.forward);

        Quaternion targetRotation = (rotationX * rotationY);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
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
