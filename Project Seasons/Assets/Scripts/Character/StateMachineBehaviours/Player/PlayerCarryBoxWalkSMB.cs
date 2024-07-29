using UnityEngine;

public class PlayerCarryBoxWalkSMB : SceneLinkedSMB<PlayerCharacter>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.InputQuickslots();

        m_MonoBehaviour.GroundedHorizontalWalkMovement(true);
        m_MonoBehaviour.GroundedVerticalMovement();

        m_MonoBehaviour.UpdateGroundedViewDirection();
        m_MonoBehaviour.CheckForGrounded();
    }
}
