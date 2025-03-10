using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class PlayerCharacter : MonoBehaviour
{
    static protected PlayerCharacter s_PlayerInstance;

    static public PlayerCharacter PlayerInstance { get { return s_PlayerInstance; } }

    #region REFERENCES PROPERTIES

    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    HealthController healthController;

    public HealthController PlayerHealthController { get { return healthController; } }

    [SerializeField]
    AetherController aetherController;

    public AetherController PlayerAether{ get { return aetherController; } }

    [SerializeField]
    Transform facingLeftBulletSpawnPoint;

    [SerializeField]
    Transform facingRightBulletSpawnPoint;

    [SerializeField]
    BulletPool bulletPoolLightAttack;

    [SerializeField]
    BulletPool bulletPoolHeavyAttack;

    [SerializeField]
    BulletPool bulletPoolSuperAttack;

    #endregion // REFERENCES PROPERTIES

    #region MOVEMENT PROPERTIES

    [SerializeField, Tooltip("Base movement speed of the player")]
    float moveSpeedWalk = 6.0f;

    [SerializeField, Tooltip("Running speed of the player")]
    float moveSpeedRun = 14.0f;

    [SerializeField]
    float groundAcceleration = 100f;

    [SerializeField]
    float groundDeceleration = 100f;

    [SerializeField, Range(0f, 1f)]
    float pushingSpeedProportion;

    public float PushingSpeedProportion { get { return pushingSpeedProportion; } }

    #endregion // MOVEMENT PROPERTIES

    #region INTERACTING PROPERTIES

    public bool IsInteracting { get; protected set; }
    public bool inCarryTrigger;
    public bool startCarryTrigger;
    public bool inFocusTrigger;
    public bool startFocusTrigger;

    #endregion // INTERACTING PROPERTIES

    #region JUMP PROPERTIES

    [SerializeField, Range(0f, 5f)]
    float airborneAccelProportion = 1f;

    [SerializeField, Range(0f, 5f)]
    float airborneDecelProportion = 0.5f;

    [SerializeField]
    float gravity = 46f;

    [SerializeField, Tooltip("This value limits the falling speed. Enter a positive value! When executed, it is converted to a negative value")]
    float limitYGravity = 80f;

    [SerializeField]
    float jumpSpeed = 26f;

    [SerializeField]
    float jumpAbortSpeedReduction = 100f;

    bool m_JumpRequested;

    public float JumpSpeed { get { return jumpSpeed; } }

    #endregion // JUMP PROPERTIES

    #region DASHING PROPERTIES

    [SerializeField]
    float dashSpeed = 20f;

    [SerializeField]
    float dashAcceleration = 2000f;

    [SerializeField]
    float dashCooldownTime = 1f;

    public UnityEvent<float> OnDashCooldownChanged = new();

    float m_DashCurrentCooldownTime;
    bool m_DashRequested;

    public float DashCooldownTime { get { return dashCooldownTime; } }
    public float DashCurrentCooldownTime
    {
        get { return m_DashCurrentCooldownTime; }
        private set
        {
            m_DashCurrentCooldownTime = value;
            OnDashCooldownChanged.Invoke(m_DashCurrentCooldownTime);
        }
    }

    #endregion // DASHING PROPERTIES

    #region HEALING PROPERTIES

    [Tooltip("The health value that the player get from heal potion.")]
    public float healPotionValue = 1.0f;

    [SerializeField, Tooltip("The maximum value for heal potion in inventory.")]
    int maxHealthPotionInInventory = 10;

    protected int healthPotionCounter;

    public int HealthItemCounter { get { return healthPotionCounter; } }
    public bool CanCollectHealthItem { get { return healthPotionCounter < maxHealthPotionInInventory; } }

    public UnityEvent<int> OnHealthItemCounterChanged = new();

    #endregion // HEALTH PROPERTIES

    #region AETHER PROPERTIES

    [Tooltip("The aether value that the player get from heal potion.")]
    public float aetherPotionValue = 1.0f;

    [SerializeField, Tooltip("The maximum value for aether potion in inventory.")]
    int maxAetherPotionInInventory = 10;

    protected int aetherPotionCounter;

    public int AetherItemCounter { get { return aetherPotionCounter; } }
    public bool CanCollectAetherItem { get { return aetherPotionCounter < maxAetherPotionInInventory; } }

    public UnityEvent<int> OnAetherItemCounterChanged = new();

    #endregion // AETHER PROPERTIES

    #region HURT PROPERTIES

    [SerializeField, Range(k_MinHurtJumpAngle, k_MaxHurtJumpAngle)]
    float hurtJumpAngle = 45f;

    [SerializeField]
    float hurtJumpSpeed = 5f;

    [SerializeField]
    float flickeringDuration = 0.1f;

    public float HurtJumpSpeed { get { return hurtJumpSpeed; } }

    #endregion // HURT PROPERTIES

    #region AUDIO PROPERTIES

    [Header("References are in the player character under SoundSources!")]

    [SerializeField, Tooltip("is executed as events in the animations")]
    RandomAudioPlayer footstepAudioPlayer;

    [SerializeField, Tooltip("is executed as events in the animations")]
    RandomAudioPlayer jumpingAudioPlayer;

    [SerializeField, Tooltip("is executed in UpdateGroundedCheck")]
    RandomAudioPlayer landingAudioPlayer;

    [SerializeField, Tooltip("")]
    RandomAudioPlayer hurtAudioPlayer;

    [SerializeField, Tooltip("")]
    RandomAudioPlayer focusedAudioPlayer;

    [SerializeField, Tooltip("")]
    RandomAudioPlayer heavyAttackAudioPlayer;

    [SerializeField, Tooltip("")]
    RandomAudioPlayer superAttackAudioPlayer;

    #endregion // AUDIO PROPERTIES

    #region PARTICLE PROPERTIES

    [Header("Particle System References")]

    [SerializeField]
    ParticleSystem particleFocused;

    [SerializeField]
    ParticleSystem particleLightAttack;

    protected GameObject particleChildLightAttack;

    [Tooltip("")]
    public ParticleSystem particleForceField;

    [Header("Particle System Settings")]

    [SerializeField]
    Vector3 jumpDustPositionOffset;
    
    [SerializeField]
    Vector3 landDustPositionOffset;

    #endregion // PARTICLE PROPERTIES

    #region OTHER PROPERTIES

    protected CharacterController2D m_CharacterController2D;
    protected Animator m_Animator;
    protected CapsuleCollider2D m_Capsule;
    protected Transform m_Transform;
    protected Vector2 m_MoveVector;
    protected List<Pushable> m_CurrentPushables = new(4);
    protected Pushable m_CurrentPushable;
    protected float m_TanHurtJumpAngle;
    protected WaitForSeconds m_FlickeringWait;
    protected Coroutine m_FlickerCoroutine;
    protected float m_CurrentSpeed;
    protected Transform m_CurrentBulletSpawnPoint;
    protected TileBase m_CurrentSurface;

    protected bool m_AttackSuperRequested;
    
    protected bool m_QuickslotAPressed;
    protected bool m_QuickslotBPressed;
    protected bool m_QuickslotCPressed;
    protected bool m_QuickslotDPressed;

    protected readonly int m_HashHorizontalSpeedPara = Animator.StringToHash("HorizontalSpeed");
    protected readonly int m_HashVerticalSpeedPara = Animator.StringToHash("VerticalSpeed");
    protected readonly int m_HashGroundedPara = Animator.StringToHash("Grounded");
    protected readonly int m_HashSwitchSidePara = Animator.StringToHash("SwitchSide");
    protected readonly int m_HashAtkLightPara = Animator.StringToHash("AtkLight");
    protected readonly int m_HashFocusedPara = Animator.StringToHash("Focused");
    protected readonly int m_HashAtkHeavyPara = Animator.StringToHash("AtkHeavy");
    protected readonly int m_HashAtkSuperPara = Animator.StringToHash("AtkSuper");
    protected readonly int m_HashPushingPara = Animator.StringToHash("Pushing");
    protected readonly int m_HashRespawnPara = Animator.StringToHash("Respawn");
    protected readonly int m_HashHurtPara = Animator.StringToHash("Hurt");
    protected readonly int m_HashDeadPara = Animator.StringToHash("Dead");
    protected readonly int m_HashForcedRespawnPara = Animator.StringToHash("ForcedRespawn");
    protected readonly int m_HashDashPara = Animator.StringToHash("Dash");

    const string m_FootstepDust = "FootstepDust";
    const string m_JumpDust = "JumpDust";
    const string m_LandDust = "LandDust";

    const string m_LightAttackShot = "LightAttackShot";
    const string m_HeavyAttackShot = "HeavyAttackShot";
    const string m_SuperAttackShot = "SuperAttackShot";

    const string m_PotionsHealthUse = "ItemPotionHealthUse";

    const float k_MinHurtJumpAngle = 0.001f;
    const float k_MaxHurtJumpAngle = 89.999f;

    const float k_GroundedStickingVelocityMultiplier = 3f; // This is to help the character stick to vertically moving platforms.

    #endregion // OTHER PROPERTIES

    void Awake()
    {
        s_PlayerInstance = this;

        m_CharacterController2D = GetComponent<CharacterController2D>();
        m_Animator = GetComponent<Animator>();
        m_Capsule = GetComponent<CapsuleCollider2D>();
        m_Transform = transform;
    }

    void Start()
    {
        SceneLinkedSMB<PlayerCharacter>.Initialise(m_Animator, this);
        m_CurrentSpeed = moveSpeedWalk;

        hurtJumpAngle = Mathf.Clamp(hurtJumpAngle, k_MinHurtJumpAngle, k_MaxHurtJumpAngle);
        m_TanHurtJumpAngle = Mathf.Tan(Mathf.Deg2Rad * hurtJumpAngle);
        m_FlickeringWait = new WaitForSeconds(flickeringDuration);

        DashCurrentCooldownTime = 0f;

        QualitySettings.vSyncCount = 1;
        Debug.LogWarning("QualitySettings.vSyncCount is 1");
    }

    #region GROUND MOVEMENT

    public void SetVerticalMovement(float newVerticalMovement)
    {
        m_MoveVector.y = newVerticalMovement;
    }

    public void GroundedVerticalMovement()
    {
        m_MoveVector.y -= gravity * Time.deltaTime;

        if (m_MoveVector.y < -gravity * Time.deltaTime * k_GroundedStickingVelocityMultiplier)
        {
            m_MoveVector.y = -gravity * Time.deltaTime * k_GroundedStickingVelocityMultiplier;
        }
    }

    public void GroundedHorizontalWalkMovement(bool useInput, float speedScale = 1f)
    {
        var pInputInstance = PlayerInput.Instance;
        m_CurrentSpeed = moveSpeedWalk;

        float desiredSpeed = useInput ? pInputInstance.InputHorizontal * m_CurrentSpeed * speedScale : 0f;
        float acceleration = useInput && pInputInstance.InputHorizontal != 0f ? groundAcceleration : groundDeceleration;

        m_MoveVector.x = Mathf.MoveTowards(m_MoveVector.x, desiredSpeed, acceleration * Time.deltaTime);
    }

    public void GroundedHorizontalRunMovement(bool useInput, float speedScale = 1f)
    {
        var pInputInstance = PlayerInput.Instance;
        m_CurrentSpeed = moveSpeedRun;

        float desiredSpeed = useInput ? pInputInstance.InputHorizontal * m_CurrentSpeed * speedScale : 0f;
        float acceleration = useInput && pInputInstance.InputHorizontal != 0f ? groundAcceleration : groundDeceleration;

        m_MoveVector.x = Mathf.MoveTowards(m_MoveVector.x, desiredSpeed, acceleration * Time.deltaTime);
    }

    public void UpdateGroundedViewDirection()
    {
        float inputHorizontal = PlayerInput.Instance.InputHorizontal;

        if (inputHorizontal != 0f)
        {
            bool isLeft = inputHorizontal < 0f;

            if (isLeft)
            {
                if (!spriteRenderer.flipX)
                    m_Animator.SetTrigger(m_HashSwitchSidePara);

                spriteRenderer.flipX = isLeft;
            }
            else
            {
                if (spriteRenderer.flipX)
                    m_Animator.SetTrigger(m_HashSwitchSidePara);

                spriteRenderer.flipX = isLeft;
            }
        }
    }

    #endregion // GROUND MOVEMENT

    #region AIRBORN BEHAVIOUR

    public bool InputCheckForJump()
    {
        bool _isJumping = PlayerInput.Instance.InputJump != 0f;

        if (_isJumping && !m_JumpRequested)
        {
            m_JumpRequested = true;
            return true;
        }
        else if (!_isJumping && m_JumpRequested)
        {
            m_JumpRequested = false;
        }

        return false;
    }

    public void UpdateJump()
    {
        if (PlayerInput.Instance.InputJump == 0f && m_MoveVector.y > 0.0f)
        {
            m_MoveVector.y -= jumpAbortSpeedReduction * Time.deltaTime;
        }
    }

    public void AirborneHorizontalMovement()
    {
        var pInputInstance = PlayerInput.Instance;
        float desiredSpeed = pInputInstance.InputHorizontal * moveSpeedRun;

        float acceleration = pInputInstance.InputHorizontal != 0f ?
            groundAcceleration * airborneAccelProportion :
            groundDeceleration * airborneDecelProportion;

        m_MoveVector.x = Mathf.MoveTowards(m_MoveVector.x, desiredSpeed, acceleration * Time.deltaTime);
    }

    public void AirborneVerticalMovement()
    {
        if (Mathf.Approximately(m_MoveVector.y, 0f) || m_CharacterController2D.IsCeilinged && m_MoveVector.y > 0f)
        {
            m_MoveVector.y = 0f;
        }

        if (m_MoveVector.y > -limitYGravity)
        {
            m_MoveVector.y -= gravity * Time.deltaTime;
        }
        else
        {
            m_MoveVector.y = -limitYGravity;
        }
    }

    #endregion // AIRBORN BEHAVIOUR

    #region ACTIONS MOVEMENT

    public void PlayFootstep()
    {
        footstepAudioPlayer.PlayRandomSound(m_CurrentSurface);
        var footstepPosition = transform.position;
        VFXController.Instance.Trigger(m_FootstepDust, footstepPosition, 0, false, null, m_CurrentSurface);
    }

    public void PlayJumping()
    {
        jumpingAudioPlayer.PlayRandomSound(m_CurrentSurface);
        var jumpDustPosition = transform.position + jumpDustPositionOffset;
        VFXController.Instance.Trigger(m_JumpDust, jumpDustPosition, 0, false, null, m_CurrentSurface);
    }

    public void PlayLanding()
    {
        landingAudioPlayer.PlayRandomSound(m_CurrentSurface);
        var landDustPosition = transform.position + landDustPositionOffset;
        VFXController.Instance.Trigger(m_LandDust, landDustPosition, 0, false, null, m_CurrentSurface);
    }

    #endregion // ACTIONS MOVEMENT

    #region INTERACTING

    public void CheckForInteractingInput()
    {
        bool inputInteracting = PlayerInput.Instance.InputInteract > 0f;

        if (inputInteracting)
        {
            if (inCarryTrigger && !startFocusTrigger)
            {
                startCarryTrigger = true;
            }
            else if (inFocusTrigger && !startCarryTrigger)
            {
                startFocusTrigger = true;
            }
        }
        else if (!inputInteracting && (startCarryTrigger || inFocusTrigger))
        {
            startCarryTrigger = false;
            inFocusTrigger = false;
        }
    }


    public void SetInteracting(bool interactState)
    {
        IsInteracting = interactState;
    }

    #endregion // INTERACTING

    #region DASHING

    public bool InputCheckForDash()
    {
        if (DashCurrentCooldownTime > 0f)
        {
            return false;
        }

        bool _isDashing = PlayerInput.Instance.InputDash != 0f;

        if (_isDashing && !m_DashRequested)
        {
            m_DashRequested = true;
            return true;
        }
        else if (!_isDashing && m_DashRequested)
        {
            m_DashRequested = false;
        }

        return false;
    }

    public void StartDashing()
    {
        if (m_DashRequested)
        {
            m_Animator.SetTrigger(m_HashDashPara);

            DashCurrentCooldownTime = dashCooldownTime;

            StartCoroutine(DashingCooldown());
        }
    }

    public void Dashing()
    {
        float desiredDirection = !spriteRenderer.flipX ? dashSpeed : -dashSpeed;
        m_MoveVector.x = Mathf.MoveTowards(m_MoveVector.x, desiredDirection, dashAcceleration * Time.deltaTime);
        m_MoveVector.y = 0f;
    }

    IEnumerator DashingCooldown()
    {
        while (DashCurrentCooldownTime > 0f)
        {
            DashCurrentCooldownTime -= Time.deltaTime;
            yield return null;
        }

        DashCurrentCooldownTime = 0f;
    }

    #endregion // DASHING

    #region HEALING

    void AddHealth()
    {
        if (healthPotionCounter > 0 && healthController.CurrentHealth < healthController.maxHealth)
        {
            healthController.GainHealth(healPotionValue);

            healthPotionCounter -= 1;
            OnHealthItemCounterChanged.Invoke(healthPotionCounter);

            Vector2 pos = new(transform.position.x, transform.position.y + 0.5f);
            VFXController.Instance.Trigger(m_PotionsHealthUse, pos, 0, false, null);
        }
    }

    public void AddHealthItem()
    {
        if (healthPotionCounter < maxHealthPotionInInventory)
        {
            healthPotionCounter += 1;
            OnHealthItemCounterChanged.Invoke(healthPotionCounter);
        }
    }

    #endregion // HEALTH AND AETHER

    #region AETHER

    void AddAether()
    {
        if (aetherPotionCounter > 0 && aetherController.CurrentAether < aetherController.maxAetherValue)
        {
            aetherController.GainAether(aetherPotionValue);

            aetherPotionCounter -= 1;
            OnAetherItemCounterChanged.Invoke(aetherPotionCounter);

            Vector2 pos = new(transform.position.x, transform.position.y + 0.5f);
            VFXController.Instance.Trigger(m_PotionsHealthUse, pos, 0, false, null);
        }
    }

    public void AddAetherItem()
    {
        if (aetherPotionCounter < maxAetherPotionInInventory)
        {
            aetherPotionCounter += 1;
            OnAetherItemCounterChanged.Invoke(aetherPotionCounter);
        }
    }

    #endregion // AETHER

    #region HURTING

    public void OnHurt(HealthDamager damager, HealthController healthController)
    {
        if (healthController.GetDamageDirection().x > 0f)
        {
            spriteRenderer.flipX = true;
            m_CurrentBulletSpawnPoint = facingLeftBulletSpawnPoint;
        }
        else
        {
            spriteRenderer.flipX = false;
            m_CurrentBulletSpawnPoint = facingRightBulletSpawnPoint;
        }

        healthController.EnableInvulnerability();
        m_Animator.SetTrigger(m_HashHurtPara);

        //we only force respawn if helath > 0, otherwise both forceRespawn & Death trigger are set in the animator, messing with each other.
        if (healthController.CurrentHealth > 0 && damager.forceRespawn)
            m_Animator.SetTrigger(m_HashForcedRespawnPara);

        m_Animator.SetBool(m_HashGroundedPara, false);
        hurtAudioPlayer.PlayRandomSound();

        //if the health is < 0, mean die callback will take care of respawn
        if (damager.forceRespawn && healthController.CurrentHealth > 0)
        {
            //StartCoroutine(DieRespawnCoroutine(false, true));
            Debug.LogWarning("Respawn need to be write");
        }

    }

    public void SetMoveVector(Vector2 newMoveVector)
    {
        m_MoveVector = newMoveVector;
    }

    public Vector2 GetHurtDirection()
    {
        Vector2 damageDirection = healthController.GetDamageDirection();

        if (damageDirection.y < 0f)
            return new Vector2(Mathf.Sign(damageDirection.x), 0f);

        float y = Mathf.Abs(damageDirection.x) * m_TanHurtJumpAngle;

        return new Vector2(damageDirection.x, y).normalized;
    }

    public void StartFlickering()
    {
        m_FlickerCoroutine = StartCoroutine(Flicker());
    }

    public void StopFlickering()
    {
        StopCoroutine(m_FlickerCoroutine);
        spriteRenderer.enabled = true;
    }

    protected IEnumerator Flicker()
    {
        float timer = 0f;

        while (timer < healthController.invulnerabilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return m_FlickeringWait;
            timer += flickeringDuration;
        }

        spriteRenderer.enabled = true;
    }

    #endregion // HURTING

    #region DEAD

    public void OnDie()
    {
        m_Animator.SetTrigger(m_HashDeadPara);

        //StartCoroutine(DieRespawnCoroutine(true, false));
    }

    /*IEnumerator DieRespawnCoroutine(bool resetHealth, bool useCheckPoint)
    {
        //PlayerInput.Instance.ReleaseControl(true);
        yield return new WaitForSeconds(1.0f); //wait one second before respawing
        //yield return StartCoroutine(ScreenFader.FadeSceneOut(useCheckPoint ? ScreenFader.FadeType.Black : ScreenFader.FadeType.GameOver));
        if (!useCheckPoint)
            yield return new WaitForSeconds(2f);
        Respawn(resetHealth, useCheckPoint);
        yield return new WaitForEndOfFrame();
        //yield return StartCoroutine(ScreenFader.FadeSceneIn());
        //PlayerInput.Instance.GainControl();
    }

    public void Respawn(bool resetHealth, bool useCheckpoint)
    {
        if (resetHealth)
            damageable.SetHealth(damageable.startingHealth);

        //we reset the hurt trigger, as we don't want the player to go back to hurt animation once respawned
        m_Animator.ResetTrigger(m_HashHurtPara);
        if (m_FlickerCoroutine != null)
        {//we stop flcikering for the same reason
            StopFlickering();
        }

        m_Animator.SetTrigger(m_HashRespawnPara);

        if (useCheckpoint && m_LastCheckpoint != null)
        {
            UpdateFacing(m_LastCheckpoint.respawnFacingLeft);
            GameObjectTeleporter.Teleport(gameObject, m_LastCheckpoint.transform.position);
        }
        else
        {
            UpdateFacing(m_StartingFacingLeft);
            GameObjectTeleporter.Teleport(gameObject, m_StartingPosition);
        }
    }
    */

    #endregion // DEAD

    #region FOCUSED

    public bool InputCheckForFocused()
    {
        bool isFocused = m_Animator.GetBool(m_HashFocusedPara);
        bool focused = PlayerInput.Instance.InputFocused != 0f;

        if (!focusedAudioPlayer.CheckIfPlaying())
        {
            focusedAudioPlayer.PlayRandomSound();
        }

        if (focused)
        {
            if (!isFocused)
                m_Animator.SetBool(m_HashFocusedPara, true);

            return true;
        }
        else
        {
            if (isFocused)
                m_Animator.SetBool(m_HashFocusedPara, false);

            return false;
        }
    }

    public void Focused()
    {
        var ps = particleFocused.main;
        var setActive = particleFocused.gameObject;
        if (!setActive.activeSelf)
        {
            setActive.SetActive(true);
            ps.loop = true;

            particleFocused.Play();
        }
        else
        {
            focusedAudioPlayer.MoveTowardsPitch(1.1f);
        }
    }

    public void StopFocused()
    {
        StartCoroutine(StopFocusedCoro());
        m_Animator.ResetTrigger(m_HashFocusedPara);
    }

    IEnumerator StopFocusedCoro()
    {
        var ps = particleFocused.main;
        ps.loop = false;

        while (particleFocused.particleCount > 0)
        {
            focusedAudioPlayer.MoveTowardsPitch(0.85f);

            yield return null;
        }

        particleFocused.gameObject.SetActive(false);
    }

    #endregion // FOCUSED

    #region ACTIONS LIGHT ATTACK

    public bool CheckForAttackLightInput()
    {
        if (!aetherController.enableLightAttack)
            return false;

        bool isAtk = m_Animator.GetBool(m_HashAtkLightPara);
        bool lightAtk = PlayerInput.Instance.InputAttackLight != 0f;

        bool hasAether = aetherController.CanShotCheckAether(aetherController.lightAttackCost);

        if (!hasAether || !CanShotCheckObstacle())
        {
            if (isAtk)
            {
                SetAttackLightPara(false);
            }

            return false;
        }

        if (lightAtk)
        {
            if (!isAtk)
                SetAttackLightPara(true);

            return true;
        }
        else
        {
            if (isAtk)
                SetAttackLightPara(false);

            return false;
        }
    }

    public void SetAttackLightPara(bool state)
    {
        m_Animator.SetBool(m_HashAtkLightPara, state);
    }

    public void LightAttack()
    {
        int lightAttackCost = aetherController.lightAttackCost;
        if (!aetherController.CanShotCheckAether(lightAttackCost))
        {
            return;
        }

        var ps = particleLightAttack.main;
        var setActive = particleLightAttack.gameObject;
        if (!setActive.activeSelf)
        {
            setActive.SetActive(true);
            ps.loop = true;

            if (particleChildLightAttack == null)
                particleChildLightAttack = particleLightAttack.transform.Find("LightAttackVortex").gameObject;

            ParticleSystem childPS = particleChildLightAttack.GetComponentInChildren<ParticleSystem>();
            var main = childPS.main;
            main.loop = true;

            particleLightAttack.Play();
        }

        Vector2 facingLeft = spriteRenderer.flipX ? facingLeftBulletSpawnPoint.position : facingRightBulletSpawnPoint.position;
        particleLightAttack.transform.position = facingLeft;

        if (aetherController.lightAttackStartInitTime <= 0f)
        {
            SpawnBullet(bulletPoolLightAttack, aetherController.lightAttackBulletSpeed);

            VFXController.Instance.Trigger(m_LightAttackShot, m_CurrentBulletSpawnPoint.position, 0, false, null);

            float bufferTime = 60f / aetherController.lightAttackCadence;
            aetherController.lightAttackStartInitTime = bufferTime;

            aetherController.HandleAetherBasePointReduction(aetherController.lightAttackCost);
        }
        else
        {
            aetherController.lightAttackStartInitTime -= Time.deltaTime;
        }
    }

    public void StopLightAttack()
    {
        StartCoroutine(StopLightAttackCoro());
    }

    IEnumerator StopLightAttackCoro()
    {
        GameObject gO = particleLightAttack.transform.Find("LightAttackVortex").gameObject;
        ParticleSystem childPS = gO.GetComponentInChildren<ParticleSystem>();
        var main = childPS.main;
        main.loop = false;

        var ps = particleLightAttack.main;
        ps.loop = false;

        while (particleLightAttack.particleCount > 0)
            yield return null;

        particleLightAttack.gameObject.SetActive(false);
        aetherController.lightAttackStartInitTime = 0.3f;
    }

    #endregion // ACTIONS LIGHT ATTACK

    #region ACTIONS HEAVY ATTACK

    public bool InputCheckForAttackHeavy()
    {
        if (!aetherController.enableHeavyAttack)
            return false;

        bool heavyAtk = PlayerInput.Instance.InputAttackHeavy != 0f;

        if (!aetherController.CanShotCheckAether(aetherController.heavyAttackCost))
        {
            return false;
        }

        if (heavyAtk)
        {
            return true;
        }
        else if (aetherController.m_AttackHeavyRequested)
        {
            aetherController.m_AttackHeavyRequested = false;
        }

        return false;
    }

    public void HeavyAttack()
    {
        if (!m_CharacterController2D.IsGrounded)
            return;

        if (!aetherController.m_AttackHeavyRequested)
        {
            StartCoroutine(HeavyAttackCoroutine());
            aetherController.m_AttackHeavyRequested = true;
        }
    }

    IEnumerator HeavyAttackCoroutine()
    {
        m_Animator.SetTrigger(m_HashAtkHeavyPara);
        heavyAttackAudioPlayer.PlayRandomSound();

        float timeBeforeShot = aetherController.heavyAttackTimeBeforeShot;
        yield return new WaitForSeconds(timeBeforeShot);

        float bulletSpeed = aetherController.heavyAttackBulletSpeed;
        SpawnBullet(bulletPoolHeavyAttack, bulletSpeed);
        VFXController.Instance.Trigger(m_HeavyAttackShot, m_CurrentBulletSpawnPoint.position, 0, false, null);

        aetherController.HandleAetherBasePointReduction(aetherController.heavyAttackCost);

        heavyAttackAudioPlayer.Stop();
    }

    #endregion // ACTIONS HEAVY ATTACK

    #region ACTIONS SUPER ATTACK

    public bool InputCheckForAttackSuper()
    {
        if (!aetherController.enableSuperAttack)
            return false;
        if (!CanShotCheckObstacle())
            return false;

        if (aetherController.CurrentAether < 1f)
            return false;

        bool superAtk = PlayerInput.Instance.InputAttackSuper != 0f;

        if (superAtk)
        {
            return true;
        }
        else if (m_AttackSuperRequested)
        {
            m_AttackSuperRequested = false;
        }

        return false;
    }
    
    public void SuperAttack()
    {
        if (!m_AttackSuperRequested)
        {
            StartCoroutine(SuperAttackCoroutine());
            SetVerticalMovement(4);

            m_AttackSuperRequested = true;
        }
    }

    IEnumerator SuperAttackCoroutine()
    {
        m_Animator.SetTrigger(m_HashAtkSuperPara);
        superAttackAudioPlayer.PlayRandomSound();

        float timeBeforeShot = aetherController.superAttackTimeBeforeShot;
        yield return new WaitForSeconds(timeBeforeShot);

        float bulletSpeed = aetherController.superAttackBulletSpeed;
        SpawnBullet(bulletPoolSuperAttack, bulletSpeed);
        aetherController.SetAether(0f);
        
        VFXController.Instance.Trigger(m_SuperAttackShot, m_CurrentBulletSpawnPoint.position, 0, false, null);
        superAttackAudioPlayer.Stop();
    }

    #endregion // ACTIONS SUPER ATTACK

    #region PUSHING
    public void CheckForPushing()
    {
        bool pushableOnCorrectSide = false;
        Pushable previousPushable = m_CurrentPushable;

        m_CurrentPushable = null;

        if (m_CurrentPushables.Count > 0)
        {
            bool movingRight = PlayerInput.Instance.InputHorizontal > float.Epsilon;
            bool movingLeft = PlayerInput.Instance.InputHorizontal < -float.Epsilon;

            for (int i = 0; i < m_CurrentPushables.Count; i++)
            {
                float pushablePosX = m_CurrentPushables[i].pushablePosition.position.x;
                float playerPosX = m_Transform.position.x;
                if (pushablePosX < playerPosX && movingLeft || pushablePosX > playerPosX && movingRight)
                {
                    pushableOnCorrectSide = true;
                    m_CurrentPushable = m_CurrentPushables[i];
                    break;
                }
            }

            if (pushableOnCorrectSide)
            {
                Vector2 moveToPosition = movingRight ? m_CurrentPushable.playerPushingRightPosition.position : m_CurrentPushable.playerPushingLeftPosition.position;
                moveToPosition.y = m_CharacterController2D.Rigidbody2D.position.y;
                m_CharacterController2D.Teleport(moveToPosition);
            }
        }

        if (previousPushable != null && m_CurrentPushable != previousPushable)
        {//we changed pushable (or don't have one anymore), stop the old one sound
            previousPushable.EndPushing();
        }

        m_Animator.SetBool(m_HashPushingPara, pushableOnCorrectSide);
    }

    public void MovePushable()
    {
        //we don't push ungrounded pushable, avoid pushing floating pushable or falling pushable.
        if (m_CurrentPushable && m_CurrentPushable.Grounded)
            m_CurrentPushable.Move(m_MoveVector * Time.deltaTime);
    }

    public void StartPushing()
    {
        if (m_CurrentPushable)
            m_CurrentPushable.StartPushing();
    }

    public void StopPushing()
    {
        if (m_CurrentPushable)
            m_CurrentPushable.EndPushing();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Pushable pushable = other.GetComponent<Pushable>();
        if (pushable != null)
        {
            m_CurrentPushables.Add(pushable);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Pushable pushable = other.GetComponent<Pushable>();
        if (pushable != null)
        {
            if (m_CurrentPushables.Contains(pushable))
                m_CurrentPushables.Remove(pushable);
        }
    }

    #endregion // PUSHING

    #region QUICKSLOTS

    public void InputQuickslots()
    {
        var playerInput = PlayerInput.Instance;

        if (playerInput.InputQuickselectA != 0f && !m_QuickslotAPressed)
        {
            QuickslotA();
            m_QuickslotAPressed = true;
        }
        else if (playerInput.InputQuickselectA == 0f && m_QuickslotAPressed)
        {
            m_QuickslotAPressed = false;
        }

        else if (playerInput.InputQuickselectB != 0f && !m_QuickslotBPressed)
        {
            QuickslotB();
            m_QuickslotBPressed = true;
        }
        else if (playerInput.InputQuickselectB == 0f && m_QuickslotBPressed)
        {
            m_QuickslotBPressed = false;
        }

        if (playerInput.InputQuickselectC != 0f && !m_QuickslotCPressed)
        {
            QuickslotC();
            m_QuickslotCPressed = true;
        }
        else if (playerInput.InputQuickselectC == 0f && m_QuickslotCPressed)
        {
            m_QuickslotCPressed = false;
        }

        if (playerInput.InputQuickselectD != 0f && !m_QuickslotDPressed)
        {
            QuickslotD();
            m_QuickslotDPressed = true;
        }
        else if (playerInput.InputQuickselectD == 0f && m_QuickslotDPressed)
        {
            m_QuickslotDPressed = false;
        }
    }

    void QuickslotA()
    {
        AddHealth();
    }

    void QuickslotB()
    {
        Debug.LogWarning("Function is not implemented");
    }

    void QuickslotC()
    {
        Debug.LogWarning("Function is not implemented");
    }

    void QuickslotD()
    {
        AddAether();
    }

    #endregion // QUICKSLOTS

    #region GENERAL

    public bool IsFalling()
    {
        return m_MoveVector.y < 0f && !m_Animator.GetBool(m_HashGroundedPara);
    }

    public bool CheckForGrounded()
    {
        bool wasGrounded = m_Animator.GetBool(m_HashGroundedPara);
        bool grounded = m_CharacterController2D.IsGrounded;

        if (grounded)
        {
            FindCurrentSurface();

            if (!wasGrounded && m_MoveVector.y < -1.0f)
            {//only play the landing sound if falling "fast" enough (avoid small bump playing the landing sound)
                PlayLanding();
            }
        }
        else
        {
            m_CurrentSurface = null;
        }

        m_Animator.SetBool(m_HashGroundedPara, grounded);

        return grounded;
    }

    public void FindCurrentSurface()
    {
        Collider2D groundCollider = m_CharacterController2D.GroundColliders[0];

        if (groundCollider == null)
            groundCollider = m_CharacterController2D.GroundColliders[1];

        if (groundCollider == null)
            return;

        TileBase b = PhysicsHelper.FindTileForOverride(groundCollider, transform.position, Vector2.down);
        if (b != null)
        {
            m_CurrentSurface = b;
        }
    }

    public void UpdateViewDirection()
    {
        float inputHorizontal = PlayerInput.Instance.InputHorizontal;

        if (inputHorizontal != 0f)
        {
            bool isLeft = inputHorizontal < 0f;

            if (isLeft)
            {
                spriteRenderer.flipX = isLeft;
            }
            else
            {
                spriteRenderer.flipX = isLeft;
            }
        }
    }

    void SpawnBullet(BulletPool atkType, float bulletSpeed)
    {
        BulletObject bullet = atkType.Pop(m_CurrentBulletSpawnPoint.position);
        bool facingLeft = m_CurrentBulletSpawnPoint == facingLeftBulletSpawnPoint;
        bullet.rigidbody2D.velocity = new Vector2(facingLeft ? -bulletSpeed : bulletSpeed, 0f);
        bullet.spriteRenderer.flipX = facingLeft ^ bullet.bullet.spriteOriginallyFacesLeft;
    }

    bool CanShotCheckObstacle()
    {
        if (spriteRenderer.flipX)
        {
            m_CurrentBulletSpawnPoint = facingLeftBulletSpawnPoint;
        }
        else
        {
            m_CurrentBulletSpawnPoint = facingRightBulletSpawnPoint;
        }

        //we check if there is a wall between the player and the bullet spawn position, if there is, we don't spawn a bullet
        //otherwise, the player can "shoot throught wall" because the arm extend to the other side of the wall
        Vector2 testPosition = transform.position;
        testPosition.y = m_CurrentBulletSpawnPoint.position.y;
        Vector2 direction = (Vector2)m_CurrentBulletSpawnPoint.position - testPosition;
        float distance = direction.magnitude;
        direction.Normalize();

        RaycastHit2D[] results = new RaycastHit2D[12];
        if (Physics2D.Raycast(testPosition, direction, m_CharacterController2D.ContactFilter, results, distance) > 0)
        {
            return false;
        }     

        return true;
    }

    #endregion // GENERAL
        
    void FixedUpdate()
    {
        m_CharacterController2D.Move(m_MoveVector * Time.deltaTime);

        var pInputInstance = PlayerInput.Instance;
        m_Animator.SetFloat(m_HashHorizontalSpeedPara, pInputInstance.InputHorizontal);
        m_Animator.SetFloat(m_HashVerticalSpeedPara, m_MoveVector.y);
    }
}
