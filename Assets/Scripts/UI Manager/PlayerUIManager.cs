using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public GunWeapon CurrentWeapon { get; set; } = null;

    [Header("Ammo and Weapon")]
    [SerializeField] Text ammoCounterText;

    [Header("HealthKit")]
    [SerializeField] Image healthKitImage;

    [Header("Crosshair")]
    [SerializeField] Image crosshairImage;

    [Header("Grenade")]
    [SerializeField] Image fragGrenadeImage;
    [SerializeField] Image flashGrenadeImage;
    [SerializeField] Image smokeGrenadeImage;

    Color transparentColor;

    void Start()
    {
        transparentColor = new Color(1, 1, 1, 0);
    }

    void Update()
    {
        if (CurrentWeapon != null)
        {
            ammoCounterText.text = $"{CurrentWeapon.CurrentAmmo}/{CurrentWeapon.ReserveAmmo}";
        }
        else
        {
            ammoCounterText.text = "";
        }
    }

    public void healthKitCooldown(float duration)
    {
        healthKitImage.fillAmount = 0;
        StartCoroutine(healthKitCooldownCoroutine(duration));
    }

    IEnumerator healthKitCooldownCoroutine(float seconds)
    {
        float animationTime = 0f;
        while (animationTime < seconds)
        {
            animationTime += Time.deltaTime;
            float lerpValue = animationTime / seconds;
            healthKitImage.fillAmount = Mathf.Lerp(0, 1, lerpValue);
            yield return null;
        }

        healthKitImage.fillAmount = 1;
    }

    public void FragGrenadeCooldown(float duration)
    {
        fragGrenadeImage.color = transparentColor;
        StartCoroutine(GrenadeCooldownCoroutine(fragGrenadeImage, duration));
    }

    public void FlashGrenadeCooldown(float duration)
    {
        flashGrenadeImage.color = transparentColor;
        StartCoroutine(GrenadeCooldownCoroutine(flashGrenadeImage, duration));
    }

    public void SmokeGrenadeCooldown(float duration)
    {
        smokeGrenadeImage.color = transparentColor;
        StartCoroutine(GrenadeCooldownCoroutine(smokeGrenadeImage, duration));
    }

    IEnumerator GrenadeCooldownCoroutine(Image grenadeImage, float seconds)
    {
        float animationTime = 0f;
        while (animationTime < seconds)
        {
            animationTime += Time.deltaTime;
            float lerpValue = animationTime / seconds;
            grenadeImage.color = Color.Lerp(transparentColor, Color.white, lerpValue);
            yield return null;
        }

        grenadeImage.color = Color.white;
    }

}
