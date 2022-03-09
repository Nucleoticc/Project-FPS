using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeWeapon : Weapon
{
    PlayerUIManager playerUIManager;

    GrenadeItem grenadeItem;
    Grenade grenade;

    Transform grenadeSpawnPoint;

    bool isOnCooldown = false;

    [HideInInspector] public GameObject instantiatedGrenade;

    //Animator Hash
    int IsCookingHash;
    int IsOnThrowLoopHash;

    bool isCooking = false;

    void Start()
    {
        playerUIManager = GetComponentInParent<PlayerUIManager>();

        IsCookingHash = Animator.StringToHash("IsCooking");
        IsOnThrowLoopHash = Animator.StringToHash("IsOnThrowLoop");
    }

    void Update()
    {
        HandleAttack();
    }

    public void UpdateGrenadeSettings(GrenadeItem item, Transform grenadeSpawnPoint)
    {
        grenadeItem = item;
        this.grenadeSpawnPoint = grenadeSpawnPoint;
    }

    protected override void HandleAttack()
    {
        if (inputHandler.primaryFireInput && !isOnCooldown)
        {
            if (weaponInventory.CurrentWeaponAnimator.GetBool("IsInteracting")
            || weaponInventory.CurrentWeaponAnimator.GetBool("IsOnThrowLoop"))
            {
                return;
            }

            if (isCooking)
            {
                return;
            }

            isCooking = true;
            weaponInventory.CurrentWeaponAnimator.SetBool(IsCookingHash, isCooking);
            weaponInventory.CurrentWeaponAnimator.SetBool(IsOnThrowLoopHash, true);

            playerAnimationHandler.PlayTargetAnimation("Throw_Start", false, weaponInventory.CurrentWeaponAnimator);
        }
        else if (!inputHandler.primaryFireInput)
        {
            isCooking = false;
            weaponInventory.CurrentWeaponAnimator.SetBool(IsCookingHash, isCooking);
        }
    }

    protected override void HandleInspect() { }

    public void InstantiateGrenade()
    {
        //Instantiating grenade
        instantiatedGrenade = Instantiate(grenadeItem.physicalObjectPrefab, grenadeSpawnPoint);
        instantiatedGrenade.transform.parent = grenadeSpawnPoint.transform;

        //Setting Grenade Settings
        grenade = instantiatedGrenade.GetComponent<Grenade>();
        grenade.delay = grenadeItem.delay;
        grenade.explosionVFX = grenadeItem.explosionVFX;
        grenade.explosionRadius = grenadeItem.explosionRadius;
        grenade.baseDamage = grenadeItem.baseDamage;
        grenade.grenadeWeapon = this;
    }

    public void ActivateRigidBodyAndThrowGrenade()
    {
        if (instantiatedGrenade == null)
        {
            return;
        }

        grenade.isStillInHand = false;

        StartCooldown();

        instantiatedGrenade.transform.parent = null;
        instantiatedGrenade.transform.LookAt(Camera.main.ViewportToWorldPoint(new Vector3(0.49f, 0.55f, 25f)));
        Rigidbody rb = instantiatedGrenade.AddComponent<Rigidbody>() as Rigidbody;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.mass = 0.1f;
        rb.drag = 0.5f;
        rb.AddForce(instantiatedGrenade.transform.forward * grenadeItem.throwForce, ForceMode.Force);
    }

    public void StartCooldown()
    {
        if (!isOnCooldown)
        {
            if (grenadeItem.itemName == "Frag Grenade")
            {
                playerUIManager.FragGrenadeCooldown(grenadeItem.cooldown);
            }
            else if (grenadeItem.itemName == "FlashBang Grenade")
            {
                playerUIManager.FlashGrenadeCooldown(grenadeItem.cooldown);
            }
            else if (grenadeItem.itemName == "Smoke Grenade")
            {
                playerUIManager.SmokeGrenadeCooldown(grenadeItem.cooldown);
            }

            isOnCooldown = true;
            Invoke("EndCooldown", grenadeItem.cooldown);
        }
    }

    void EndCooldown()
    {
        isOnCooldown = false;
    }
}