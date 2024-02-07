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
    [Header("Muzzle Flash")]
    [SerializeField] private Sprite[] flashes;
    [SerializeField] private Image muzzleFlash;

    //Aim OF Gun
    [Header("Aim of Gun")]
    [SerializeField] private Vector3 normalLocalPosition;
    [SerializeField] private Vector3 aimingLocalPosition;
    [SerializeField] float smoothTiming;
    bool canAim = false;

    //RayCasting
    [Header("RayCasting of Gun")]
    [SerializeField] float damage = 10.0f;
    [SerializeField] float shootRange = 100.0f;

    //Recoiling Of Gun
    WeaponSway weaponSway;

    //Enemy damage controller
    Enemy enemy;

    private void Start()
    {
        currentAmmoInClip = clipSize;
        ammoInReserve = reservedAmmoCapacity;
        canShoot = true;
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();

    }

    private void Awake()
    {
        inputControls = new InputControls();
        weaponSway = GetComponent<WeaponSway>();
    }

    private void FixedUpdate()
    {
        DetermineAim();
    }

    private void Update()
    {
        Debug.DrawLine(transform.parent.position, transform.parent.forward, Color.red);
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
        weaponSway.RecoilOfGun();
        StartCoroutine(MuzzleImage());

        yield return new WaitForSeconds(fireRate);
        canShoot = true;

        GunShootRayCasting();
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

    void GunShootRayCasting()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.parent.position, transform.parent.forward, out hit, shootRange))
        {
            Debug.Log(hit.transform.name);
            
            if(hit.transform.tag == "Enemy")
            {
                enemy.TakeDamage(damage);
            }
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
