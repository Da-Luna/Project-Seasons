using System;
using UnityEngine;
using UnityEngine.Events;

public class AetherController : MonoBehaviour, IDataPersister
{
    [Serializable]
    public class AetherEvent : UnityEvent
    { }

    [Serializable]
    public class AetherFillEvent : UnityEvent<float>
    { }

    [Tooltip("The initial aether value.")]
    public float startingAether = 0.5f;

    [Tooltip("The max aether value.")]
    public float maxAether = 1.0f;

    [HideInInspector]
    public DataSettings dataSettings;

    protected float m_CurrentAether;
    protected bool m_ResetAetherOnSceneReload;
    protected float m_Timer;

    /// <summary>
    /// Gets the current aether value.
    /// </summary>
    public float CurrentAether
    {
        get { return m_CurrentAether; }
    }

    [Tooltip("Event triggered when aether is set.")]
    public AetherEvent OnAetherSet;

    [Tooltip("Event triggered when the object gains aether.")]
    public AetherFillEvent OnGainAether;

    void OnEnable()
    {
        PersistentDataManager.RegisterPersister(this);

        m_CurrentAether = startingAether;

        OnAetherSet.Invoke();
    }

    /// <summary>
    /// Unregisters this instance from the PersistentDataManager.
    /// </summary>
    void OnDisable()
    {
        PersistentDataManager.UnregisterPersister(this);
    }

    void Update()
    {
        if (m_CurrentAether < 1f)
        {
            if (m_Timer > 0f)
            {
                m_Timer -= Time.deltaTime;
            }
            else if (m_Timer <= 0f)
            {
                float aetherValue = 0.0042f;
                m_CurrentAether += aetherValue;

                OnAetherSet.Invoke();
                OnGainAether.Invoke(aetherValue);

                m_Timer = 1f;
            }
        }
    }

    /// <summary>
    /// Increases the aether of the object.
    /// </summary>
    /// <param name="amount">The amount of aether to gain.</param>
    public void GainAether(float amount)
    {
        m_CurrentAether += amount;

        if (m_CurrentAether > maxAether)
            m_CurrentAether = maxAether;

        OnAetherSet.Invoke();
        OnGainAether.Invoke(amount);
    }

    /// <summary>
    /// Sets the aether of the object to a specified amount.
    /// </summary>
    /// <param name="amount">The amount to set the aether to.</param>
    public void SetAether(float amount)
    {
        m_CurrentAether = amount;
        OnAetherSet.Invoke();
    }

    public DataSettings GetDataSettings()
    {
        return dataSettings;
    }

    public void SetDataSettings(string dataTag, DataSettings.PersistenceType persistenceType)
    {
        dataSettings.dataTag = dataTag;
        dataSettings.persistenceType = persistenceType;
    }

    public Data SaveData()
    {
        return new Data<float, bool>(CurrentAether, m_ResetAetherOnSceneReload);
    }

    public void LoadData(Data data)
    {
        Data<float, bool> healthData = (Data<float, bool>)data;
        m_CurrentAether = healthData.value1 ? startingAether : healthData.value0;
        OnAetherSet.Invoke();
    }
}
