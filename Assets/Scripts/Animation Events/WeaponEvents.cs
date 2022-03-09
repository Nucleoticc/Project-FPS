using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEvents : MonoBehaviour
{
    WeaponInventory weaponInventory;

    void Awake()
    {
        weaponInventory = GetComponentInParent<WeaponInventory>();
    }

    void SwitchWeapon()
    {
        weaponInventory.ActivateWeapon();
    }

    void Reload()
    {
        GunWeapon currentWeapon = weaponInventory.currentWeapon as GunWeapon;
        if (currentWeapon.gunType == Item.GunType.Pistol)
        {
            PistolWeapon weapon = weaponInventory.currentWeapon as PistolWeapon;
            weapon.Reload();
        }
        else if (currentWeapon.gunType == Item.GunType.Rifle)
        {
            RifleWeapon weapon = weaponInventory.currentWeapon as RifleWeapon;
            weapon.Reload();
        }
        else if (currentWeapon.gunType == Item.GunType.Shotgun)
        {
            ShotgunWeapon weapon = weaponInventory.currentWeapon as ShotgunWeapon;
            weapon.Reload();
        }
        else if (currentWeapon.gunType == Item.GunType.SMG)
        {
            BurstWeapon weapon = weaponInventory.currentWeapon as BurstWeapon;
            weapon.Reload();
        }
        else if (currentWeapon.gunType == Item.GunType.Sniper)
        {
            SniperWeapon weapon = weaponInventory.currentWeapon as SniperWeapon;
            weapon.Reload();
        }
    }

    void OpenMeleeDamageCollider()
    {
        MeleeWeapon weapon = weaponInventory.currentWeapon as MeleeWeapon;
        weapon.OpenMeleeDamageCollider();
    }

    void CloseMeleeDamageCollider()
    {
        MeleeWeapon weapon = weaponInventory.currentWeapon as MeleeWeapon;
        weapon.CloseMeleeDamageCollider();
    }

    void InstantiateGrenade()
    {
        GrenadeWeapon weapon = weaponInventory.currentWeapon as GrenadeWeapon;
        weapon.InstantiateGrenade();
    }

    void ThrowGrenade()
    {
        GrenadeWeapon weapon = weaponInventory.currentWeapon as GrenadeWeapon;
        weapon.ActivateRigidBodyAndThrowGrenade();
    }
}
