using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Defuse Kit")]
public class DefuseKitItem : Item
{
    [Header("General")]
    [SerializeField] public int defuseAward = 500;

    [Header("Defuse")]
    [SerializeField] public float defuseTime = 5.0f;
}
