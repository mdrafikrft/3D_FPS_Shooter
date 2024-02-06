using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    InputControls inputControls;

    public float swayAmount = 0.02f;
    public float maxSwayAmount = 0.06f;
    public float swaySmooth = 3.0f;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    private void Awake()
    {
        inputControls = new InputControls();
    }

    void Update()
    {
        Vector2 swayMovement = inputControls.Player.Look.ReadValue<Vector2>();
        float movementX = -swayMovement.x * swayAmount;
        float movementY = -swayMovement.y * swayAmount;

        movementX = Mathf.Clamp(movementX, -maxSwayAmount, maxSwayAmount);
        movementY = Mathf.Clamp(movementY, -maxSwayAmount, maxSwayAmount);

        Vector3 finalPosition = new Vector3(movementX, movementY, 0);
        transform.localPosition = Vector3.Slerp(transform.localPosition, finalPosition, Time.deltaTime * swaySmooth);
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
