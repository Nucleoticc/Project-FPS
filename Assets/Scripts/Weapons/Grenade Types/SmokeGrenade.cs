using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenade : Grenade
{
    protected override void Start()
    {
        base.Start();
        Invoke("Explode", delay);
    }

    protected override void Explode()
    {
        base.Explode();

        Destroy(gameObject);
        Destroy(instantiatedVFX, 16f);
    }
}
