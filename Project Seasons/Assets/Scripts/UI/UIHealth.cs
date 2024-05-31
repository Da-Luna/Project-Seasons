using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    Image healthBar;
    DamageController damageController;

    void Start()
    {
        if (healthBar == null)
            healthBar = GetComponent<Image>();

        if (damageController == null)
            damageController = PlayerCharacter.PlayerInstance.PlayerDamageable;

        if (damageController != null)
        {
            damageController.OnTakeDamage.AddListener(OnDamageTaken);
            damageController.OnGainHealth.AddListener(OnHealthGained);
        }

        // Initialize the health bar fill amount
        UpdateHealthBar(damageController.CurrentHealth);
    }

    void OnDisable()
    {
        if (damageController != null)
        {
            damageController.OnTakeDamage.RemoveListener(OnDamageTaken);
            damageController.OnGainHealth.RemoveListener(OnHealthGained);
        }
    }

    // Method to handle the OnTakeDamage event
    void OnDamageTaken(Damager damager, DamageController damageController)
    {
        UpdateHealthBar(damageController.CurrentHealth);
    }

    // Method to handle the OnGainHealth event
    void OnHealthGained(float amount, DamageController damageController)
    {
        UpdateHealthBar(damageController.CurrentHealth);
    }

    // Method to update the health bar
    void UpdateHealthBar(float currentHealth)
    {
        if (healthBar != null)
            healthBar.fillAmount = currentHealth;
    }
}
