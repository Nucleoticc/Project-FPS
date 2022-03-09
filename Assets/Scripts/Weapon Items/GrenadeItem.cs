using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Grenade")]
public class GrenadeItem : Item
{
    [Header("General")]
    [SerializeField] public GameObject physicalObjectPrefab;
    [SerializeField] public int killAward = 300;
    [SerializeField] public int throwForce = 200;

    [Header("Cooldown")]
    [SerializeField] public float cooldown = 5f;

    [Header("Damage")]
    [SerializeField] public int baseDamage = 98;

    [Header("Delay")]
    [SerializeField] public float delay = 5f;

    [Header("Explosion")]
    [SerializeField] public GameObject explosionVFX;
    [SerializeField] public float explosionRadius = 5f;
}
