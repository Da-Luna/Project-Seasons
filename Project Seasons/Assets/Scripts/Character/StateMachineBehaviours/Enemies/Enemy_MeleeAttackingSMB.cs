using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackingSMB : SceneLinkedSMB<EnemyBaseBehaviour>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.UpdateMeleeAttackRangeCheck();
        m_MonoBehaviour.UpdateLookToTarget();
        m_MonoBehaviour.ChargingToTarget();
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
}
