using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    InputControls inputControls;

    [Header("Gun Setting")]
    [SerializeField] float fireRate = 0.1f;
    [SerializeField] int clipSize = 30;
    [SerializeField] int reservedAmmoCapacity = 270;
    [SerializeField] Transform gunMuzzlePoint;

    //This will change during the gameplay
    bool canShoot;
    int currentAmmoInClip;
    int ammoInReserve;

    //MuzzleFlash
    [SerializeField] private Sprite[] flashes;
    [SerializeField] private Image muzzleFlash;

    //Aim OF Gun
    [SerializeField] private Vector3 normalLocalPosition;
    [SerializeField] private Vector3 aimingLocalPosition;
    [SerializeField] float smoothTiming;
    bool canAim = false;


    private void Start()
    {
        currentAmmoInClip = clipSize;
        ammoInReserve = reservedAmmoCapacity;
        canShoot = true;
    }

    private void Awake()
    {
        inputControls = new InputControls();
    }

    private void FixedUpdate()
    {
        DetermineAim();
    }

    private void Update()
    {
        if (inputControls.Player.Shoot.triggered && canShoot && currentAmmoInClip > 0)
        {
            currentAmmoInClip--;
            canShoot = false;
            StartCoroutine(ShootGun());
        }
        else if(inputControls.Player.Reload.triggered && currentAmmoInClip < clipSize && ammoInReserve >= 0)
        {
            int ammoundNeeded = clipSize - currentAmmoInClip;

            if(ammoundNeeded <= ammoInReserve)
            {
                currentAmmoInClip = clipSize;
                ammoInReserve -= ammoundNeeded;
            }            
        }

        if (inputControls.Player.GunPositionAiming.triggered)
        {
            canAim = !canAim;
        }

    }

    

    IEnumerator ShootGun()
    {
        //RaycastHit hit;
        Debug.DrawLine(transform.position, Vector3.forward, Color.red, 2.0f);
        StartCoroutine(MuzzleImage());
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    IEnumerator MuzzleImage()
    {
        muzzleFlash.sprite = flashes[Random.Range(0, flashes.Length)];
        muzzleFlash.color = Color.white;
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.sprite = null;
        muzzleFlash.color = new Color(0, 0, 0, 0);
    }

    void DetermineAim()
    {
        Vector3 target = normalLocalPosition;

        if (canAim)
        {
            target = aimingLocalPosition;
            Vector3 desiredPosition = Vector3.Slerp(transform.localPosition, target, smoothTiming * Time.fixedDeltaTime);
            transform.localPosition = desiredPosition;
            target = normalLocalPosition;
        }
        if (!canAim)
        {
            Vector3 disiredPosition = Vector3.Slerp(transform.localPosition, target, smoothTiming * Time.deltaTime);
            transform.localPosition = disiredPosition;
            target = normalLocalPosition;
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
