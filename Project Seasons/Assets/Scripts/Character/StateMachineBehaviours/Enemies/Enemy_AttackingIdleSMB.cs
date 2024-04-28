using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackingIdleSMB : SceneLinkedSMB<EnemyBehaviour>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.ChargingToTarget();
     
        m_MonoBehaviour.UpdateMeleeAttackRangeCheck();
        m_MonoBehaviour.UpdateLookToTarget();
    }
}
