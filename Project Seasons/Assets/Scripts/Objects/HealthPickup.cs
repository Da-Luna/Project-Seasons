using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthPickup : MonoBehaviour
{
    [SerializeField]
    float healthAmount = 1;

    [SerializeField]
    UnityEvent OnGivingHealth;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == PlayerCharacter.PlayerInstance.gameObject)
        {
            Damageable damageable = PlayerCharacter.PlayerInstance.PlayerDamageable;
            if (damageable.CurrentHealth < damageable.startingHealth)
            {
                damageable.GainHealth(Mathf.Min(healthAmount, damageable.startingHealth - damageable.CurrentHealth));
                OnGivingHealth.Invoke();
            }
        }
    }
}
