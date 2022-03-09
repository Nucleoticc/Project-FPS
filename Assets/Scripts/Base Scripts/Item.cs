using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public enum FireMode
    {
        Semi_Automatic,
        Automatic,
        Pump_Action,
        Burst,
        Bolt_Action,
        Slash
    }

    public enum GunType
    {
        Pistol,
        Rifle,
        Shotgun,
        SMG,
        Sniper,
        Grenade
    }

    [Header("Item Information")]
    [SerializeField] public Sprite itemIcon;
    [SerializeField] public string itemName;
    [SerializeField] public GameObject modelPrefab;
    [SerializeField] public GameObject FpsWeaponPrefab;
    [SerializeField] public AudioSource audioSource;

    [Header("General")]
    [SerializeField] public bool isDroppable;
    [SerializeField] public float movementSpeedFactor = 8f;
}
