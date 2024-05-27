using System.Collections;
using UnityEngine;

public class PlayerAirborneSMB : SceneLinkedSMB<PlayerCharacter>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_MonoBehaviour.CheckForAttackLightInput())
            m_MonoBehaviour.LightAttack();

        if (m_MonoBehaviour.InputCheckForFocused())
            m_MonoBehaviour.Focused();

        if (m_MonoBehaviour.InputCheckForAttackSuper())
            m_MonoBehaviour.SuperAttack();

        m_MonoBehaviour.InputQuickslots();

        m_MonoBehaviour.AirborneHorizontalMovement();
        m_MonoBehaviour.AirborneVerticalMovement();

        m_MonoBehaviour.UpdateViewDirection();
        m_MonoBehaviour.UpdateJump();
        m_MonoBehaviour.CheckForGrounded();

        if (m_MonoBehaviour.InputCheckForDash())
            m_MonoBehaviour.StartDashing();
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!m_MonoBehaviour.CheckForAttackLightInput())
            m_MonoBehaviour.StopLightAttack();

        if (!m_MonoBehaviour.InputCheckForFocused())
            m_MonoBehaviour.StopFocused();
    }
}
