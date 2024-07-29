using UnityEngine;
using UnityEngine.UI;

public class HealthUIReceiver : MonoBehaviour
{
    Image healthBar;
    HealthController healthController;

    /// <summary>
    /// Initializes the health bar and health controller references.
    /// Subscribes to health-related events.
    /// </summary>
    void Start()
    {
        if (healthBar == null)
            healthBar = GetComponent<Image>();

        if (healthController == null)
            healthController = PlayerCharacter.PlayerInstance.PlayerHealthController;

        if (healthController != null)
        {
            healthController.OnTakeDamage.AddListener(OnDamageTaken);
            healthController.OnGainHealth.AddListener(OnHealthGained);
        }

        // Initialize the health bar fill amount
        float startHealth = healthController.CurrentHealth;
        UpdateHealthBar(startHealth);
    }

    /// <summary>
    /// Unsubscribes from health-related events when the object is disabled.
    /// </summary>
    void OnDisable()
    {
        if (healthController != null)
        {
            healthController.OnTakeDamage.RemoveListener(OnDamageTaken);
            healthController.OnGainHealth.RemoveListener(OnHealthGained);
        }
    }

    /// <summary>
    /// Updates the health bar UI when damage is taken.
    /// </summary>
    /// <param name="healthDamager">The source of the damage.</param>
    /// <param name="healthController">The health controller of the player.</param>
    void OnDamageTaken(HealthDamager healthDamager, HealthController healthController)
    {
        UpdateHealthBar(healthController.CurrentHealth);
    }

    /// <summary>
    /// Updates the health bar UI when health is gained.
    /// </summary>
    /// <param name="amount">The amount of health gained.</param>
    /// <param name="healthController">The health controller of the player.</param>
    void OnHealthGained(float amount, HealthController healthController)
    {
        UpdateHealthBar(healthController.CurrentHealth);
    }

    /// <summary>
    /// Updates the fill amount of the health bar UI element.
    /// </summary>
    /// <param name="currentHealth">The current health value.</param>
    void UpdateHealthBar(float currentHealth)
    {
        if (healthBar != null)
            healthBar.fillAmount = currentHealth;
    }
}
