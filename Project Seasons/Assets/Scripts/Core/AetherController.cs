using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AetherController : MonoBehaviour
{
    [Serializable] public class AetherEvent : UnityEvent { }

    [Serializable] public class AetherFillEvent : UnityEvent<float> { }

    #region REFERENCES

    [Header("REFERENCES")]

    [Tooltip("Prefab for the aether base attack object.")]
    public GameObject uIPrefabAetherLightHeavy;

    [Tooltip("The parent transform that will hold all aether base attack objects.")]
    public Transform parentUIAetherLightHeavy;

    const string uIParentSearch = "ParentAetherBaseAtk";

    #endregion // REFERENCES

    #region LIGHT AND HEAVY ATTACK SETTINGS

    [Header("LIGHT AND HEAVY ATTACK SETTINGS")]

    [Tooltip("The delay in seconds after using a base attack before another can be activated.")]
    public float delayAfterUseLHAttack = 2f;

    [Tooltip("The initial number of aether base attack points to start with.")]
    public int activeAetherLHPoints = 8;

    [Tooltip("The time interval in seconds at which aether base attack points are activated.")]
    public float aetherLHTimeStep = 0.5f;

    protected List<GameObject> m_ActiveAetherLHPointsList = new();
    protected int m_ActiveAetherLHPointsIndex;
    protected float m_TimerAetherLHPoints = 1f;

    #endregion // LIGHT AND HEAVY ATTACK SETTINGS

    #region LIGHT AETHER SETTINGS

    [Header("LIGHT ATTACK SETTINGS")]

    [Tooltip(" ???????????????????? ")]
    public bool enableLightAttack;

    [Tooltip("The cost in aether points for performing a light attack.")]
    public int lightAttackCost = 1;

    [Tooltip("Indicates shots per minute.")]
    public float lightAttackCadence = 93f;

    [Tooltip(" ???????????????????? ")]
    public float lightAttackBulletSpeed = 20f;

    [Tooltip(" ???????????????????? ")]
    public float lightAttackStartInitTime = 0.3f;

    #endregion // LIGHT AETHER SETTINGS

    #region HEAVY AETHER SETTINGS

    [Header("HEAVY ATTACK SETTINGS")]

    [Tooltip(" ???????????????????? ")]
    public bool enableHeavyAttack;

    [Tooltip("The cost in aether points for performing a heavy attack.")]
    public int heavyAttackCost = 3;

    [Tooltip(" ???????????????????? ")]
    public float heavyAttackBulletSpeed = 25f;

    [Tooltip(" ???????????????????? ")]
    public float heavyAttackTimeBeforeShot = 0.95f;

    [Tooltip(" ???????????????????? ")]
    public bool m_AttackHeavyRequested = false;

    #endregion // LIGHT AETHER SETTINGS

    #region SUPER AETHER SETTINGS

    [Header("AETHER BAR SETTINGS")]

    [Tooltip(" ???????????????????? ")]
    public bool enableSuperAttack;

    [Tooltip("The initial aether value displayed on the aether bar.")]
    public float startingAetherValue = 0.5f;

    [Tooltip("The maximum aether value that the aether bar can display.")]
    public float maxAetherValue = 1.0f;

    [Tooltip("The amount of aether added to the aether bar per second.")]
    public float aetherValueFillPerSecond = 0.0042f;

    [Tooltip("The time interval in seconds at which aether is incremented on the aether bar.")]
    public float aetherValueTimeStep = 1.0f;

    [Header("SUPER ATTACK SETTINGS")]

    [Tooltip(" ???????????????????? ")]
    public float superAttackBulletSpeed = 30f;

    [Tooltip(" ???????????????????? ")]
    public float superAttackTimeBeforeShot = 1.25f;

    protected float m_CurrentAetherBarValue;
    protected float m_TimerAetherBarValue;

    #endregion // SUPER AETHER SETTINGS

    #region EVENTS

    [Header("EVENTS")]
    [Space]

    [Tooltip("Event triggered when the aether value is updated.")]
    public AetherEvent OnSetAetherBarValue;

    [Tooltip("Event triggered when the object gains aether.")]
    public AetherFillEvent OnGainAetherBarValue;

    #endregion // EVENTS

    protected bool m_ResetAetherOnSceneReload; // Currently not used
    protected float m_AetherValueBeforeDead; //Currently not used

    void OnEnable()
    {
        Debug.LogWarning($"m_ResetAetherOnSceneReload & m_AetherValueBeforeDead has no function");

        if (parentUIAetherLightHeavy == null)
        {
            parentUIAetherLightHeavy = GameObject.Find(uIParentSearch).transform;

            if (parentUIAetherLightHeavy != null)
            {
                Debug.Log($"{parentUIAetherLightHeavy} is NULL. search is started, string is *{uIParentSearch}*");
            }
            else
            {
                Debug.LogError($"{parentUIAetherLightHeavy} is NULL. Check reference");
                Debug.Break();
            }
        }

        m_CurrentAetherBarValue = startingAetherValue;

        OnSetAetherBarValue.Invoke();
    }

    void Start()
    {
        for (int i = 0; i < activeAetherLHPoints; i++)
        {
            GameObject gO = Instantiate(uIPrefabAetherLightHeavy, parentUIAetherLightHeavy);

            m_ActiveAetherLHPointsList.Add(gO);
            m_ActiveAetherLHPointsIndex++;
        }
    }

    #region CONTROL AND COMPARISON METHODS

    /// <summary>
    /// Gets the current aether value.
    /// </summary>
    public float CurrentAether
    {
        get { return m_CurrentAetherBarValue; }
    }

    /// <summary>
    /// Checks if there are enough active aether base attack points to perform an action.
    /// </summary>
    /// <param name="requiredPoints">The number of aether base attack points required.</param>
    /// <returns>True if there are enough points, false otherwise.</returns>
    public bool CanShotCheckAether(int requiredPoints)
    {
        if (m_ActiveAetherLHPointsIndex - requiredPoints < 0f)
            return false;

        return true;
    }

    #endregion // CONTROL AND COMPARISON FUNCTIONS

    #region AETHERBAR METHODS

    /// <summary>
    /// Reduces the active aether base attack points by the specified amount.
    /// Deactivates the corresponding objects in the list.
    /// </summary>
    /// <param name="requiredPoints">The number of aether base attack points to reduce.</param>
    public void HandleAetherBasePointReduction(int requiredPoints)
    {
        if ((m_ActiveAetherLHPointsIndex - requiredPoints) < 0)
            return;

        for (int i = 0; i < requiredPoints; i++)
        {
            m_ActiveAetherLHPointsIndex--;
            m_ActiveAetherLHPointsList[m_ActiveAetherLHPointsIndex].GetComponent<Animator>().SetTrigger("Reduce");
        }

        m_TimerAetherLHPoints = delayAfterUseLHAttack;
    }

    /// <summary>
    /// Increases the aether of the object.
    /// </summary>
    /// <param name="amount">The amount of aether to gain.</param>
    public void GainAether(float amount)
    {
        m_CurrentAetherBarValue += amount;

        if (m_CurrentAetherBarValue > maxAetherValue)
            m_CurrentAetherBarValue = maxAetherValue;

        OnSetAetherBarValue.Invoke();

        OnGainAetherBarValue.Invoke(amount);
    }

    /// <summary>
    /// Sets the aether of the object to a specified amount.
    /// </summary>
    /// <param name="amount">The amount to set the aether to.</param>
    public void SetAether(float amount)

    {
        m_CurrentAetherBarValue = amount;
        OnSetAetherBarValue.Invoke();
    }

    /// <summary>
    /// Gradually fills the aether bar value over time.
    /// </summary>
    void AetherBarValueFilling()
    {
        if (m_CurrentAetherBarValue < 1f)
        {
            if (m_TimerAetherBarValue > 0f)
            {
                m_TimerAetherBarValue -= Time.deltaTime;
            }
            else if (m_TimerAetherBarValue <= 0f)
            {
                m_CurrentAetherBarValue += aetherValueFillPerSecond;

                OnSetAetherBarValue.Invoke();
                OnGainAetherBarValue.Invoke(m_CurrentAetherBarValue);

                m_TimerAetherBarValue = aetherValueTimeStep;
            }
        }
    }

    /// <summary>
    /// Activates aether base attack points at regular intervals.
    /// </summary>
    void AetherBasePointsFilling()
    {
        // Stop if all objects are active
        if (m_ActiveAetherLHPointsIndex >= activeAetherLHPoints)
            return;

        // Update the timer
        m_TimerAetherLHPoints -= Time.deltaTime;

        // Check if one second has passed
        if (m_TimerAetherLHPoints <= 0f)
        {
            // Activate the next object in the list
            m_ActiveAetherLHPointsList[m_ActiveAetherLHPointsIndex].GetComponent<Animator>().SetTrigger("Restore");
            m_ActiveAetherLHPointsIndex++;

            // Reset the timer for the next activation
            m_TimerAetherLHPoints = aetherValueTimeStep;
        }
    }

    #endregion // AETHERBAR

    void Update()
    {
        AetherBarValueFilling();
        AetherBasePointsFilling();
    }
}
