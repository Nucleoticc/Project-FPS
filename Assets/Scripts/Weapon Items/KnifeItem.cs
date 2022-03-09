using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Knife")]
public class KnifeItem : Item
{
    [Header("General")]
    [SerializeField] public FireMode firingMode = FireMode.Slash;
    [SerializeField] public int killAward = 1500;

    [Header("Fire Rate")]
    [SerializeField] public float primaryFireRate = 0.4f;
    [SerializeField] public float secondaryFireRate = 1f;

    [Header("Damage")]
    [SerializeField] public int primaryBaseDamage = 34;
    [SerializeField] public int secondaryBaseDamage = 55;

    [Header("Penetration")]
    [SerializeField] public float armorPenetration = 0.85f;
}
