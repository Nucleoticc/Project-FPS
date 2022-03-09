using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    InputHandler inputHandler;

    [SerializeField] float smooth;
    [SerializeField] float swayMultiplier;

    void Awake()
    {
        inputHandler = GetComponentInParent<InputHandler>();
    }

    void FixedUpdate()
    {
        float mouseX = inputHandler.cameraInput.x * swayMultiplier;

        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        if (Mathf.Abs(mouseX) > 0)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, rotationY, smooth * 0.003f);
        }
        else
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, smooth * 0.003f);
        }
    }
}
