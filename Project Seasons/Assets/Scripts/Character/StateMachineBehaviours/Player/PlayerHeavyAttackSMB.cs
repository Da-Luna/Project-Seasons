using UnityEngine;

public class PlayerHeavyAttackSMB : SceneLinkedSMB<PlayerCharacter>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.StopFocused();
        m_MonoBehaviour.SetAttackLightPara(false);
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.HeavyAttack();

        m_MonoBehaviour.GroundedHorizontalRunMovement(false);
        if (!m_MonoBehaviour.CheckForGrounded())
            m_MonoBehaviour.AirborneVerticalMovement();
    }
}
