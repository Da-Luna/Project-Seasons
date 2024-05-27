using UnityEngine;

public class PlayerBaseMovementSMB : SceneLinkedSMB<PlayerCharacter>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.CheckForAttackLightInput();

        if (m_MonoBehaviour.InputCheckForFocused())
            m_MonoBehaviour.Focused();

        if (m_MonoBehaviour.InputCheckForAttackHeavy())
            m_MonoBehaviour.HeavyAttack();

        if (m_MonoBehaviour.InputCheckForAttackSuper())
            m_MonoBehaviour.SuperAttack();

        m_MonoBehaviour.InputQuickslots();

        m_MonoBehaviour.GroundedHorizontalRunMovement(true);
        m_MonoBehaviour.GroundedVerticalMovement();

        m_MonoBehaviour.UpdateGroundedViewDirection();
        m_MonoBehaviour.CheckForGrounded();
        
        if (m_MonoBehaviour.InputCheckForJump())
            m_MonoBehaviour.SetVerticalMovement(m_MonoBehaviour.JumpSpeed);

        if (m_MonoBehaviour.InputCheckForDash())
            m_MonoBehaviour.StartDashing();

        m_MonoBehaviour.CheckForPushing();
    }
}
