using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthKit : MonoBehaviour
{
    WeaponInventory weaponInventory;
    InputHandler inputHandler;
    PlayerManager playerManager;
    PlayerUIManager playerUIManager;

    HealthKitItem healthKitItem;

    //Animator Hash
    int healthKitHash;

    bool isHealing = false;

    //Tracking Healed Amount
    int healedAmount = 0;

    //Cooldown
    bool isOnCooldown = false;
    WaitForSeconds cooldownWait;

    void Awake()
    {
        weaponInventory = GetComponentInParent<WeaponInventory>();
        inputHandler = GetComponentInParent<InputHandler>();
        playerManager = GetComponentInParent<PlayerManager>();
        playerUIManager = GetComponentInParent<PlayerUIManager>();
    }

    public void UpdateHealthKitSettings(HealthKitItem item)
    {
        healthKitItem = item;
        cooldownWait = new WaitForSeconds(healthKitItem.cooldown);
    }

    void Start()
    {
        healthKitHash = Animator.StringToHash("IsHealing");
    }

    void Update()
    {
        if (inputHandler.healthKitInputStarted && !playerManager.isFullyHealed())
        {
            ActivateHealthKit();
        }
        else if (inputHandler.healthKitInputEnded)
        {
            DeactivateHealthKit();
        }
    }

    void ActivateHealthKit()
    {
        if (isOnCooldown)
        {
            return;
        }

        if (!isHealing)
        {
            weaponInventory.DeactivateWeaponAndActivateHealthKit();
        }
        if (!IsInvoking("HealPlayer"))
        {
            InvokeRepeating("HealPlayer", 1.75f, 0.1f);
        }
        isHealing = true;
        isOnCooldown = true;
        weaponInventory.healthKitAnimator.SetBool(healthKitHash, true);
    }

    void DeactivateHealthKit()
    {
        inputHandler.healthKitInputEnded = false;
        if (!isHealing)
        {
            return;
        }

        isHealing = false;
        StartCoroutine(Cooldown());
        playerUIManager.healthKitCooldown(healthKitItem.cooldown);
        weaponInventory.healthKitAnimator.SetBool(healthKitHash, false);
        CancelInvoke("HealPlayer");
        healedAmount = 0;
    }

    void HealPlayer()
    {
        healedAmount += healthKitItem.healthRegenPerSecond;
        playerManager.Heal(healthKitItem.healthRegenPerSecond);
        if (healedAmount >= healthKitItem.totalHealthRegen || playerManager.isFullyHealed())
        {
            DeactivateHealthKit();
            inputHandler.healthKitInputStarted = false;
            inputHandler.healthKitInputEnded = false;
        }
    }

    IEnumerator Cooldown()
    {
        yield return cooldownWait;
        isOnCooldown = false;
    }
}
