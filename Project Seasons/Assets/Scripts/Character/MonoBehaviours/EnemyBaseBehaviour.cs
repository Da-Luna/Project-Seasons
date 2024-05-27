using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyBaseBehaviour : MonoBehaviour
{
    // Movement parameters  
    [SerializeField] float patrolSpeed = 0.8f;
    public float runSpeed = 0.8f;
    [SerializeField] float gravity = 40.0f;
    public bool useGravity = true;

    // Patrolling parameters
    [SerializeField, Tooltip("The distance at which the m_TargetPatrolPosition is set.")]
    float patrolDistanceToTravel = 5f;
    [SerializeField, Tooltip("Minimum time the enemy waits before patrolling.")]
    float minPatrolWaitTime = 1.5f;
    [SerializeField, Tooltip("Maximum time the enemy waits before patrolling.")]
    float maxPatrolWaitTime = 3.0f;

    // Patrolling obstacle parameters
    [SerializeField, Tooltip("LayerMask for detecting ground obstacles.")]
    LayerMask groundObstacleMask;
    [SerializeField, Tooltip("LayerMask for detecting wall obstacles.")]
    LayerMask wallObstacleMask;
    [SerializeField, Tooltip("Offset position of abyss detection sphere.")]
    Vector2 abyssCheckSphereOffset = new(1.5f, 0f);
    readonly float m_AbyssCheckSphereRadius = 0.5f;

    // Targeting parameters
    [SerializeField, Range(0.0f, 10.0f), Tooltip("Height of the starting point of the field of view.")]
    float viewPointHeight = 1.0f;
    [SerializeField, Range(0.0f, 360.0f), Tooltip("Rotation of the entire field of view.")]
    float viewDirection = 90.0f;
    [SerializeField, Range(0.0f, 360.0f), Tooltip("Size of the field of view.")]
    float viewFov = 180.0f;
    [SerializeField, Tooltip("Maximum range of the field of view.")]
    float viewDistance = 20.0f;
    [Tooltip("Time without target in the view cone before target is considered lost.")]
    public float timeBeforeLostTarget = 3.0f;

    // Attacking parameters
    [SerializeField, Tooltip("LayerMask for recognizing obstacles to avoid continuous attacks against walls.")]
    LayerMask attackObstacleMask;
    [SerializeField, Tooltip("LayerMask for detecting targets.")]
    LayerMask attackMask;
    [SerializeField, Tooltip("Additional offset for the attack detection sphere.")]
    Vector2 attackOffset = new(2.5f, 1.2f);
    [SerializeField, Tooltip("Minimum range from which the character starts attacking.")]
    float attackRange = 1.2f;
    [SerializeField, Tooltip("Radius of the sphere in which the player takes damage.")]
    float attackRadius = 1.2f;

    // References to components
    protected SpriteRenderer m_SpriteRenderer;
    protected CharacterController2D m_CharacterController2D;
    protected Animator m_Animator;

    // Movement vectors and targets
    protected Vector2 m_MoveVector;
    public  Transform m_Target;
    protected Vector2 m_TargetPatrolPosition;
    protected float m_PatrolWaitTime;
    protected float m_TimeBeforeLostTarget;

    // Flag for character death
    protected bool m_Dead = false;

    // Animator parameter hashes
    protected readonly int m_HashIdlePara = Animator.StringToHash("Idle");
    protected readonly int m_HashPatrolPara = Animator.StringToHash("Patrol");
    protected readonly int m_HashSpottedPara = Animator.StringToHash("Spotted");
    protected readonly int m_HashAttackPara = Animator.StringToHash("Attack");
    protected readonly int m_HashAttackIdlePara = Animator.StringToHash("AttackIlde");
    protected readonly int m_HashGroundedPara = Animator.StringToHash("Grounded");

    public virtual void Awake()
    {
        m_CharacterController2D = GetComponent<CharacterController2D>();
        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void Start()
    {
        SceneLinkedSMB<EnemyBaseBehaviour>.Initialise(m_Animator, this);       
        m_PatrolWaitTime = Random.Range(minPatrolWaitTime, maxPatrolWaitTime);
    }

    void FixedUpdate()
    {
        if (m_Dead)
            return;

        // Apply gravity    
        if(useGravity) m_MoveVector.y = Mathf.Max(m_MoveVector.y - gravity * Time.deltaTime, -gravity);

        // Move the character
        m_CharacterController2D.Move(m_MoveVector * Time.deltaTime);

        // Update timers
        UpdateTimers();

        // Update grounded status for the animator
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
        m_MoveVector.x = (m_TargetPatrolPosition.x - transform.position.x > 0) ? patrolSpeed : -patrolSpeed;
    }

    public virtual void PatrollingWalk(bool useabyss = true)
    {
        bool reachedTarget = Mathf.Abs(transform.position.x - m_TargetPatrolPosition.x) < 0.1f;

        // Calculate sphere position for obstacle detection
        float offsetX = m_SpriteRenderer.flipX ? -abyssCheckSphereOffset.x : abyssCheckSphereOffset.x;
        Vector2 sphereOffset = new(offsetX, abyssCheckSphereOffset.y);
        Vector2 spherePosition = transform.position + transform.TransformDirection(sphereOffset);

        // Detect obstacles and stop if necessary
        bool wallDetected = Physics2D.OverlapCircleAll(spherePosition, m_AbyssCheckSphereRadius, wallObstacleMask).Length > 0;
        bool abyssDetected = useabyss && Physics2D.OverlapCircleAll(spherePosition, m_AbyssCheckSphereRadius, groundObstacleMask).Length == 0;

        if (reachedTarget || abyssDetected || wallDetected)
        {
            m_MoveVector = Vector2.zero;
            m_PatrolWaitTime = Random.Range(minPatrolWaitTime, maxPatrolWaitTime);

            m_Animator.SetTrigger(m_HashIdlePara);

            PatrollingWait();
        }
    }

    /// <summary>
    /// Sets the next patrol position.
    /// </summary>
    /// <param name="targetPosition">Reference to the target position vector.</param>
    /// <returns>The updated target position vector.</returns>
    Vector2 SetPatrolPosition(ref Vector2 targetPosition)
    {
        float determinedPatrolPosition = patrolDistanceToTravel;

        bool facingRight = !m_SpriteRenderer.flipX;
        float distanceSign = facingRight ? -1f : 1f;

        float xDis = transform.position.x + (distanceSign * determinedPatrolPosition);
        float yDis = m_CharacterController2D.Rigidbody2D.position.y;

        targetPosition = new Vector2(xDis, yDis);

        return targetPosition;
    }

    #endregion // PATROLLING

    #region ATTACKING

    //Method to use in specific enemy scripts for movement on X and Y Axes when Moving to the Target
    public virtual void ChargingToTarget() { }
   
    public void UpdateMeleeAttackRangeCheck()
    {
        if (m_Target == null)
            return;

        // Check if target is in melee attack range
        Vector2 characterPosition = new(transform.position.x, transform.position.y + attackOffset.y);
        if (Vector2.Distance(m_Target.position, characterPosition) <= attackRange)
        {
            RaycastHit2D hit = Physics2D.Raycast(characterPosition, ((Vector2)m_Target.position - characterPosition).normalized, attackRange, attackObstacleMask);

            // If the Raycast doesn't hit anything, trigger attack animation
            if (hit.collider == null)// || canEnemyFly)
            {
                m_Animator.SetTrigger(m_HashAttackPara);
            }
        }
    }

    public void UpdateMagicianAttackRangeCheck()
    {
        if (m_Target == null)
            return;

        // Check if target is in magician attack range
        if (Vector2.Distance(m_Target.position, transform.position) <= attackRange)
        {
            m_Animator.SetTrigger(m_HashAttackPara);
        }
    }

    public void UpdateTargetStillVisibleCheck()
    {
        if (m_TimeBeforeLostTarget <= 0)
        {
            m_Target = null;

            m_Animator.ResetTrigger(m_HashSpottedPara);
            m_Animator.SetTrigger(m_HashIdlePara);

            m_MoveVector.x = 0f;
            m_MoveVector.y = 0f;

            m_PatrolWaitTime = Random.Range(minPatrolWaitTime, maxPatrolWaitTime);
        }
    }

    public void MeleeAttack()
    {
        m_MoveVector.x = 0f;
        m_MoveVector.y = 0f;

        // Calculate attack position
        int side = 1;
        if (m_SpriteRenderer != null && m_SpriteRenderer.flipX)
        {
            side = -1;
        }

        Vector3 pos = transform.position;
        pos += transform.right * (attackOffset.x * side);
        pos += transform.up * attackOffset.y;

        // Check for targets in attack range
        Collider2D hitInfo = Physics2D.OverlapCircle(pos, attackRadius, attackMask);
        if (hitInfo != null)
        {
            Debug.Log("Attack Hit Info");
        }
    }

    #endregion

    #region GENERAL

    public void UpdateCheckForTarget()
    {
        // Get the direction of the field of view
        Vector3 circleDirection = m_SpriteRenderer.flipX == false ?
            Quaternion.Euler(0, 0, transform.rotation.eulerAngles.y + viewDirection) * Vector3.right :
            Quaternion.Euler(0, 180f, transform.rotation.eulerAngles.y + viewDirection) * Vector3.right;

        Vector3 circleCenter = transform.position + Vector3.up * viewPointHeight;
        float circleRadius = viewDistance;

        // Check for targets in the field of view
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

    public void UpdateLookToTarget()
    {
        // Set the character's facing direction towards the target
        SetViewDirection((int)Mathf.Sign(((transform.position.x - m_Target.position.x) < 0) ? 1 : -1));
    }

    public void UpdateLookToPatrolPosition()
    {
        // Set the character's facing direction towards the patrol position
        SetViewDirection((int)Mathf.Sign(((transform.position.x - m_TargetPatrolPosition.x) < 0) ? 1 : -1));
    }

    #endregion // GENERAL

    // Method to set the view direction of the character
    public virtual void SetViewDirection(int facing)
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

    // Method to update timers used for various time-based operations
    public virtual void UpdateTimers()
    {
        if (m_PatrolWaitTime > 0.0f)
            m_PatrolWaitTime -= Time.deltaTime;

        if (m_TimeBeforeLostTarget > 0.0f)
            m_TimeBeforeLostTarget -= Time.deltaTime;
    }

#if UNITY_EDITOR
    #region CONTROL PANEL PARAMETERS

    [SerializeField]
    bool showViewField;
    [SerializeField]
    Color colorViewField = new(0f, 1.0f, 0f, 0.05f);

    [SerializeField]
    bool showPatrolTargetPosition;
    [SerializeField]
    Color colorPatrolTargetPosition = Color.blue;

    [SerializeField]
    bool showGroundDetectSphere;
    [SerializeField]
    Color colorGroundDetectSphere = Color.cyan;

    [SerializeField]
    bool showAttackProperties;
    [SerializeField]
    Color colorAttackField = Color.red;

    [SerializeField]
    Transform attackTarget;
    #endregion // CONTROL PANEL

    void OnDrawGizmos()
    {
        // Draw field of view
        if (showViewField)
        {
            Vector3 circleCenter = transform.position + Vector3.up * viewPointHeight;

            // Adjust view direction based on character rotation
            Vector3 forward = m_SpriteRenderer != null && !m_SpriteRenderer.flipX ?
                Quaternion.Euler(0, 0, transform.rotation.eulerAngles.y + viewDirection) * Vector3.right :
                Quaternion.Euler(0, 180f, transform.rotation.eulerAngles.y + viewDirection) * Vector3.right;

            forward = Vector3.ClampMagnitude(forward, 1);

            Vector3 endpoint = circleCenter + (Quaternion.Euler(0, 0, viewFov * 0.5f) * forward);

            Handles.color = colorViewField;
            Handles.DrawSolidArc(circleCenter, -Vector3.forward, (endpoint - circleCenter).normalized, viewFov, viewDistance);
        }

        // Draw patrol target position
        if (showPatrolTargetPosition)
        {
            Gizmos.color = colorPatrolTargetPosition;

            Vector2 newPos = !EditorApplication.isPlaying ?
                new Vector2(transform.position.x + patrolDistanceToTravel, transform.position.y) :
                new Vector2(m_TargetPatrolPosition.x, m_CharacterController2D.Rigidbody2D.position.y);

            Gizmos.DrawSphere(newPos, 0.5f);
        }

        // Draw ground detection sphere
        if (showGroundDetectSphere)
        {
            Gizmos.color = colorGroundDetectSphere;
            Vector2 sphereOffset = m_SpriteRenderer != null ?
                (m_SpriteRenderer.flipX ? new Vector2(-abyssCheckSphereOffset.x, abyssCheckSphereOffset.y) : abyssCheckSphereOffset) :
                abyssCheckSphereOffset;

            Vector2 spherePosition = transform.position + transform.TransformDirection(sphereOffset);
            Gizmos.DrawWireSphere(spherePosition, m_AbyssCheckSphereRadius);
        }

        // Draw attack properties
        if (showAttackProperties)
        {
            Gizmos.color = colorAttackField;

            int side = 1;
            if (m_SpriteRenderer != null && m_SpriteRenderer.flipX)
            {
                side = -1;
            }

            Vector3 attackFieldPosition = transform.position + new Vector3(attackOffset.x * side, attackOffset.y, 0f);
            Gizmos.DrawWireSphere(attackFieldPosition, attackRadius);

            Vector2 characterPosition = new(transform.position.x, transform.position.y + attackOffset.y);

            if (attackTarget != null)
            {
                Vector2 direction = (Vector2)attackTarget.position - characterPosition;
                Vector2 minRequiredPosition = characterPosition + direction.normalized * attackRange;

                if (Vector2.Distance(attackTarget.position, characterPosition) <= attackRange)
                {
                    RaycastHit2D hit = Physics2D.Raycast(characterPosition, ((Vector2)attackTarget.position - characterPosition).normalized, attackRange, attackObstacleMask);

                    // If the Raycast doesn't hit anything, draw green line, else draw yellow line
                    if (hit.collider == null)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawLine(characterPosition, attackTarget.position);

                        Gizmos.color = Color.green;
                        Gizmos.DrawWireSphere(attackTarget.position, 0.2f);
                    }
                    else
                    {
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawLine(characterPosition, attackTarget.position);

                        Gizmos.color = Color.red;
                        Gizmos.DrawWireSphere(attackTarget.position, 0.2f);
                    }
                }
                else
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(characterPosition, minRequiredPosition);

                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(minRequiredPosition, 0.2f);
                }
            }
        }
    }
#endif
}
