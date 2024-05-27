using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashSMB : SceneLinkedSMB<PlayerCharacter>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.UpdateViewDirection();
        m_MonoBehaviour.Dashing();
        
        m_MonoBehaviour.InputQuickslots();
    }
}
