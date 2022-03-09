using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay { get; set; }
    public GameObject explosionVFX { get; set; }
    public float explosionRadius { get; set; }
    public int baseDamage { get; set; }

    public GrenadeWeapon grenadeWeapon { get; set; }

    public bool isStillInHand = true;

    protected GameObject instantiatedVFX;

    protected Camera cam;

    protected virtual void Start()
    {
        cam = Camera.main;
    }

    protected virtual void Explode()
    {
        grenadeWeapon.StartCooldown();

        Vector3 instantiatedVFXPosition = transform.position;

        if (isStillInHand)
        {
            instantiatedVFXPosition = cam.transform.position;
        }

        instantiatedVFX = Instantiate(explosionVFX, instantiatedVFXPosition + new Vector3(0, 0.5f, 0), Quaternion.identity);
    }
}
