using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    InputHandler inputHandler;

    [Header("Health")]
    [SerializeField] int health;
    [SerializeField] int maxHealth = 100;
    [SerializeField] Slider healthSlider;

    void Awake()
    {
        inputHandler = GetComponent<InputHandler>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Start()
    {
        health = maxHealth;
        UpdateHealthUI();
    }

    void Update()
    {
        ResetScene();
    }

    void ResetScene()
    {
        if (inputHandler.ResetGameInput)
        {
            inputHandler.ResetGameInput = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void Damage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        UpdateHealthUI();
    }

    public void Heal(int heal)
    {
        health += heal;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        healthSlider.value = health;
    }

    public bool isFullyHealed()
    {
        return health == maxHealth;
    }
}
