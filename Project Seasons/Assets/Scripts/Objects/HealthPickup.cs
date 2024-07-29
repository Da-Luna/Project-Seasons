using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthPickup : MonoBehaviour
{
    [SerializeField]
    float healthAmount = 0.25f;

    [SerializeField]
    UnityEvent OnGivingHealth;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == PlayerCharacter.PlayerInstance.gameObject)
        {
            HealthController damageController = PlayerCharacter.PlayerInstance.PlayerHealthController;

            if (damageController.CurrentHealth < damageController.maxHealth)
            {
                damageController.GainHealth(healthAmount);
                OnGivingHealth.Invoke();
            }
        }
    }
}
