using Cinemachine.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAndWalkingEnemy : EnemyBaseBehaviour
{
    //Visual Target Object
    [Header("NoseDive")]
    [SerializeField, Tooltip("The Target Point for the Nosedive")]
    Transform targetPointObject;
    [SerializeField, Tooltip("The Y-Axis Offset")]
    float targetPointYOffset = -1.7f;
    [SerializeField, Tooltip("Min. Wait Time for the Nosedive")]
    float minWaitTimeForNosedive = 2;
    [SerializeField, Tooltip("Max. Wait Time for the Nosedive")]
    float maxWaitTimeForNosedive = 4;
    [SerializeField, Tooltip("The Speed used when a Nosedive is activated")]
    float noseDiveSpeed = 4;

    protected float m_TimeForNosedive;

    protected bool m_startedNosedive = false;
    protected bool m_IsNosediving = false;
    protected bool m_IsWaitingForNosedive = false;
    protected bool m_isGroundedMode = false;

    protected Vector2 m_LastPositionOfTargetObject;

    const float k_OffScreenError = 0.01f;

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
        if(!m_isGroundedMode)
            base.PatrollingWalk(m_isGroundedMode);
        else
        {            
            if (!isOnScreen())
            {

            }

        }       
    }

    public override void ChargingToTarget()
    {
        if (m_Target == null)
            return;

        m_isGroundedMode = m_Animator.GetBool("IsGroundedMode");
        if (!m_isGroundedMode)
        {
            if (!targetPointObject.gameObject.activeSelf)
            {
                targetPointObject.gameObject.SetActive(true);

                m_TimeForNosedive = Random.Range(minWaitTimeForNosedive, maxWaitTimeForNosedive);
            }

            if (!m_startedNosedive)
            {
                targetPointObject.position = new Vector2(m_Target.position.x, targetPointYOffset);
                // m_MoveVector = Vector2.zero;

                NosediveWait();

                m_MoveVector.x = (m_Target.position.x - transform.position.x > 0) ? (runSpeed - 1) : (-runSpeed + 1);
            }
            else
            {
                if (!m_IsNosediving)
                {
                    if (m_IsWaitingForNosedive)
                    {
                        NosediveAttackWait();
                        return;
                    }
                    m_LastPositionOfTargetObject = targetPointObject.position;
                    m_TimeForNosedive = Random.Range(1, 2);
                    m_IsWaitingForNosedive = true;
                    return;
                }


                if (Vector2.Distance(m_LastPositionOfTargetObject, transform.position) < 1 && m_IsNosediving)
                {
                    //StartWalking();
                    return;
                }

                transform.position = Vector2.Lerp(transform.position, m_LastPositionOfTargetObject, noseDiveSpeed);

                targetPointObject.position = m_LastPositionOfTargetObject;

                m_Animator.SetTrigger(m_HashSpottedPara);
            }
        }
        else
        {
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

            if (!isOnScreen())
            {

            }
        }

    }

    private void ChangeMoveBehaviour()
    {
        m_MoveVector = Vector2.zero;
        targetPointObject.gameObject.SetActive(false);
        m_Animator.SetBool("IsGroundedMode", true);
        m_IsNosediving = false;
        base.useGravity = true;
        m_CharacterController2D.Rigidbody2D.isKinematic = false;

        VFXController.Instance.Trigger("NoseDiveVFX", transform.position, 0, false, null);
    }

    private bool isOnScreen()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > -k_OffScreenError &&
                        screenPoint.x < 1 + k_OffScreenError && screenPoint.y > -k_OffScreenError &&
                        screenPoint.y < 1 + k_OffScreenError;

        return onScreen;
    }

    public override void SetViewDirection(int facing)
    {
        if (m_IsWaitingForNosedive || m_IsNosediving) return;
        base.SetViewDirection(facing);
    }

    public override void UpdateTimers()
    {
        base.UpdateTimers();

        if (m_TimeForNosedive > 0.0f)
            m_TimeForNosedive -= Time.deltaTime;
    }

    private void NosediveWait()
    {
        if (m_TimeForNosedive > 0.0f) return;

        StartNoseDive();
    }

    private void NosediveAttackWait()
    {
        if (m_TimeForNosedive > 0.0f) return;

        StartNoseDiveWait();
    }

    private void StartNoseDive()
    {
        m_startedNosedive = true;
    }

    private void StartNoseDiveWait()
    {
        m_IsNosediving = true;
        m_IsWaitingForNosedive = false;
    }

}
