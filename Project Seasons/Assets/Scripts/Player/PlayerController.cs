using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerStates { STAND, WALK, RUN, JUMP, STUN, OTHER }
    public PlayerStates PlayerState { get; private set; }

    void SetPlayerState()
    {

    }


    [SerializeField, Tooltip("ScriptableObject for reading player input")]
    private InputReaderSO inputReader;

    #region GROUND SETTINGS

    [Header("GROUND SETTINGS")]
    
    [SerializeField, Tooltip("Offset for the ground check position relative to the player's position")]
    Vector3 grdCheckOffset;
    
    [SerializeField, Tooltip("Radius of the circle used for ground detection")]
    Vector2 grdCheckRadius;

    [SerializeField, Tooltip("Layer mask for detecting the ground")]
    LayerMask groundMask;

    #endregion // GROUND SETTINGS

    #region PLAYER MOVEMENT SETTINGS
    
    [Header("PLAYER MOVEMENT SETTINGS")]
    
    [SerializeField, Tooltip("Base movement speed of the player")]
    float moveBaseSpeed = 8.0f;
    
    [SerializeField, Tooltip("Running speed of the player")]
    float moveRunSpeed = 16.0f;
    
    [SerializeField, Tooltip("Multiplier for adjusting the speed change rate")]
    [Range(0.01f, 100.0f)]
    
    float moveSpeedMultiplier = 4.0f;

    #endregion // PLAYER MOVEMENT SETTINGS

    #region PLAYER JUMP SETTINGS

    [Header("PLAYER JUMP SETTINGS")]
    
    [SerializeField, Tooltip("Force applied to make the player jump")]
    float jumpForce = 300.0f;
    
    [SerializeField, Tooltip("Force mode for applying jump force")]
    ForceMode2D jumpForceMode;

    #endregion // PLAYER MOVEMENT SETTINGS

    SpriteRenderer playerSpriteRenderer;

    #region GETTER AND SETTER METHODS

    public Rigidbody2D PlayerRb { get; private set; }
    public bool IsGrounded { get; private set; }
    public float InputMoveValue { get; private set; }
    public bool IsMoving { get; private set; }
    private bool _moveRequest;
    private float currentSpeed;
    public bool ExeSprint { get; private set; }
    private bool _sprintRequest;
    public bool ExeJump { get; private set; }
    private bool _jumpRequest;
    public bool ExeAttack { get; private set; }

    #endregion // GETTER AND SETTER METHODS

    private void OnEnable()
    {
        EnablePlayerInput();

        if (playerSpriteRenderer == null)
            playerSpriteRenderer = transform.Find("SpriteHolder").GetComponent<SpriteRenderer>();

        if (PlayerRb == null)
            PlayerRb = GetComponent<Rigidbody2D>();

        currentSpeed = moveBaseSpeed;
    }

    private void OnDisable()
    {
        DisablePlayerInput();
    }

    #region INPUT SYSTEM - ACTIONS
    public void EnablePlayerInput()
    {
#if UNITY_EDITOR
        if (inputReader == null)
        {
            Debug.LogError("PlayerController: inputReader is NULL, game will be stopped");
            UnityEditor.EditorApplication.isPlaying = false;
            return;
        }
#endif

        inputReader.EnablePlayerActions();

        inputReader.Movement += ReadMovementInput;
        inputReader.Sprint += ReadSprintInput;
        inputReader.Jump += ReadJumpInput;
    }
    public void DisablePlayerInput()
    {
        inputReader.DisablePlayerActions();

        inputReader.Movement -= ReadMovementInput;
        inputReader.Sprint -= ReadSprintInput;
        inputReader.Jump -= ReadJumpInput;
    }
    #endregion // INPUT SYSTEM - ACTIONS

    #region INPUT SYSTEM - READ
    private void ReadMovementInput(float moveDirection)
    {
        InputMoveValue = moveDirection;
        if (InputMoveValue != 0f)
        {
            _moveRequest = true;
        }
        else
        {
            _moveRequest = false;

            ExeSprint = false;
        }
    }
    private void ReadSprintInput(float sprintValue)
    {
        if (sprintValue != 0f)
        {
            _sprintRequest = true;
        }
        else
        {
            _sprintRequest = false;
        }
    }
    private void ReadJumpInput()
    {
        if (!IsGrounded) return;

        _jumpRequest = true;
    }
    #endregion // INPUT SYSTEM - READ

    #region Handle Behavior Methods
    public void HandleMovementSpeed()
    {
        if (_moveRequest)
        {
            if (_sprintRequest)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, moveRunSpeed, moveSpeedMultiplier * Time.deltaTime);
                ExeSprint = true;
            }
            else if (currentSpeed != moveBaseSpeed)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, moveBaseSpeed, moveSpeedMultiplier * Time.deltaTime);
                ExeSprint = false;
            }

            IsMoving = true;
        }
        else
        {
            currentSpeed = moveBaseSpeed;

            ExeSprint = false;
            IsMoving = false;
        }
    }
    public void SetSpriteDirection()
    {
        if (InputMoveValue < 0f)
            playerSpriteRenderer.flipX = true;
        else if (InputMoveValue > 0f)
            playerSpriteRenderer.flipX = false;
    }
    #endregion

    #region Movement Execution Methods
    public void GroundCheck()
    {
        Vector2 groundCheckPosition = transform.position + new Vector3(grdCheckOffset.x, grdCheckOffset.y);
        IsGrounded = Physics2D.OverlapBox(groundCheckPosition, grdCheckRadius, 0, groundMask);
    }

    public void ExecuteMovement()
    {
        if (IsMoving)
        {
            Vector2 mSpeed = new(InputMoveValue * currentSpeed, PlayerRb.velocity.y);
            PlayerRb.velocity = mSpeed;
            //PlayerRb.AddForce(mSpeed, ForceMode2D.Impulse);
        }
        if (!IsGrounded) return;
        else if (IsGrounded && ExeJump)
        {
            ExeJump = false;
        }
        if (_jumpRequest)
        {
            float jDir = InputMoveValue * jumpForce;
            Vector2 jForce = new(jDir / 6f, jumpForce);

            PlayerRb.velocity = new Vector2(PlayerRb.velocity.x, 0f);
            PlayerRb.AddForce(jForce, jumpForceMode);

            _jumpRequest = false;
            ExeJump = true;
        }
    }
    #endregion

#if UNITY_EDITOR
    [Header("TLS Control - only UNITY_EDITOR")]
    [SerializeField, Tooltip("Toggle to show the position of the ground check in the scene view")]
    private bool showGroundCheckPosition = false;
    [SerializeField, Tooltip("Color of the marker for ground check position")]
    private Color groundCheckMarkColor = Color.yellow;
    void OnDrawGizmos()
    {
        if (showGroundCheckPosition)
        {
            Gizmos.color = groundCheckMarkColor;
            Vector2 groundCheckPosition = transform.position + new Vector3(grdCheckOffset.x, grdCheckOffset.y);
            Gizmos.DrawCube(groundCheckPosition, grdCheckRadius);
        }
    }
#endif
}
