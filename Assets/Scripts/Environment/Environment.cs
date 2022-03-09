using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    [SerializeField] GameObject hitmarkerPrefab;
    [SerializeField] GameObject hitmarkerParent;

    [SerializeField] public bool isPenetrable = false;

    public void SpawnHitMarker(Vector3 position, Quaternion rotation)
    {
        GameObject obj = Instantiate(hitmarkerPrefab, position, rotation);
        obj.transform.SetParent(hitmarkerParent.transform);
    }
}
