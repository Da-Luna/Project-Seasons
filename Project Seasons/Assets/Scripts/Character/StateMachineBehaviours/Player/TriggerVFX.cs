﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerVFX : StateMachineBehaviour
{
    public string vfxName;
    public Vector3 offset = Vector3.zero;
    public bool attachToParent = false;
    public float startDelay = 0;
    public bool OnEnter = true;
    public bool OnExit = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (OnEnter)
        {
            Trigger(animator.transform);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (OnExit)
        {
            Trigger(animator.transform);
        }
    }

    void Trigger(Transform transform)
    {
        var flip = false;
        var spriteRender = transform.GetComponent<SpriteRenderer>();
        if (spriteRender)
            flip = spriteRender.flipX;
        VFXController.Instance.Trigger(vfxName, offset, startDelay, flip, attachToParent ? transform : null);
    }

}
