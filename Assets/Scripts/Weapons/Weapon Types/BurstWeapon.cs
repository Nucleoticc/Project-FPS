using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstWeapon : GunWeapon
{
    bool isShooting = false;

    WaitForSeconds waitForSeconds;
    Coroutine burstCoroutine;

    void Start()
    {
        waitForSeconds = new WaitForSeconds(currentWeapon.fireRate);
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
        if (!inputHandler.primaryFireInput && !isShooting)
        {
            //reset camera rotation to 0 over time
            cam.transform.localRotation = Quaternion.Slerp(cam.transform.localRotation, Quaternion.identity, 8f * Time.deltaTime);
            currentRecoilIndex = 0;
        }

        if (weaponInventory.CurrentWeaponAnimator.GetBool("IsInteracting") ||
            weaponInventory.CurrentWeaponAnimator.GetBool("IsReloading"))
        {
            return;
        }

        if (inputHandler.primaryFireInput && CurrentAmmo > 0)
        {
            inputHandler.primaryFireInput = false;
            if (!isShooting)
            {
                Shoot();
            }
        }
    }

    protected override void Shoot()
    {
        if (burstCoroutine != null)
        {
            StopCoroutine(burstCoroutine);
        }
        burstCoroutine = StartCoroutine(BurstFire());
    }

    IEnumerator BurstFire()
    {
        isShooting = true;
        for (int i = 0; i < 3; i++)
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
                ShootWithRayCast(startPos, direction);
                currentRecoilIndex += 1;
                if (currentRecoilIndex >= currentWeapon.recoil.Count)
                {
                    currentRecoilIndex = 0;
                }
                yield return waitForSeconds;
            }
        }
        isShooting = false;
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
