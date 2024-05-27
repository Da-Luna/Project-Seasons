using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : EnemyBaseBehaviour
{
    public override void Awake()
    {    
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void ChargingToTarget()
    {
        if (m_Target == null)
            return;

        // Move towards the target or stop if close enough
        float tolerance = 0.1f;
        if (Mathf.Abs(transform.position.x - m_Target.position.x) <= tolerance)
        {
            m_MoveVector = Vector2.zero;
            m_Animator.SetTrigger(m_HashAttackIdlePara);
        }
        else
        {
            m_MoveVector.x = (m_Target.position.x - transform.position.x > 0) ? runSpeed : -runSpeed;
          
            m_Animator.SetTrigger(m_HashSpottedPara);
        }
    }

    public override void PatrollingWalk(bool useabyss)
    {
        base.PatrollingWalk();
    }
}
