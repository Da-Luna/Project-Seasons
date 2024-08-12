using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    protected static PlayerInput s_Instance;

    public static PlayerInput Instance { get { return s_Instance; } }

    [Header("Base Settings")]
    [Space]

    [SerializeField, Tooltip("ScriptableObject for reading player input")]
    InputReaderSO inputReader;

    #region ENABLING PROPERTIES

    [Tooltip("Enables the entire control in the OnEnable method")]
    public bool enableInputByStart = false;

    public bool enableMovement = false;
    public bool enableJump = false;
    public bool enableDash = false;
    public bool enableInteract = false;

    public bool enableFocus = false;
    public bool enableLightAttack = false;
    public bool enableHeavyAttack = false;
    public bool enableSuperAttack = false;

    public bool enableSlotA = false;
    public bool enableSlotB = false;
    public bool enableSlotC = false;
    public bool enableSlotD = false;

    #endregion // ENABLING PROPERTIES

    #region INPUT PROPERTIES - GETTER AND SETTER

    public float InputHorizontal { get; protected set; }
    public float InputJump { get; protected set; }
    public float InputDash { get; protected set; }
    public float InputInteract { get; protected set; }

    public float InputFocused { get; protected set; }
    public float InputAttackLight { get; protected set; }
    public float InputAttackHeavy { get; protected set; }
    public float InputAttackSuper { get; protected set; }

    public float InputQuickselectA { get; protected set; }
    public float InputQuickselectB { get; protected set; }
    public float InputQuickselectC { get; protected set; }
    public float InputQuickselectD { get; protected set; }

    #endregion // INPUT PROPERTIES - GETTER AND SETTER

    void Awake()
    {
        if (s_Instance == null)
            s_Instance = this;
        else
            throw new UnityException("There cannot be more than one PlayerInput script.  The instances are " + s_Instance.name + " and " + name + ".");
    }

    void OnEnable()
    {
        InitPlayerInput();

        if (enableInputByStart)
        {
            EnablePlayerInput();
#if UNITY_EDITOR
            Debug.Log   ("class PlayerInput enable the Input");
#endif
        }
    }

    void OnDisable()
    {
        DisablePlayerInput();
        s_Instance = null;
    }

    #region INPUT SYSTEM - ACTIONS

    void InitPlayerInput()
    {
#if UNITY_EDITOR
        if (inputReader == null)
        {
            Debug.LogError("PlayerController: inputReader is NULL, game will be stopped");
            UnityEditor.EditorApplication.isPlaying = false;
            return;
        }
#endif
    }

    public void EnablePlayerInput()
    {
        inputReader.EnablePlayerActions();

        inputReader.Horizontal += ReadHorizontalInput;
        enableMovement = true;
        inputReader.Jump += ReadJumpInput;
        enableJump = true;
        inputReader.Dash += ReadDashInput;
        enableDash = true;
        inputReader.Interact += ReadInteractInput;
        enableInteract = true;

        inputReader.Focused += ReadFocusedInput;
        enableFocus = true;
        inputReader.AttackLight += ReadAttackLightInput;
        enableLightAttack = true;
        inputReader.AttackHeavy += ReadAttackHeavyInput;
        enableHeavyAttack = true;
        inputReader.AttackSuper += ReadAttackSuperInput;
        enableSuperAttack = true;

        inputReader.QuickselectA += ReadQuickselectAInput;
        enableSlotA = true;
        inputReader.QuickselectB += ReadQuickselectBInput;
        enableSlotB = true;
        inputReader.QuickselectC += ReadQuickselectCInput;
        enableSlotC = true;
        inputReader.QuickselectD += ReadQuickselectDInput;
        enableSlotD = true;
    }

    public void DisablePlayerInput()
    {
        inputReader.DisablePlayerActions();

        inputReader.Horizontal -= ReadHorizontalInput;
        inputReader.Jump -= ReadJumpInput;
        inputReader.Dash -= ReadDashInput;
        inputReader.Interact -= ReadInteractInput;

        inputReader.AttackLight -= ReadAttackLightInput;
        inputReader.Focused -= ReadFocusedInput;
        inputReader.AttackHeavy -= ReadAttackHeavyInput;
        inputReader.AttackSuper -= ReadAttackSuperInput;

        inputReader.QuickselectA -= ReadQuickselectAInput;
        inputReader.QuickselectB -= ReadQuickselectBInput;
        inputReader.QuickselectC -= ReadQuickselectCInput;
        inputReader.QuickselectD -= ReadQuickselectDInput;
    }

    #endregion // INPUT SYSTEM - ACTIONS

    #region INPUT SYSTEM - READ

    void ReadHorizontalInput(float moveDirection)
    {
        if (!enableMovement)
            return;
        
        InputHorizontal = moveDirection;
    }

    void ReadJumpInput(float jumpValue)
    {
        if (!enableJump)
            return;

        InputJump = jumpValue;
    }

    void ReadDashInput(float dashValue)
    {
        if (!enableDash)
            return;

        InputDash = dashValue;
    }

    void ReadInteractInput(float interactValue)
    {
        if (!enableInteract)
            return;

        InputInteract = interactValue;
    }

    void ReadAttackLightInput(float attackValue)
    {
        if (!enableLightAttack)
            return;

        InputAttackLight = attackValue;
    }

    void ReadFocusedInput(float attackValue)
    {
        if (!enableFocus)
            return;

        InputFocused = attackValue;
    }

    void ReadAttackHeavyInput(float attackValue)
    {
        if (!enableHeavyAttack)
            return;

        InputAttackHeavy = attackValue;
    }

    void ReadAttackSuperInput(float attackValue)
    {
        if (!enableSuperAttack)
            return;

        InputAttackSuper = attackValue;
    }

    void ReadQuickselectAInput(float quickA)
    {
        if (!enableSlotA)
            return;

        InputQuickselectA = quickA;
    }

    void ReadQuickselectBInput(float quickB)
    {
        if (!enableSlotB)
            return;

        InputQuickselectB = quickB;
    }

    void ReadQuickselectCInput(float quickC)
    {
        if (!enableSlotC)
            return;

        InputQuickselectC = quickC;
    }

    void ReadQuickselectDInput(float quickD)
    {
        if (!enableSlotD)
            return;

        InputQuickselectD = quickD;
    }

    #endregion // INPUT SYSTEM - READ
}
