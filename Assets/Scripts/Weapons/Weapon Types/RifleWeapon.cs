using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleWeapon : GunWeapon
{
    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        HandleAttack();
        HandleReloading();
        HandleInspect();
    }

    protected override void HandleAttack()
    {
        if (!inputHandler.primaryFireInput)
        {
            recoilResetTime -= Time.deltaTime;
            if (recoilResetTime <= 0)
            {
                currentRecoilIndex = 0;
            }

            //reset camera rotation to 0 over time
            cam.transform.localRotation = Quaternion.Slerp(cam.transform.localRotation, Quaternion.identity, 8f * Time.deltaTime);
        }

        if (weaponInventory.CurrentWeaponAnimator.GetBool("IsInteracting") ||
            weaponInventory.CurrentWeaponAnimator.GetBool("IsReloading"))
        {
            return;
        }

        if (inputHandler.primaryFireInput && CurrentAmmo > 0)
        {
            Shoot();
        }
    }

    protected override void Shoot()
    {
        if (Time.time > nextFire)
        {
            InstantiateMuzzleFlash();
            CurrentAmmo--;
            nextFire = Time.time + currentWeapon.fireRate;
            Vector3 startPos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
            Vector3 direction = HandleBulletSpread();
            HandleRecoil();
            playerAnimationHandler.PlayTargetAnimation("Shot", false, weaponInventory.CurrentWeaponAnimator);
            ShootWithRaycastAndPenetrate(startPos, direction, false);
            recoilResetTime = currentWeapon.recoilResetTime;
            currentRecoilIndex += 1;
            if (currentRecoilIndex >= currentWeapon.recoil.Count)
            {
                currentRecoilIndex = 0;
            }
        }
    }

    public void Reload()
    {
        int ammoToReload = currentWeapon.magSize - CurrentAmmo;
        if (ReserveAmmo >= ammoToReload)
        {
            ReserveAmmo -= ammoToReload;
            CurrentAmmo = currentWeapon.magSize;
        }
        else if (ReserveAmmo < ammoToReload && ReserveAmmo > 0)
        {
            CurrentAmmo += ReserveAmmo;
            ReserveAmmo = 0;
        }
    }
}
