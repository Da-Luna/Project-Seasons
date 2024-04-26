using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackingBufferSMB : SceneLinkedSMB<EnemyBehaviour>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.CheckAttackRange();
    }
}
