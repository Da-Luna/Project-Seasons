using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Collider2D))]
public class InteractForceField2D : MonoBehaviour
{
    [Header("INTERACTION SETTINGS")]

    [SerializeField, Tooltip("")]
    int interactionTimeToReach = 500;

    [Header("LIGHT SETTINGS")]

    [SerializeField, Tooltip("")]
    float lightValueOnInteract = 0.8f;

    [SerializeField, Tooltip("")]
    float lightValueOnFinal = 1.2f;

    [SerializeField, Tooltip("")]
    float lightChangeSpeed = 1.0f;

    [Header("EVENTS")]
    [Space]

    [SerializeField, Tooltip("")]
    UnityEvent OnFinal;

    Collider2D m_Collider2D;
    Light2D m_AetherLight;
    GameObject m_ForceField;
    Transform m_PlayerTransform;
    ParticleSystem m_PlayerForceSystem;
    Animator m_PlayerAnimator;
    int m_ParticleCounter = 0;
    float m_CurrentLightValue = 0f;

    bool m_StartInteracting = false;
    bool m_FinalInteracting = false;

    const string m_PlayerLayerMask = "Player";
    const string m_PlayerForceField = "ParticleSystems/ParticleForceField";
    readonly int m_HashFocusedPara = Animator.StringToHash("Focused");

    void Start()
    {
        m_Collider2D = GetComponent<Collider2D>();

        // Get the Light2D script and initialize it 
        var _lightGO = transform.GetChild(0).gameObject;
        m_AetherLight = _lightGO.GetComponent<Light2D>();
        m_AetherLight.intensity = 0f;
        m_AetherLight.gameObject.SetActive(false);

        // Get the ParticleForceField Component script and initialize it 
        m_ForceField = transform.GetChild(1).gameObject;
        m_ForceField.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag(m_PlayerLayerMask))
        {
            GameObject _player = target.gameObject;

            m_PlayerTransform = _player.transform;
            m_PlayerAnimator = _player.GetComponent<Animator>();

            GameObject _playerForceSystem = m_PlayerTransform.Find(m_PlayerForceField).gameObject;
            m_PlayerForceSystem = _playerForceSystem.GetComponent<ParticleSystem>();
        }
    }

    void OnTriggerStay2D(Collider2D target)
    {
        if (m_FinalInteracting)
        {
            return;
        }

        if (GetFocusedState())
        {
            StartInteracting();
            UpdateInteracting();

            if (m_ParticleCounter >= interactionTimeToReach)
            {
                FinalInteracting();
                OnFinal.Invoke();
            }
        }
        else if (m_StartInteracting && !m_FinalInteracting)
        {
            StopInteracting();
        }
    }

    void OnTriggerExit2D(Collider2D target)
    {
        if (target.CompareTag(m_PlayerLayerMask))
        {
            if (m_StartInteracting && !m_FinalInteracting)
            {
                StopInteracting();
            }
        }
    }

    void StartInteracting()
{
    if (m_StartInteracting) 
    {
        return;
    }
    
    m_PlayerForceSystem.gameObject.SetActive(true);

    var pSMain = m_PlayerForceSystem.main;
    pSMain.loop = true;
    m_PlayerForceSystem.Play();

    m_ForceField.SetActive(true);
    m_AetherLight.gameObject.SetActive(true);

    m_StartInteracting = true;
}

    void StopInteracting()
    {
        StartCoroutine(StopInteractCoroutine());

        m_StartInteracting = false;
    }

    void UpdateInteracting()
    {
        // Upadate Light
        if (m_AetherLight.intensity < lightValueOnInteract)
        {
            m_CurrentLightValue = Mathf.MoveTowards(m_CurrentLightValue, lightValueOnInteract, lightChangeSpeed * Time.deltaTime);
            m_AetherLight.intensity = m_CurrentLightValue;
        }

        // Particle Counter
        if (m_PlayerForceSystem.particleCount != 0)
        {
            m_ParticleCounter = m_PlayerForceSystem.particleCount;
        }
    }

    void FinalInteracting()
    {
        if (m_FinalInteracting)
        {
            return;
        }

        m_AetherLight.intensity = lightValueOnFinal;
        StartCoroutine(FinalInteractCoroutine());

        m_FinalInteracting = true;
    }

    bool GetFocusedState()
    {
        bool isFocused = false;

        if (m_PlayerAnimator != null)
        {
            isFocused = m_PlayerAnimator.GetBool(m_HashFocusedPara);
        }

        return isFocused;
    }

    IEnumerator StopInteractCoroutine()
    {
        var pSMain = m_PlayerForceSystem.main;
        pSMain.loop = false;

        while (m_AetherLight.intensity > 0f)
        {
            float _lightChangeSpeed = lightChangeSpeed / 2f;
            m_CurrentLightValue = Mathf.MoveTowards(m_CurrentLightValue, 0f, _lightChangeSpeed * Time.deltaTime);
            m_AetherLight.intensity = m_CurrentLightValue;

            yield return null;
        }
        while (m_PlayerForceSystem.particleCount > 0)
        {
            yield return null;
        }

        m_PlayerForceSystem.gameObject.SetActive(false);
        m_ForceField.SetActive(false);
        m_AetherLight.gameObject.SetActive(false);
    }

    IEnumerator FinalInteractCoroutine()
    {
        var pSMain = m_PlayerForceSystem.main;
        pSMain.loop = false;

        while (m_PlayerForceSystem.particleCount > 0)
        {
            yield return null;
        }

        m_PlayerForceSystem.gameObject.SetActive(false);
        m_ForceField.SetActive(false);

        m_Collider2D.enabled = false;
        enabled = false;
    }
}
