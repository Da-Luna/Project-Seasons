using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAppearSMB : SceneLinkedSMB<PlayerCharacter>
{
    [SerializeField]
    Vector2 spawnPosition;

    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.transform.position = spawnPosition;
    }
}
