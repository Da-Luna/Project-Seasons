using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The HealthController class represents an object that can take damage and gain health.
/// It supports events for taking damage, healing, and dying, and manages invulnerability states.
/// </summary>
public class HealthController : MonoBehaviour
{
    [Serializable] public class HealthEvent : UnityEvent<HealthController> { }

    [Serializable] public class DamageEvent : UnityEvent<HealthDamager, HealthController> { }

    [Serializable] public class HealEvent : UnityEvent<float, HealthController> { }

    [Header("HEALTH SETTINGS")]

    [Tooltip("The initial health value at start.")]
    public float startingHealth = 0.5f;

    [Tooltip("The max health value.")]
    public float maxHealth = 1.0f;

    [Header("DAMAGE REACTION SETTINGS")]

    [Tooltip("Whether the object becomes invulnerable after taking damage.")]
    public bool invulnerableAfterDamage = true;

    [Tooltip("Duration of the invulnerability period after taking damage.")]
    public float invulnerabilityDuration = 3f;

    [Tooltip("Whether to disable the object upon death.")]
    public bool disableOnDeath = false;

    [Tooltip("An offset from the object position used to set from where the distance to the damager is computed.")]
    public Vector2 centreOffset = new(0f, 1f);

    [Header("EVENTS")]
    [Space]

    [Tooltip("Event triggered when health is set.")]
    public HealthEvent OnHealthSet;

    [Tooltip("Event triggered when the object takes damage.")]
    public DamageEvent OnTakeDamage;

    [Tooltip("Event triggered when the object dies.")]
    public DamageEvent OnDie;

    [Tooltip("Event triggered when the object gains health.")]
    public HealEvent OnGainHealth;

    protected bool m_Invulnerable;
    protected float m_CurrentHealth;
    protected Vector2 m_DamageDirection;
    protected bool m_ResetHealthOnSceneReload;

    /// <summary>
    /// Registers this instance with the PersistentDataManager and initializes health and aether values.
    /// </summary>
    void OnEnable()
    {
        m_CurrentHealth = startingHealth;

        OnHealthSet.Invoke(this);

        DisableInvulnerability();
    }

    /// <summary>
    /// Start invulnerability for the object.
    /// </summary>
    public void EnableInvulnerability()
    {
        if (invulnerableAfterDamage)
        {
            m_Invulnerable = true;
            StartCoroutine(HandleInvulnerability());
        }
    }

    /// <summary>
    /// Disables invulnerability for the object.
    /// </summary>
    public void DisableInvulnerability()
    {
        m_Invulnerable = false;
    }

    /// <summary>
    /// Handle invulnerability.
    /// </summary>
    IEnumerator HandleInvulnerability()
    {
        yield return new WaitForSeconds(invulnerabilityDuration);
        DisableInvulnerability();
    }

    /// <summary>
    /// Gets the current health value.
    /// </summary>
    public float CurrentHealth
    {
        get { return m_CurrentHealth; }
    }

    /// <summary>
    /// Gets the direction of the damage taken.
    /// </summary>
    /// <returns>The direction of the damage.</returns>
    public Vector2 GetDamageDirection()
    {
        return m_DamageDirection;
    }

    /// <summary>
    /// Applies damage to the object.
    /// </summary>
    /// <param name="damager">The source of the damage.</param>
    /// <param name="ignoreInvincible">If true, ignores the invincible state.</param>
    public void TakeDamage(HealthDamager damager, bool ignoreInvincible = false)
    {
        if ((m_Invulnerable && !ignoreInvincible) || m_CurrentHealth <= 0)
            return;

        // We can reach that point if the damager was one that was ignoring the invincible state.
        // We still want the callback that we were hit, but not the damage to be removed from health.
        if (!m_Invulnerable)
        {
            m_CurrentHealth -= damager.damage;
            OnHealthSet.Invoke(this);
        }

        m_DamageDirection = transform.position + (Vector3)centreOffset - damager.transform.position;

        CheckIfDead();

        OnTakeDamage.Invoke(damager, this);
    }

    /// <summary>
    /// Increases the health of the object.
    /// </summary>
    /// <param name="amount">The amount of health to gain.</param>
    public void GainHealth(float amount)
    {
        m_CurrentHealth += amount;

        if (m_CurrentHealth > maxHealth)
            m_CurrentHealth = maxHealth;

        OnHealthSet.Invoke(this);
        
        OnGainHealth.Invoke(amount, this);
    }

    /// <summary>
    /// Sets the health of the object to a specified amount.
    /// </summary>
    /// <param name="amount">The amount to set the health to.</param>
    public void SetHealth(float amount)
    {
        m_CurrentHealth = amount;
        CheckIfDead();

        OnHealthSet.Invoke(this);
    }

    void CheckIfDead()
    {
        if (m_CurrentHealth <= 0)
        {
            EnableInvulnerability();
            m_ResetHealthOnSceneReload = true;

            if (disableOnDeath)
            {
                gameObject.SetActive(false);
            }

            OnDie.Invoke(null, this);
        }
    }
}
