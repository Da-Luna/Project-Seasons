using Cinemachine.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying : EnemyBaseBehaviour
{
 
    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void PatrollingWalk(bool useabyss)
    {
        base.PatrollingWalk(false);
    }

    public override void ChargingToTarget()
    {
        if (m_Target == null)
            return;

        // Move towards the target or stop if close enough
        float tolerance = 0.1f;
        if (Mathf.Abs(transform.position.x - m_Target.position.x) <= tolerance)
        {     
            if (Mathf.Abs(transform.position.y - m_Target.position.y) > tolerance)
            {             
                var y = (m_Target.position.y - transform.position.y > 0) ? runSpeed : -runSpeed;

                m_MoveVector = new Vector2(0, y);
                return;
            }

            m_MoveVector = Vector2.zero;
            m_Animator.SetTrigger(m_HashAttackIdlePara);
        }
        else
        {
            var x = (m_Target.position.x - transform.position.x > 0) ? runSpeed : -runSpeed;
            var y = (m_Target.position.y - transform.position.y > 0) ? runSpeed : -runSpeed;

            if (Mathf.Abs(transform.position.y - m_Target.position.y) <= tolerance)
                y = 0f;
            else
                y = (m_Target.position.y - transform.position.y > 0.1f) ? runSpeed : -runSpeed;

            m_MoveVector = new Vector2(x, y);

            m_Animator.SetTrigger(m_HashSpottedPara);
        }
    }

}
