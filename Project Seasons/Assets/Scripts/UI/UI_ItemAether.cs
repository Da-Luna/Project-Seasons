using UnityEngine;
using TMPro;

public class UI_ItemAether : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI aetherItemUGUI;

    void Start()
    {
        if (aetherItemUGUI == null)
            aetherItemUGUI = GetComponent<TextMeshProUGUI>();

        PlayerCharacter.PlayerInstance.OnAetherItemCounterChanged.AddListener(UpdateAetherItemCount);
        UpdateAetherItemCount(PlayerCharacter.PlayerInstance.AetherItemCounter);
    }

    void OnDestroy()
    {
        if (PlayerCharacter.PlayerInstance != null)
        {
            PlayerCharacter.PlayerInstance.OnAetherItemCounterChanged.RemoveListener(UpdateAetherItemCount);
        }
    }

    void UpdateAetherItemCount(int count)
    {
        if (aetherItemUGUI != null)
        {
            aetherItemUGUI.text = count.ToString();
        }
    }
}
