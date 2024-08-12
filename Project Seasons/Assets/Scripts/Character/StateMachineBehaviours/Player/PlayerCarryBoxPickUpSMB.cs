using UnityEngine;

public class PlayerCarryBoxPickUpSMB : SceneLinkedSMB<PlayerCharacter>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.CheckForInteractingInput();

        m_MonoBehaviour.SetMoveVector(Vector2.zero);
        
        m_MonoBehaviour.GroundedHorizontalWalkMovement(true, 0f);
        m_MonoBehaviour.GroundedVerticalMovement();
    }
}
