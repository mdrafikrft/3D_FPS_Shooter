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
    bool wantAim = false;

    //RayCasting
    [Header("RayCasting of Gun")]
    [SerializeField] Transform gunMuzzle;
    [SerializeField] float damage = 10.0f;
    [SerializeField] float shootRange = 100.0f;

    //Recoiling Of Gun
    WeaponSway weaponSway;

    //Enemy damage controller
    [SerializeField] float ImpactForceToEnemy = 10.0f;
    

    //MuzzleFlash Particle
    [SerializeField] private ParticleSystem muzzleFlashParticleEffect;

    //particle effects
    [SerializeField] GameObject bulletImpactEffect;
    [SerializeField] GameObject enemyKillParticleEffect;


    private void Start()
    {
        currentAmmoInClip = clipSize;
        ammoInReserve = reservedAmmoCapacity;
        canShoot = true;
        
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
        //Shoot activity 
        if (inputControls.Player.Shoot.triggered && canShoot && currentAmmoInClip > 0)
        {
            currentAmmoInClip--;
            canShoot = false;
            StartCoroutine(ShootGun());
        }

        //Reloading of Gun Activity
        else if(inputControls.Player.Reload.triggered && currentAmmoInClip < clipSize && ammoInReserve >= 0)
        {
            int ammoNeeded = clipSize - currentAmmoInClip;

            if(ammoNeeded <= ammoInReserve)
            {
                currentAmmoInClip = clipSize;
                ammoInReserve -= ammoNeeded;
            }            
        }

        if (inputControls.Player.GunPositionAiming.triggered)
        {
            wantAim = !wantAim;
        }

    }

    IEnumerator ShootGun()
    {
        muzzleFlashParticleEffect.Play();

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

        if (wantAim)
        {
            target = aimingLocalPosition;
            Vector3 desiredPosition = Vector3.Slerp(transform.localPosition, target, smoothTiming * Time.fixedDeltaTime);
            transform.localPosition = desiredPosition;
            target = normalLocalPosition;
        }
        if (!wantAim)
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

            Enemy barrels = hit.transform.GetComponent<Enemy>();
           
            if(barrels != null)
            {
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * ImpactForceToEnemy);
                }

                GameObject killEffect = Instantiate(enemyKillParticleEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(killEffect, 1.0f);

                barrels.TakeDamage(damage);

            }
            
            else
            {
                Instantiate(bulletImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
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
