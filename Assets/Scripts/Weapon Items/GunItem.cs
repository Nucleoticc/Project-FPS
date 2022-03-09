using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon")]
public class GunItem : Item
{
    [SerializeField] public GameObject muzzleFlashPrefab;

    [Header("General")]
    [SerializeField] public FireMode firingMode = FireMode.Automatic;
    [SerializeField] public GunType gunType;
    [SerializeField] public int killAward = 300;
    [SerializeField] public float recoilResetTime;

    [Header("Damage")]
    [SerializeField] public int baseDamage = 25;
    [SerializeField] public float range = 0.05f;
    [SerializeField] public float damageFalloffStartFactor = 0.15f;
    [SerializeField] public float penetrationPower = 3f;

    [Header("Reload")]
    [SerializeField] public int magSize = 30;
    [SerializeField] public int reserveAmmo = 90;
    [SerializeField] public AudioSource reloadSound;

    [Header("Fire Rate")]
    [SerializeField] public float fireRate = 0.1f;

    [Header("Recoil")]
    [SerializeField] public Vector2 spread;
    [SerializeField] public List<Vector2> recoil = new List<Vector2>();
}
