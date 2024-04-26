using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackingRunSMB : SceneLinkedSMB<EnemyBehaviour>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.ChargingTarget();

        m_MonoBehaviour.CheckForTarget();
        m_MonoBehaviour.CheckTargetStillVisible();

        m_MonoBehaviour.CheckAttackRange();
    }
}
