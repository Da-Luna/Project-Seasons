using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class InteractCarrybleBox : MonoBehaviour
{
    [Header("BASE SETTINGS")]
    
    [SerializeField, Tooltip("")]
    float m_CarryBoxSpeedPickUp = 4.6f;

    [SerializeField, Tooltip("")]
    float m_CarryBoxSpeedWalk = 25f;

    [SerializeField, Tooltip("")]
    Vector2 m_CarryOffsetPositionIdle = new(0.95f, 1.5f);

    [SerializeField, Tooltip("")]
    Vector2 m_CarryOffsetPositionWalk = new(0.88f, 1.15f);

    [Header("BASE SETTINGS")]

    [SerializeField, Tooltip("")]
    Vector2 playerColliderSizeXCarry = new(1.8f, 1.1f);

    // Player References
    Animator m_PlayerAnimator;
    BoxCollider2D m_PlayerBoxCollider2D;
    Vector2 m_PlayerColliderSizeXBase;
    Transform m_PlayerTransform;
    SpriteRenderer m_PlayerSpriteRenderer;

    // Carrybale Box References
    Rigidbody2D m_RigidBody2D;
    Collider2D m_Collider2D;

    // Carrybale Box Parameters
    bool m_StartCarrying;
    bool m_IsCarrying;
    bool m_Request;
    bool m_InTrigger;
    Vector2 m_CurrentBoxPosition;
    Vector2 m_TargetBoxPosition;

    const string m_PlayerLayerMask = "Player";
    readonly int m_HashCarryPara = Animator.StringToHash("Carry");
    readonly int m_HashHorizontalSpeedPara = Animator.StringToHash("HorizontalSpeed");

    void Start()
    {
        m_RigidBody2D = GetComponent<Rigidbody2D>();
        m_Collider2D = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag(m_PlayerLayerMask))
        {
            m_PlayerAnimator = target.GetComponent<Animator>();
            m_PlayerBoxCollider2D = target.GetComponent<BoxCollider2D>();
            m_PlayerColliderSizeXBase = m_PlayerBoxCollider2D.size;
            m_PlayerTransform = target.transform;
            m_PlayerSpriteRenderer = target.GetComponent<SpriteRenderer>();

            m_InTrigger = true;
        }
    }

    void OnTriggerStay2D(Collider2D target)
    {
        if (target.CompareTag(m_PlayerLayerMask))
        {
            HandleCarrying();
            Carrying();
        }
    }

    void OnTriggerExit2D(Collider2D target)
    {
        if (target.CompareTag(m_PlayerLayerMask))
        {
            m_InTrigger = false;
        }
    }
    void UpdatePositions()
    {
        Vector2 _playerPos = m_PlayerTransform.position;
        float xOffset;

        if (m_PlayerAnimator.GetFloat(m_HashHorizontalSpeedPara) == 0 || !m_IsCarrying)
        {
            xOffset = m_PlayerSpriteRenderer.flipX ? -Mathf.Abs(m_CarryOffsetPositionIdle.x) : Mathf.Abs(m_CarryOffsetPositionIdle.x);
            m_TargetBoxPosition = new Vector2(_playerPos.x + xOffset, _playerPos.y + m_CarryOffsetPositionIdle.y);
        }
        else if (m_IsCarrying)
        {
            xOffset = m_PlayerSpriteRenderer.flipX ? -Mathf.Abs(m_CarryOffsetPositionWalk.x) : Mathf.Abs(m_CarryOffsetPositionWalk.x);
            m_TargetBoxPosition = new Vector2(_playerPos.x + xOffset, _playerPos.y + m_CarryOffsetPositionWalk.y);
        }

        m_CurrentBoxPosition = transform.position;
    }

    void HandleCarrying()
    {
        if (m_Request)
        {
            if (!m_StartCarrying)
            {
                m_RigidBody2D.bodyType = RigidbodyType2D.Kinematic;
                m_RigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                m_Collider2D.enabled = false;

                m_PlayerAnimator.SetBool(m_HashCarryPara, true);
                m_PlayerBoxCollider2D.size = playerColliderSizeXCarry;
                
                m_StartCarrying = true;
            }
            else
            {
                m_RigidBody2D.bodyType = RigidbodyType2D.Dynamic;
                m_RigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                m_Collider2D.enabled = true;

                m_PlayerAnimator.SetBool(m_HashCarryPara, false);
                m_PlayerBoxCollider2D.size = m_PlayerColliderSizeXBase;

                m_IsCarrying = false;
                m_StartCarrying = false;
            }

            m_Request = false;
        }
    }

    void Carrying()
    {
        if (m_StartCarrying)
        {
            UpdatePositions();

            if (m_IsCarrying)
            {
                if (m_CurrentBoxPosition != m_TargetBoxPosition)
                {
                    transform.position = Vector2.MoveTowards(transform.position, m_TargetBoxPosition, m_CarryBoxSpeedWalk * Time.deltaTime);
                }
            }
            else
            {
                if (Vector2.Distance(m_CurrentBoxPosition, m_TargetBoxPosition) < 0.05f)
                {
                    m_IsCarrying = true;
                }
                else
                {
                    transform.position = Vector2.Lerp(transform.position, m_TargetBoxPosition, m_CarryBoxSpeedPickUp * Time.deltaTime);
                }
            }
        }
    }

    void Update()
    {
        if (!m_InTrigger)
        {
            return;
        }
        if (m_Request)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            m_Request = true;
        }
    }
}
