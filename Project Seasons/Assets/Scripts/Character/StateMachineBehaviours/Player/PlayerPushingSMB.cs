using UnityEngine;

public class PlayerPushingSMB : SceneLinkedSMB<PlayerCharacter>
{
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.StartPushing();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.GroundedHorizontalWalkMovement(true, m_MonoBehaviour.PushingSpeedProportion);
        m_MonoBehaviour.GroundedVerticalMovement();

        m_MonoBehaviour.CheckForPushing();
        m_MonoBehaviour.MovePushable();

        m_MonoBehaviour.UpdateGroundedViewDirection();
        m_MonoBehaviour.CheckForGrounded();

        m_MonoBehaviour.InputQuickslots();
    }
}
