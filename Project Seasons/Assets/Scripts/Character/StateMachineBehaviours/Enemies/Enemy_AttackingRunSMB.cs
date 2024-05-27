using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackingRunSMB : SceneLinkedSMB<EnemyBaseBehaviour>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.ChargingToTarget();

        m_MonoBehaviour.UpdateCheckForTarget();
        m_MonoBehaviour.UpdateLookToTarget();
        m_MonoBehaviour.UpdateTargetStillVisibleCheck();

        m_MonoBehaviour.UpdateMeleeAttackRangeCheck();
    }
}
