using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_PatrollingWaitSMB : SceneLinkedSMB<EnemyBehaviour>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.PatrollingWait();
        m_MonoBehaviour.CheckForTarget();
    }
}
