using UnityEngine;

public class PlayerLightAttackSMB : SceneLinkedSMB<PlayerCharacter>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_MonoBehaviour.CheckForAttackLightInput())
            m_MonoBehaviour.LightAttack();

        if (m_MonoBehaviour.InputCheckForFocused())
            m_MonoBehaviour.Focused();

        if (m_MonoBehaviour.InputCheckForAttackHeavy())
            m_MonoBehaviour.HeavyAttack();

        if (m_MonoBehaviour.InputCheckForAttackSuper())
            m_MonoBehaviour.SuperAttack();

        m_MonoBehaviour.InputQuickslots();

        m_MonoBehaviour.GroundedHorizontalRunMovement(true);
        m_MonoBehaviour.GroundedVerticalMovement();

        m_MonoBehaviour.UpdateViewDirection();
        m_MonoBehaviour.UpdateJump();
        m_MonoBehaviour.CheckForGrounded();

        if (m_MonoBehaviour.InputCheckForJump())
            m_MonoBehaviour.SetVerticalMovement(m_MonoBehaviour.JumpSpeed);

        if (m_MonoBehaviour.InputCheckForDash())
            m_MonoBehaviour.StartDashing();

        m_MonoBehaviour.CheckForPushing();
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.StopLightAttack();
    }
}
