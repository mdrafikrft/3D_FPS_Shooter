using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    InputControls inputControls;

    public float swayAmount = 0.02f;
    public float maxSwayAmount = 0.06f;
    public float swaySmooth = 3.0f;
    private Vector2 currentRotation;

    private Vector3 initialPosition;

    //Recoiling of the Gun
    [SerializeField] bool isRandomizeRecoil;
    [SerializeField] Vector2 randomizedConstraint;

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
        SwayOfGun();
    }

    public void SwayOfGun()
    {
        Vector2 swayMovement = inputControls.Player.Look.ReadValue<Vector2>();

        currentRotation = -swayMovement * swayAmount;

        currentRotation.x = Mathf.Clamp(currentRotation.x, -maxSwayAmount, maxSwayAmount);
        currentRotation.y = Mathf.Clamp(currentRotation.y, -maxSwayAmount, maxSwayAmount);

        Vector3 finalPosition = new Vector3(currentRotation.x, currentRotation.y, 0);
        transform.localPosition = Vector3.Slerp(transform.localPosition, finalPosition, Time.deltaTime * swaySmooth);
    }

    public void RecoilOfGun()
    {
        transform.localPosition -= Vector3.forward * 0.1f;

        if (isRandomizeRecoil)
        {
            float xRecoil = Random.Range(-randomizedConstraint.x, randomizedConstraint.x);
            float yRecoil = Random.Range(-randomizedConstraint.y, randomizedConstraint.y);

            Vector2 recoiled = new Vector2(xRecoil, yRecoil);
            currentRotation += recoiled;

        }
        
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
