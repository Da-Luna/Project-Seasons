using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Collider2D))]
public class InteractOnTriggerStay2D : MonoBehaviour
{    
    [SerializeField]
    LayerMask aetherLayers;
    
    [SerializeField]
    float aetherValueToReach;
    
    [SerializeField]
    float aetherValueStepPoints;

    [SerializeField]
    float disableTime = 2f;

    public Light2D aetherLight;
    public float aetherLightValue;
    public float aetherLightSpeed;
    public float aetherFinalLightValue;
    public float m_LightValue;

    public UnityEvent OnStay;

    float m_AetherFillTime = 0f;

    bool m_IsDone;

    void OnEnable()
    {
        if (aetherLight == null)
        {
            aetherLight = GetComponentInChildren<Light2D>();
        }

        m_LightValue = 0f;
        aetherLight.intensity = m_LightValue;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!enabled)
            return;

        if (aetherLayers.Contains(other.gameObject))
        {
            ExecuteOnStay(other);
        }
    }

    protected virtual void ExecuteOnStay(Collider2D other)
    {
        if (m_IsDone)
            return;

        m_AetherFillTime += aetherValueStepPoints * Time.deltaTime;

        m_LightValue = Mathf.MoveTowards(m_LightValue, aetherLightValue, aetherLightSpeed * Time.deltaTime);
        aetherLight.intensity = m_LightValue;

        if (m_AetherFillTime >= aetherValueToReach)
        {
            OnStay.Invoke();
            StartCoroutine(AutoDisable());

            aetherLight.intensity = aetherFinalLightValue;
            m_IsDone = true;
        }
    }

    IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(disableTime);
        enabled = false;
    }
}
