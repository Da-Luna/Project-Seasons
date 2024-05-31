using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The Damager class represents an object that can deal damage to Damageable objects.
/// It handles collision detection and triggers appropriate events when damage is dealt.
/// </summary>
public class Damager : MonoBehaviour
{
    [Serializable]
    public class DamageableEvent : UnityEvent<Damager, DamageController>
    { }

    [Serializable]
    public class NonDamageableEvent : UnityEvent<Damager>
    { }

    /// <summary>
    /// Gets the last collider hit by the Damager.
    /// </summary>
    public Collider2D LastHit { get { return m_LastHit; } }

    [Tooltip("Amount of damage this damager deals.")]
    public float damage = 1;

    [Tooltip("Offset of the damager from its transform position.")]
    public Vector2 offset = new(1.5f, 1f);

    [Tooltip("Size of the damager's hit area.")]
    public Vector2 size = new(2.5f, 1f);

    [Tooltip("If set, the offset x will be changed based on the sprite's flipX setting. This makes the damager always face the direction of the sprite.")]
    public bool offsetBasedOnSpriteFacing = true;

    [Tooltip("SpriteRenderer used to read the flipX value when using offsetBasedOnSpriteFacing.")]
    public SpriteRenderer spriteRenderer;

    [Tooltip("If disabled, the damager will ignore triggers when checking for hits.")]
    public bool canHitTriggers;

    [Tooltip("If set, the damager will be disabled after hitting an object.")]
    public bool disableDamageAfterHit = false;

    [Tooltip("If set, the player will be forced to respawn to the latest checkpoint in addition to losing life.")]
    public bool forceRespawn = false;

    [Tooltip("If set, invincible damageable objects will still receive the onHit message (but won't lose any life).")]
    public bool ignoreInvincibility = false;

    [Tooltip("Layers that the damager can hit.")]
    public LayerMask hittableLayers;

    [Tooltip("Event triggered when a Damageable object is hit.")]
    public DamageableEvent OnDamageableHit;

    [Tooltip("Event triggered when a non-Damageable object is hit.")]
    public NonDamageableEvent OnNonDamageableHit;

    protected bool m_SpriteOriginallyFlipped;
    protected bool m_CanDamage = true;
    protected ContactFilter2D m_AttackContactFilter;
    protected Collider2D[] m_AttackOverlapResults = new Collider2D[10];
    protected Transform m_DamagerTransform;
    protected Collider2D m_LastHit;

    /// <summary>
    /// Initializes the damager's contact filter and other settings.
    /// </summary>
    void Awake()
    {
        m_AttackContactFilter.layerMask = hittableLayers;
        m_AttackContactFilter.useLayerMask = true;
        m_AttackContactFilter.useTriggers = canHitTriggers;

        if (offsetBasedOnSpriteFacing && spriteRenderer != null)
            m_SpriteOriginallyFlipped = spriteRenderer.flipX;

        m_DamagerTransform = transform;
    }

    /// <summary>
    /// Enables the damager to deal damage.
    /// </summary>
    public void EnableDamage()
    {
        m_CanDamage = true;
    }

    /// <summary>
    /// Disables the damager from dealing damage.
    /// </summary>
    public void DisableDamage()
    {
        m_CanDamage = false;
    }

    /// <summary>
    /// Checks for collisions and deals damage if a Damageable object is hit.
    /// </summary>
    void FixedUpdate()
    {
        if (!m_CanDamage)
            return;

        Vector2 scale = m_DamagerTransform.lossyScale;

        Vector2 facingOffset = Vector2.Scale(offset, scale);
        if (offsetBasedOnSpriteFacing && spriteRenderer != null && spriteRenderer.flipX != m_SpriteOriginallyFlipped)
            facingOffset = new Vector2(-offset.x * scale.x, offset.y * scale.y);

        Vector2 scaledSize = Vector2.Scale(size, scale);

        Vector2 pointA = (Vector2)m_DamagerTransform.position + facingOffset - scaledSize * 0.5f;
        Vector2 pointB = pointA + scaledSize;

        int hitCount = Physics2D.OverlapArea(pointA, pointB, m_AttackContactFilter, m_AttackOverlapResults);

        for (int i = 0; i < hitCount; i++)
        {
            m_LastHit = m_AttackOverlapResults[i];
            DamageController damageable = m_LastHit.GetComponent<DamageController>();

            if (damageable)
            {
                OnDamageableHit.Invoke(this, damageable);
                damageable.TakeDamage(this, ignoreInvincibility);
                if (disableDamageAfterHit)
                    DisableDamage();
            }
            else
            {
                OnNonDamageableHit.Invoke(this);
            }
        }
    }
}
