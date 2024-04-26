using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField]
    LayerMask grdLayerMask;

    [SerializeField]
    Vector2 grdCheckOffset;

    [SerializeField]
    Vector2 grdCheckRadius;

    Rigidbody2D m_Rigidbody2D;
    Vector2 m_PreviousPosition;
    Vector2 m_CurrentPosition;
    Vector2 m_NextMovement;

    public bool IsGrounded { get; protected set; }
    public Vector2 Velocity { get; protected set; }
    public Rigidbody2D Rigidbody2D { get { return m_Rigidbody2D; } }

    void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        m_CurrentPosition = m_Rigidbody2D.position;
        m_PreviousPosition = m_Rigidbody2D.position;

        Physics2D.queriesStartInColliders = false;
    }

    void FixedUpdate()
    {
        CheckGroundedState();

        m_PreviousPosition = m_Rigidbody2D.position;
        m_CurrentPosition = m_PreviousPosition + m_NextMovement;
        Velocity = (m_CurrentPosition - m_PreviousPosition) / Time.deltaTime;

        m_Rigidbody2D.MovePosition(m_CurrentPosition);
        m_NextMovement = Vector2.zero;
    }

    /// <summary>
    /// This moves a rigidbody and so should only be called from FixedUpdate or other Physics messages.
    /// </summary>
    /// <param name="movement">The amount moved in global coordinates relative to the rigidbody2D's position.</param>
    public void Move(Vector2 movement)
    {
        m_NextMovement += movement;
    }
    
    void CheckGroundedState()
    {
        Vector2 baseGrdCheckPosition = new(transform.position.x, transform.position.y);
        Vector2 grdCheckPosition = baseGrdCheckPosition + grdCheckOffset;

        IsGrounded = Physics2D.OverlapBox(grdCheckPosition, grdCheckRadius, 0, grdLayerMask);
    }

#if UNITY_EDITOR

    [Header("CONTROL PANEL (ONLY EDITOR)")]

    [SerializeField]
    bool showGroundCheckPosition = false;

    [SerializeField]
    Color colorGroundCheckMark = Color.yellow;

    void OnDrawGizmosSelected()
    {
        if (showGroundCheckPosition)
        {
            Gizmos.color = colorGroundCheckMark;
            Vector2 baseGrdCheckPosition = new(transform.position.x, transform.position.y);
            Vector2 grdCheckPosition = baseGrdCheckPosition + grdCheckOffset;

            Vector2 groundCheckPosition = grdCheckPosition;
            Gizmos.DrawCube(groundCheckPosition, grdCheckRadius);
        }
    }

#endif
}
