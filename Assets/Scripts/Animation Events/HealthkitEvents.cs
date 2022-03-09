using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthkitEvents : MonoBehaviour
{
    WeaponInventory weaponInventory;

    void Awake()
    {
        weaponInventory = GetComponentInParent<WeaponInventory>();
    }

    void ReactivateWeapon()
    {
        weaponInventory.ReactivateWeapon();
    }
}
