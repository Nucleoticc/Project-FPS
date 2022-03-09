using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    KnifeItem knifeItem;
    MeleeDamage meleeDamage;

    int damageOnAttack;

    void Update()
    {
        HandleAttack();
        HandleInspect();
    }

    public void UpdateMeleeSettings(KnifeItem weapon, MeleeDamage meleeDamage)
    {
        knifeItem = weapon;
        this.meleeDamage = meleeDamage;
    }

    protected override void HandleAttack()
    {
        if (weaponInventory.CurrentWeaponAnimator.GetBool("IsInteracting"))
        {
            return;
        }

        if (inputHandler.primaryFireInput)
        {
            damageOnAttack = knifeItem.primaryBaseDamage;
            playerAnimationHandler.PlayTargetAnimation("Attack_01", true, weaponInventory.CurrentWeaponAnimator);
        }
        else if (inputHandler.secondaryFireInput)
        {
            damageOnAttack = knifeItem.secondaryBaseDamage;
            playerAnimationHandler.PlayTargetAnimation("Attack_02", true, weaponInventory.CurrentWeaponAnimator);
        }
    }

    public void OpenMeleeDamageCollider()
    {
        meleeDamage.ActivateCollider(damageOnAttack);
    }

    public void CloseMeleeDamageCollider()
    {
        meleeDamage.DeactivateCollider();
    }
}
