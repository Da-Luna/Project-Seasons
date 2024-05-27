using UnityEngine;

public class PlayerSuperAttackSMB : SceneLinkedSMB<PlayerCharacter>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.StopFocused();
        m_MonoBehaviour.SetAttackLightPara(false);
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.SuperAttack();

        m_MonoBehaviour.GroundedHorizontalRunMovement(false);
        m_MonoBehaviour.SetVerticalMovement(0);

        m_MonoBehaviour.CheckForGrounded();
    }
}
