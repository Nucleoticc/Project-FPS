using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SniperWeapon : GunWeapon
{
    Image scopeOverlay;
    GameObject scopeCamera;
    Camera mainCamera;

    Coroutine scopedCoroutine;
    WaitForSeconds scopeTime;
    bool isScoped = false;

    [Header("Scopes")]
    [SerializeField] float scopedFOV = 15f;
    [SerializeField] float normalFOV = 77f;

    void Start()
    {
        cam = Camera.main;
        scopeCamera = Camera.allCameras[1].gameObject;
        scopeOverlay = GameObject.FindGameObjectWithTag("ScopeOverlay").GetComponent<Image>();
        scopeTime = new WaitForSeconds(0.30f);
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
            //reset camera rotation to 0 over time
            cam.transform.localRotation = Quaternion.Slerp(cam.transform.localRotation, Quaternion.identity, 8f * Time.deltaTime);
        }

        if (inputHandler.secondaryFireInput)
        {
            isScoped = true;
            weaponInventory.CurrentWeaponAnimator.SetBool("IsAiming", true);
            if (scopedCoroutine == null)
            {
                scopedCoroutine = StartCoroutine(HandleScope());
            }
        }
        else
        {
            weaponInventory.CurrentWeaponAnimator.SetBool("IsAiming", false);
            CloseScope();
            if (scopedCoroutine != null)
            {
                StopCoroutine(scopedCoroutine);
                scopedCoroutine = null;
            }
        }

        if (weaponInventory.CurrentWeaponAnimator.GetBool("IsInteracting") ||
            weaponInventory.CurrentWeaponAnimator.GetBool("IsReloading"))
        {
            return;
        }

        if (inputHandler.primaryFireInput && CurrentAmmo > 0)
        {
            inputHandler.primaryFireInput = false;
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
            Vector3 direction = cam.transform.forward;
            if (!isScoped)
            {
                direction = HandleBulletSpread();
            }
            HandleRecoil();
            playerAnimationHandler.PlayTargetAnimation("Shot", false, weaponInventory.CurrentWeaponAnimator);
            ShootWithRaycastAndPenetrate(startPos, direction, true);
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

    IEnumerator HandleScope()
    {
        yield return scopeTime;
        scopeOverlay.enabled = true;
        scopeCamera.SetActive(false);
        cam.fieldOfView = scopedFOV;
    }

    public void CloseScope()
    {
        scopeOverlay.enabled = false;
        scopeCamera.SetActive(true);
        cam.fieldOfView = normalFOV;
    }
}
