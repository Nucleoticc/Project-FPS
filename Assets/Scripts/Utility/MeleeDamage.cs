using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    Collider meleeDamageCollider;

    int damageOnAttack;

    void Start()
    {
        meleeDamageCollider = GetComponentInChildren<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"MeleeDamage: {other.name}");
        if (other.CompareTag("Enemy"))
        {
            other.GetComponentInParent<EnemyManager>().Damage(damageOnAttack);
        }
    }

    public void ActivateCollider(int damage)
    {
        damageOnAttack = damage;
        meleeDamageCollider.enabled = true;
    }

    public void DeactivateCollider()
    {
        meleeDamageCollider.enabled = false;
    }
}
