using UnityEngine;
using UnityEngine.UI;

public class AetherUIReceiver : MonoBehaviour
{
    Image aetherBar;
    AetherController aetherController;

    /// <summary>
    /// Initializes the aether bar and aether controller references.
    /// Subscribes to aether-related events.
    /// </summary>
    void Start()
    {
        if (aetherBar == null)
            aetherBar = GetComponent<Image>();

        if (aetherController == null)
        {
            aetherController = PlayerCharacter.PlayerInstance.PlayerAether;
            aetherController.OnGainAetherBarValue.AddListener(OnAetherBarValueGained);
            aetherController.OnSetAetherBarValue.AddListener(OnSetBarValueAether);
        }

        UpdateAetherBarValue(aetherController.CurrentAether);
    }

    /// <summary>
    /// Unsubscribes from aether-related events when the object is disabled.
    /// </summary>
    void OnDisable()
    {
        if (aetherController != null)
        {
            aetherController.OnGainAetherBarValue.RemoveListener(OnAetherBarValueGained);
            aetherController.OnSetAetherBarValue.RemoveListener(OnSetBarValueAether);
        }
    }

    /// <summary>
    /// Updates the aether bar UI when aether is gained.
    /// </summary>
    /// <param name="amount">The amount of aether gained.</param>
    void OnAetherBarValueGained(float amount)
    {
        UpdateAetherBarValue(aetherController.CurrentAether);
    }

    /// <summary>
    /// Updates the aether bar UI when the aether value is set.
    /// </summary>
    void OnSetBarValueAether()
    {
        UpdateAetherBarValue(aetherController.CurrentAether);
    }

    /// <summary>
    /// Updates the fill amount of the aether bar UI element.
    /// </summary>
    /// <param name="currentAether">The current aether value.</param>
    void UpdateAetherBarValue(float currentAether)
    {
        if (aetherBar != null)
            aetherBar.fillAmount = currentAether;
    }
}
