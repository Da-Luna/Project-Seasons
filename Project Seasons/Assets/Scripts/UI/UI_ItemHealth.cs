using UnityEngine;
using TMPro;

public class UIHealthItem : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI healthItemUGUI;

    void Start()
    {
        if (healthItemUGUI == null)
            healthItemUGUI = GetComponent<TextMeshProUGUI>();

        PlayerCharacter.PlayerInstance.OnHealthItemCounterChanged.AddListener(UpdateHealthItemCount);
        UpdateHealthItemCount(PlayerCharacter.PlayerInstance.HealthItemCounter);
    }

    void OnDestroy()
    {
        if (PlayerCharacter.PlayerInstance != null)
        {
            PlayerCharacter.PlayerInstance.OnHealthItemCounterChanged.RemoveListener(UpdateHealthItemCount);
        }
    }

    void UpdateHealthItemCount(int count)
    {
        if (healthItemUGUI != null)
        {
            healthItemUGUI.text = count.ToString();
        }
    }
}
