using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemy : EnemyBaseBehaviour
{
    [Header("Jumping")]
    [SerializeField, Tooltip("")]
    LayerMask jumpingGroundLayer;

    protected bool m_canJump;

    public override void Awake()
    {
        m_canJump = true;
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
         
            if(m_Target.position.y >= 2.5f)
            {
                //Überprüfen ob über dem Enemy eine platform ist 
                //if(...)
                if(m_canJump)
                    SetVerticalMovement(26f); m_canJump = false;
             
            }

            m_Animator.SetTrigger(m_HashSpottedPara);
        }
    }

    private void Update()
    {
        //Debug.DrawLine(transform.position, transform.position + transform.up * 4, Color.black);

        if (Physics.Raycast(transform.position, transform.up, 100, jumpingGroundLayer))
        {
            print("ja");
        }
    }

    public void SetVerticalMovement(float newVerticalMovement)
    {
        m_MoveVector.y = newVerticalMovement;
    }

    public override void PatrollingWalk(bool useabyss)
    {
        base.PatrollingWalk();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up * 4);
    }

#endif
}
