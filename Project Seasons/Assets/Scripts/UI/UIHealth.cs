using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    Image healthBar;
    Damageable damageable;

    void Start()
    {
        if (healthBar == null)
            healthBar = GetComponent<Image>();

        if (damageable == null)
            damageable = PlayerCharacter.PlayerInstance.PlayerDamageable;

        if (damageable != null)
        {
            damageable.OnTakeDamage.AddListener(OnDamageTaken);
            damageable.OnGainHealth.AddListener(OnHealthGained);
        }

        // Initialize the health bar fill amount
        UpdateHealthBar(damageable.CurrentHealth);
    }

    void OnDisable()
    {
        if (damageable != null)
        {
            damageable.OnTakeDamage.RemoveListener(OnDamageTaken);
            damageable.OnGainHealth.RemoveListener(OnHealthGained);
        }
    }

    // Method to handle the OnTakeDamage event
    void OnDamageTaken(Damager damager, Damageable damageable)
    {
        UpdateHealthBar(damageable.CurrentHealth);
    }

    // Method to handle the OnGainHealth event
    void OnHealthGained(float amount, Damageable damageable)
    {
        UpdateHealthBar(damageable.CurrentHealth);
    }

    // Method to update the health bar
    void UpdateHealthBar(float currentHealth)
    {
        if (healthBar != null)
            healthBar.fillAmount = currentHealth / damageable.startingHealth;
    }
}
