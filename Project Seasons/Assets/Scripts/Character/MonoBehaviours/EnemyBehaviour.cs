using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Movement")]
    public float patrolSpeed = 0.8f;
    public float runSpeed = 0.8f;
    public float gravity = 10.0f;

    [Header("Patrolling")]
    [SerializeField, Tooltip("")]
    bool canFlyEnemy;
    [SerializeField, Tooltip("")]
    LayerMask groundObstacleMask;
    [SerializeField, Tooltip("")]
    LayerMask wallObstacleMask;
    [SerializeField, Tooltip("")]
    float patrolDistanceToTravel = 5f;
    [SerializeField, Tooltip("Specifies the minimum time that the enemy waits by game start and after reaching the m_TargetPatrolPosition in the PatrollingWalk")]
    float minPatrolWaitTime = 1.5f;
    [SerializeField, Tooltip("Specifies the maximum time that the enemy waits by game start and after reaching the m_TargetPatrolPosition in the PatrollingWalk")]
    float maxPatrolWaitTime = 3.0f;
    [SerializeField, Tooltip("")]
    Vector2 abyssCheckSphereOffset = new(1.5f, 0f);
    readonly float m_AbyssCheckSphereRadius = 0.5f;

    [Header("Targeting")]
    [SerializeField, Range(0.0f, 10.0f), Tooltip("")]
    float viewPointHeight = 1.0f;
    [SerializeField, Range(0.0f, 360.0f), Tooltip("")]
    float viewDirection = 90.0f;
    [SerializeField, Range(0.0f, 360.0f), Tooltip("")]
    float viewFov = 180.0f;
    [SerializeField, Tooltip("")]
    float viewDistance = 20.0f;
    [Tooltip("Time in seconds without the target in the view cone before the target is considered lost from sight")]
    public float timeBeforeLostTarget = 3.0f;

    [Header("Attacking")]
    [SerializeField]
    LayerMask attackMask; // Layer must be markted as "Player"
    [SerializeField]
    Vector2 attackOffset = new(2.5f, 1.2f);
    [SerializeField]
    float attackRange = 1.2f;

    protected SpriteRenderer m_SpriteRenderer;
    protected CharacterController2D m_CharacterController2D;
    protected Animator m_Animator;

    protected Vector2 m_MoveVector;
    protected Transform m_Target;
    protected Vector2 m_TargetPatrolPosition;
    protected float m_PatrolWaitTime;
    protected float m_TimeBeforeLostTarget;

    protected bool m_Dead = false;

    protected readonly int m_HashIdlePara = Animator.StringToHash("Idle");
    protected readonly int m_HashPatrolPara = Animator.StringToHash("Patrol");
    protected readonly int m_HashSpottedPara = Animator.StringToHash("Spotted");
    protected readonly int m_HashAttackPara = Animator.StringToHash("Attack");
    protected readonly int m_HashGroundedPara = Animator.StringToHash("Grounded");

    void Awake()
    {
        m_CharacterController2D = GetComponent<CharacterController2D>();
        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        SceneLinkedSMB<EnemyBehaviour>.Initialise(m_Animator, this);
        m_PatrolWaitTime = Random.Range(minPatrolWaitTime, maxPatrolWaitTime);
    }

    void FixedUpdate()
    {
        if (m_Dead)
            return;

        m_MoveVector.y = Mathf.Max(m_MoveVector.y - gravity * Time.deltaTime, -gravity);

        m_CharacterController2D.Move(m_MoveVector * Time.deltaTime);

        UpdateTimers();

        m_Animator.SetBool(m_HashGroundedPara, m_CharacterController2D.IsGrounded);
    }

    #region PATROLLING

    public void PatrollingWait()
    {
        if (m_PatrolWaitTime > 0f) return;

        m_Animator.SetTrigger(m_HashPatrolPara);
    }

    public void PatrollingStart()
    {
        SetPatrolPosition(ref m_TargetPatrolPosition);
        SetViewDirection((int)Mathf.Sign((m_TargetPatrolPosition.x - transform.position.x) * patrolSpeed));

        m_MoveVector.x = (m_TargetPatrolPosition.x - transform.position.x > 0) ? patrolSpeed: -patrolSpeed;
    }

    public void PatrollingWalk()
    {
        bool reachedTarget = Mathf.Abs(transform.position.x - m_TargetPatrolPosition.x) < 0.1f;

        float offsetX = m_SpriteRenderer.flipX ? -abyssCheckSphereOffset.x : abyssCheckSphereOffset.x;
        Vector2 sphereOffset = new(offsetX, abyssCheckSphereOffset.y);

        Vector2 spherePosition = transform.position + transform.TransformDirection(sphereOffset);
        bool wallDetected = Physics2D.OverlapCircleAll(spherePosition, m_AbyssCheckSphereRadius, wallObstacleMask).Length > 0;

        bool abyssDetected = false;
        if (!canFlyEnemy)
        {
            abyssDetected = Physics2D.OverlapCircleAll(spherePosition, m_AbyssCheckSphereRadius, groundObstacleMask).Length == 0;
        }

        if (reachedTarget || abyssDetected || wallDetected)
        {
            m_MoveVector.x = 0f;
            m_PatrolWaitTime = Random.Range(minPatrolWaitTime, maxPatrolWaitTime);

            m_Animator.SetTrigger(m_HashIdlePara);

            PatrollingWait();
        }
    }

    public Vector2 SetPatrolPosition(ref Vector2 targetPosition)
    {
        float determinedRandomDistance = patrolDistanceToTravel;

        bool facingRight = !m_SpriteRenderer.flipX;
        float distanceSign = facingRight ? -1f : 1f;

        float xDis = transform.position.x + (distanceSign * determinedRandomDistance);
        float yDis = m_CharacterController2D.Rigidbody2D.position.y;

        targetPosition = new Vector2(xDis, yDis);

        return targetPosition;
    }

    #endregion // PATROLLING

    #region ATTACKING

    public void ChargingTarget()
    {
        if (m_Target == null)
            return;

        m_MoveVector.x = (m_Target.position.x - transform.position.x > 0) ? runSpeed : -runSpeed;
        SetViewDirection((int)Mathf.Sign((m_Target.position.x - transform.position.x) * runSpeed));
    }

    public void CheckAttackRange()
    {
        if (m_Target == null)
            return;

        if (Vector2.Distance(m_Target.position, m_CharacterController2D.Rigidbody2D.position) <= attackRange)
        {
            m_Animator.SetTrigger(m_HashAttackPara);
        }
    }

    public void MeleeAttack()
    {
        m_MoveVector.x = 0f;

        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D hitInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (hitInfo != null)
        {
            Debug.Log("Attack Hit Info");
        }
    }

    public void CheckTargetStillVisible()
    {
        if (m_TimeBeforeLostTarget <= 0)
        {
            m_Target = null;

            m_Animator.ResetTrigger(m_HashSpottedPara);
            m_Animator.SetTrigger(m_HashIdlePara);

            m_MoveVector.x = 0f;

            m_PatrolWaitTime = Random.Range(minPatrolWaitTime, maxPatrolWaitTime);
        }
    }

    #endregion

    public void CheckForTarget()
    {
        Vector3 circleDirection;
        if (m_SpriteRenderer.flipX == false)
        {
            circleDirection = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.y + viewDirection) * Vector3.right;
        }
        else
        {
            circleDirection = Quaternion.Euler(0, 180f, transform.rotation.eulerAngles.y + viewDirection) * Vector3.right;
        }

        Vector3 circleCenter = transform.position + Vector3.up * viewPointHeight;
        float circleRadius = viewDistance;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(circleCenter, circleRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                m_Target = collider.GetComponent<Transform>();

                Vector2 playerDirection = ((Vector2)collider.transform.position - (Vector2)circleCenter).normalized;
                float angle = Vector2.Angle(circleDirection, playerDirection);

                if (angle < viewFov * 0.5f)
                {
                    m_Animator.SetTrigger(m_HashSpottedPara);
                    m_TimeBeforeLostTarget = timeBeforeLostTarget;
                }
            }
        }

    }

    void SetViewDirection(int facing)
    {
        if (facing == -1)
        {
            m_SpriteRenderer.flipX = true;
        }
        else if (facing == 1)
        {
            m_SpriteRenderer.flipX = false;
        }
    }

    void UpdateTimers()
    {
        if (m_PatrolWaitTime > 0.0f)
            m_PatrolWaitTime -= Time.deltaTime;

        if (m_TimeBeforeLostTarget > 0.0f)
            m_TimeBeforeLostTarget -= Time.deltaTime;
    }

#if UNITY_EDITOR
    #region CONTROL PANEL PARAMETERS
    [Header("CONTROL PANEL (ONLY EDITOR)")]

    [SerializeField]
    bool showViewField;
    [SerializeField]
    Color colorViewField = new(0f, 1.0f, 0f, 0.05f);
    
    [SerializeField]
    bool showPatrolTargetPosition;
    [SerializeField]
    Color colorPatrolTargetPosition= Color.blue;

    [SerializeField]
    bool showGroundDetectSphere;
    [SerializeField]
    Color colorGroundDetectSphere= Color.cyan;

    #endregion // CONTROL PANEL

    void OnDrawGizmosSelected()
    {
        if (showViewField)
        {
            Vector3 circleCenter = transform.position + Vector3.up * viewPointHeight;

            // Anpassung der Blickrichtung basierend auf der Charakterrotation
            Vector3 forward;
            if (m_SpriteRenderer != null)
            {
                if (m_SpriteRenderer.flipX == false)
                {
                    // Charakter schaut nach rechts
                    forward = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.y + viewDirection) * Vector3.right;
                }
                else
                {
                    // Charakter schaut nach links
                    forward = Quaternion.Euler(0, 180f, transform.rotation.eulerAngles.y + viewDirection) * Vector3.right;
                }
            }
            else
            {
                forward = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.y + viewDirection) * Vector3.right;
            }

            forward = Vector3.ClampMagnitude(forward, 1);

            Vector3 endpoint = circleCenter + (Quaternion.Euler(0, 0, viewFov * 0.5f) * forward);

            Handles.color = colorViewField;
            Handles.DrawSolidArc(circleCenter, -Vector3.forward, (endpoint - circleCenter).normalized, viewFov, viewDistance);
        }

        if (showPatrolTargetPosition)
        {
            Gizmos.color = colorPatrolTargetPosition;

            if (!EditorApplication.isPlaying)
            {
                Vector2 newPos = new(transform.position.x + patrolDistanceToTravel, transform.position.y);
                Gizmos.DrawSphere(newPos, 0.5f);
            }
            else
            {
                Vector2 newPos = new(m_TargetPatrolPosition.x, m_CharacterController2D.Rigidbody2D.position.y);
                Gizmos.DrawSphere(newPos, 0.5f);
            }
        }

        if (showGroundDetectSphere)
        {
            Gizmos.color = colorGroundDetectSphere;
            Vector2 sphereOffset;
            if (m_SpriteRenderer != null)
            {
                float offsetX = m_SpriteRenderer.flipX ? -abyssCheckSphereOffset.x : abyssCheckSphereOffset.x;
                sphereOffset = new(offsetX, abyssCheckSphereOffset.y);
            }
            else
            {
                sphereOffset = abyssCheckSphereOffset;
            }

            Vector2 spherePosition = transform.position + transform.TransformDirection(sphereOffset);
            Gizmos.DrawWireSphere(spherePosition, m_AbyssCheckSphereRadius);
        }
    }
#endif
}
