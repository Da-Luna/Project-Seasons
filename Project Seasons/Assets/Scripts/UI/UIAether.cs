using UnityEngine;
using UnityEngine.UI;

public class UIAether : MonoBehaviour
{
    Image aetherBar;
    AetherController aetherController;

    void Start()
    {
        if (aetherBar == null)
            aetherBar = GetComponent<Image>();

        if (aetherController == null)
            aetherController = PlayerCharacter.PlayerInstance.PlayerAether;

        if (aetherController != null)
        {
            aetherController.OnGainAether.AddListener(OnAetherGained);
        }

        UpdateAetherBar(aetherController.CurrentAether);
    }

    void OnDisable()
    {
        if (aetherController != null)
        {
            aetherController.OnGainAether.RemoveListener(OnAetherGained);
        }
    }

    void OnAetherReduce(AetherController aetherController)
    {
        UpdateAetherBar(aetherController.CurrentAether);
    }

    void OnAetherGained(float amount)
    {
        UpdateAetherBar(aetherController.CurrentAether);
    }

    void UpdateAetherBar(float currentAether)
    {
        if (aetherBar != null)
            aetherBar.fillAmount = currentAether;
    }
}
