using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/C4")]
public class C4Item : Item
{
    [Header("General")]
    [SerializeField] public int killAward = 300;
    [SerializeField] public int plantAward = 300;

    [Header("Damage")]
    [SerializeField] public int baseDamage = 500;
    [SerializeField] public float damageDropoff = 0.90f;

    [Header("Penetration")]
    [SerializeField] public float armorPenetration = 1.00f;
    [SerializeField] public int penetrationPower = 200;

    [Header("Bomb")]
    [SerializeField] public float defuseTime = 10.0f;
    [SerializeField] public float plantTime = 3.0f;
    [SerializeField] public float detonateTime = 40f;
}
