using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_PatrollingWalkSMB : SceneLinkedSMB<EnemyBaseBehaviour>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.PatrollingStart();
        m_MonoBehaviour.UpdateLookToPatrolPosition();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.PatrollingWalk();

        m_MonoBehaviour.UpdateCheckForTarget();
    }
}
