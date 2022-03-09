using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    Animator animator;

    [SerializeField] int maxHealth;
    [SerializeField] Slider healthSlider;

    int health;
    bool isRecoveringHP = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        health = maxHealth;
        UpdateHealthUI();
    }

    public void Damage(int damage)
    {
        animator.CrossFade("Hit", 0.2f);

        if (isRecoveringHP)
        {
            return;
        }

        health -= damage;
        if (health <= 0)
        {
            health = 0;
            isRecoveringHP = true;
            Invoke("Heal", 2f);
        }
        UpdateHealthUI();
    }

    void Heal()
    {
        StartCoroutine(HealCoroutine(1.5f));
    }

    void UpdateHealthUI()
    {
        healthSlider.value = health;
    }

    IEnumerator HealCoroutine(float seconds)
    {
        float animationTime = 0f;
        while (animationTime < seconds)
        {
            animationTime += Time.deltaTime;
            float lerpValue = animationTime / seconds;
            healthSlider.value = Mathf.Lerp(0, maxHealth, lerpValue);
            yield return null;
        }
        healthSlider.value = maxHealth;
        health = maxHealth;
        isRecoveringHP = false;
    }
}
