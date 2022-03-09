using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashGrenade : Grenade
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

        if (CheckVisibility())
        {
            float distance = Vector3.Distance(transform.position, cam.transform.position) / explosionRadius;
            FlashBlindness.activeInstance.GoBlind(distance);
        }
        Destroy(instantiatedVFX, 1f);
    }

    private bool CheckVisibility()
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(cam);
        Vector3 point = transform.position;

        foreach (Plane plane in planes)
        {
            if (plane.GetDistanceToPoint(point) > 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(cam.transform.position, transform.position - cam.transform.position, out hit, explosionRadius))
                {
                    return hit.transform.gameObject == this.gameObject;
                }
            }
            else
            {
                return false;
            }
        }
        return false;
    }
}