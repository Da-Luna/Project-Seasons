using UnityEngine;
using UnityEngine.UI;

public class UIDash : MonoBehaviour
{
    [SerializeField]
    Image dashBar;
    
    [SerializeField]
    float startValue = 1f;

    void Start()
    {
        if (dashBar == null)
            dashBar = GetComponent<Image>();

        if (dashBar != null)
            dashBar.fillAmount = startValue;

        if (PlayerCharacter.PlayerInstance != null)
        {
            PlayerCharacter.PlayerInstance.OnDashCooldownChanged.AddListener(UpdateDashBar);
        }
    }

    void OnDisable()
    {
        if (PlayerCharacter.PlayerInstance != null)
        {
            PlayerCharacter.PlayerInstance.OnDashCooldownChanged.RemoveListener(UpdateDashBar);
        }
    }

    void UpdateDashBar(float dashCooldownTime)
    {
        float dashCooldown = PlayerCharacter.PlayerInstance.DashCooldownTime;
        float dashCooldownProgress = 1 - Mathf.Clamp01(dashCooldownTime / dashCooldown);
        dashBar.fillAmount = dashCooldownProgress;
    }
}