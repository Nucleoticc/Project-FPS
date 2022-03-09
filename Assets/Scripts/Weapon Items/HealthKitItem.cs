using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Health Kit")]
public class HealthKitItem : Item
{
    [Header("General")]
    [SerializeField] public int totalHealthRegen = 50;
    [SerializeField] public int healthRegenPerSecond = 1;

    [Header("Cooldown")]
    [SerializeField] public float cooldown = 5f;
}
