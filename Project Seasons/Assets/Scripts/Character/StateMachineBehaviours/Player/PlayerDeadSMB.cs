using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadSMB : SceneLinkedSMB<PlayerCharacter>
{
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.SetMoveVector(m_MonoBehaviour.GetHurtDirection() * m_MonoBehaviour.HurtJumpSpeed);
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.CheckForGrounded();
        m_MonoBehaviour.AirborneVerticalMovement();
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.SetMoveVector(Vector2.zero);
    }
}
