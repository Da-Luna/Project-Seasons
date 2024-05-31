using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The Damageable class represents an object that can take damage and gain health.
/// It supports events for taking damage, healing, and dying, and manages invulnerability states.
/// </summary>
public class DamageController : MonoBehaviour, IDataPersister
{
    [Serializable]
    public class HealthEvent : UnityEvent<DamageController>
    { }

    [Serializable]
    public class DamageEvent : UnityEvent<Damager, DamageController>
    { }

    [Serializable]
    public class HealEvent : UnityEvent<float, DamageController>
    { }

    [Tooltip("The initial health value.")]
    public float startingHealth = 0.5f;

    [Tooltip("The max health value.")]
    public float maxHealth = 1.0f;

    [Tooltip("Whether the object becomes invulnerable after taking damage.")]
    public bool invulnerableAfterDamage = true;

    [Tooltip("Duration of the invulnerability period after taking damage.")]
    public float invulnerabilityDuration = 3f;

    [Tooltip("Whether to disable the object upon death.")]
    public bool disableOnDeath = false;

    [Tooltip("An offset from the object position used to set from where the distance to the damager is computed.")]
    public Vector2 centreOffset = new(0f, 1f);

    [Tooltip("Event triggered when health is set.")]
    public HealthEvent OnHealthSet;

    [Tooltip("Event triggered when the object takes damage.")]
    public DamageEvent OnTakeDamage;

    [Tooltip("Event triggered when the object dies.")]
    public DamageEvent OnDie;

    [Tooltip("Event triggered when the object gains health.")]
    public HealEvent OnGainHealth;

    [HideInInspector]
    public DataSettings dataSettings;

    protected bool m_Invulnerable;
    protected float m_InulnerabilityTimer;
    protected float m_CurrentHealth;
    protected Vector2 m_DamageDirection;
    protected bool m_ResetHealthOnSceneReload;

    /// <summary>
    /// Gets the current health value.
    /// </summary>
    public float CurrentHealth
    {
        get { return m_CurrentHealth; }
    }

    /// <summary>
    /// Registers this instance with the PersistentDataManager and initializes health and aether values.
    /// </summary>
    void OnEnable()
    {
        PersistentDataManager.RegisterPersister(this);

        m_CurrentHealth = startingHealth;

        OnHealthSet.Invoke(this);

        DisableInvulnerability();
    }

    /// <summary>
    /// Unregisters this instance from the PersistentDataManager.
    /// </summary>
    void OnDisable()
    {
        PersistentDataManager.UnregisterPersister(this);
    }

    /// <summary>
    /// Updates the invulnerability timer if the object is invulnerable.
    /// </summary>
    void Update()
    {
        if (m_Invulnerable)
        {
            m_InulnerabilityTimer -= Time.deltaTime;

            if (m_InulnerabilityTimer <= 0f)
            {
                m_Invulnerable = false;
            }
        }
    }

    /// <summary>
    /// Enables invulnerability for the object.
    /// </summary>
    /// <param name="ignoreTimer">If true, invulnerability duration is set to a very large value.</param>
    public void EnableInvulnerability(bool ignoreTimer = false)
    {
        m_Invulnerable = true;
        // Technically don't ignore timer, just set it to an insanely big number.
        // Allow to avoid adding more tests & special cases.
        m_InulnerabilityTimer = ignoreTimer ? float.MaxValue : invulnerabilityDuration;
    }

    /// <summary>
    /// Disables invulnerability for the object.
    /// </summary>
    public void DisableInvulnerability()
    {
        m_Invulnerable = false;
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
    public void TakeDamage(Damager damager, bool ignoreInvincible = false)
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

        OnTakeDamage.Invoke(damager, this);

        if (m_CurrentHealth <= 0)
        {
            OnDie.Invoke(damager, this);
            m_ResetHealthOnSceneReload = true;
            EnableInvulnerability();
            if (disableOnDeath) gameObject.SetActive(false);
        }
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

        if (m_CurrentHealth <= 0)
        {
            OnDie.Invoke(null, this);
            m_ResetHealthOnSceneReload = true;
            EnableInvulnerability();
            if (disableOnDeath) gameObject.SetActive(false);
        }

        OnHealthSet.Invoke(this);
    }

    /// <summary>
    /// Gets the data settings for persistence.
    /// </summary>
    /// <returns>The data settings.</returns>
    public DataSettings GetDataSettings()
    {
        return dataSettings;
    }

    /// <summary>
    /// Sets the data settings for persistence.
    /// </summary>
    /// <param name="dataTag">The data tag.</param>
    /// <param name="persistenceType">The type of persistence.</param>
    public void SetDataSettings(string dataTag, DataSettings.PersistenceType persistenceType)
    {
        dataSettings.dataTag = dataTag;
        dataSettings.persistenceType = persistenceType;
    }

    /// <summary>
    /// Saves the current health and reset state.
    /// </summary>
    /// <returns>The saved data.</returns>
    public Data SaveData()
    {
        return new Data<float, bool>(CurrentHealth, m_ResetHealthOnSceneReload);
    }

    /// <summary>
    /// Loads the health data.
    /// </summary>
    /// <param name="data">The data to load.</param>
    public void LoadData(Data data)
    {
        Data<float, bool> healthData = (Data<float, bool>)data;
        m_CurrentHealth = healthData.value1 ? startingHealth : healthData.value0;
        OnHealthSet.Invoke(this);
    }
}
