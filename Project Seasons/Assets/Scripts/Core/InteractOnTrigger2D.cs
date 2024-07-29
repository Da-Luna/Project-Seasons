using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class InteractOnTrigger2D : MonoBehaviour
{
    [Header("BASE SETTINGS")]
    [Tooltip("Layers that the trigger can interact with.")]
    public LayerMask triggerLayerMask;
    [Tooltip("")]
    public InventoryController.InventoryChecker[] inventoryChecks;


    [Header("DISABLE SETTINGS")]
    [Tooltip("Whether the script should be disabled after triggering.")]
    public bool disableAfterTrigger;
    [Tooltip("Time in seconds before the script is disabled.")]
    public float timeBeforeDisable;
    [Tooltip("")]
    public GameObject objToDisable;

    protected WaitForSeconds m_DisableTime;
    protected Collider2D m_Collider2D;

    [Header("EVENTS")]
    public UnityEvent OnEnter;
    public UnityEvent OnExit;

    private void OnEnable()
    {
        m_DisableTime = new WaitForSeconds(timeBeforeDisable);
        m_Collider2D = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (!enabled)
            return;

        // Check the LayerMask of GameObject thats hit the collider
        if (triggerLayerMask.Contains(target.gameObject))
        {
            ExecuteOnEnter(target);
        }
    }

    void OnTriggerExit2D(Collider2D target)
    {
        if (!enabled)
            return;

        // Check the LayerMask of GameObject thats hit the collider
        if (triggerLayerMask.Contains(target.gameObject))
        {
            ExecuteOnExit(target);
        }
    }

    /// <summary>
    /// Executes actions when the trigger collider is entered.
    /// </summary>
    /// <param name="target">The collider that entered the trigger.</param>
    protected virtual void ExecuteOnEnter(Collider2D target)
    {
        OnEnter.Invoke();

        for (int i = 0; i < inventoryChecks.Length; i++)
        {
            inventoryChecks[i].CheckInventory(target.GetComponentInChildren<InventoryController>());
        }

        if (disableAfterTrigger)
        {
            StartCoroutine(DisableAfterTime());
        }
    }

    /// <summary>
    /// Executes actions when the trigger collider is exited.
    /// </summary>
    /// <param name="target">The collider that exited the trigger.</param>
    protected virtual void ExecuteOnExit(Collider2D target)
    {
        OnExit.Invoke();

        if (disableAfterTrigger)
        {
            StartCoroutine(DisableAfterTime());
        }
    }

    /// <summary>
    /// Disables the script after a specified amount of time.
    /// </summary>
    /// <returns>IEnumerator for coroutine handling.</returns>
    IEnumerator DisableAfterTime()
    {
        yield return m_DisableTime;

        m_Collider2D.enabled = false;

        enabled = false;
        objToDisable.SetActive(false);
    }
}
