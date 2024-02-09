using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    InputControls inputControls;

    [SerializeField] int selectedWeapon = 0;

    private void Awake()
    {
        inputControls = new InputControls();
    }

    private void Update()
    {
        CheckSwitching();
    }

    void CheckSwitching()
    {
        if (inputControls.Player.WeaponSwitch.triggered)
        {
            selectedWeapon++;
            if (selectedWeapon > transform.childCount - 1)
            {
                selectedWeapon = 0;
            }
            if(selectedWeapon < 0)
            {
                selectedWeapon = transform.childCount - 1;
            }
            
        }
        WeaponSelection();
    }

    void WeaponSelection()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if(i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }

            i++;
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
