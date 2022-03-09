using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashBlindness : MonoBehaviour
{
    CanvasGroup cg;

    float effectStrength;
    float effectDuration;

    Coroutine effectCoroutine;

    bool isFlashed = false;

    public static FlashBlindness activeInstance;

    void Start()
    {
        activeInstance = this;
        cg = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (isFlashed)
        {
            // effectCoroutine = StartCoroutine(ReduceOpacity());
            cg.alpha = Mathf.MoveTowards(cg.alpha, 0f, (1/effectDuration) * Time.deltaTime);
            if (cg.alpha == 0f)
            {
                isFlashed = false;
            }
        }
    }

    public void GoBlind(float distance)
    {
        effectStrength = Mathf.Max(0f, 1f - 0.8f * distance);
        isFlashed = true;
        if (effectStrength > 0.7)
        {
            effectStrength = 1f;
        }
        cg.alpha = effectStrength;
        SetDuration();

        // if (effectCoroutine != null)
        // {
        //     StopCoroutine(effectCoroutine);
        // }
    }

    void SetDuration()
    {
        if (effectStrength > 0.85)
        {
            effectDuration = 15f;
        }
        else if (effectStrength > 0.7)
        {
            effectDuration = 10f;
        }
        else if (effectStrength > 0.6)
        {
            effectDuration = 8f;
        }
        else if (effectStrength > 0.5)
        {
            effectDuration = 5f;
        }
        else if (effectStrength > 0.4)
        {
            effectDuration = 3f;
        }
        else
        {
            effectDuration = 2f;
        }
    }

    // IEnumerator ReduceOpacity()
    // {
    //     for (float t = 0f; t < effectDuration; t += Time.deltaTime)
    //     {
    //         float normalizedTime = t / effectDuration;

    //         cg.alpha = Mathf.Lerp(effectStrength, 0f, normalizedTime);
    //         yield return null;
    //     }
    //     cg.alpha = 0;
    //     isFlashed = false;
    // }
}




