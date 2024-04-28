using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_MagicianAttackingSMB : SceneLinkedSMB<EnemyBehaviour>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.ChargingToTarget();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.UpdateMagicianAttackRangeCheck();

        m_MonoBehaviour.UpdateLookToTarget();
        m_MonoBehaviour.UpdateTargetStillVisibleCheck();
    }
}
