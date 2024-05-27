using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField, Tooltip("ScriptableObject for reading player input")]
    InputReaderSO inputReader;

    public static PlayerInput Instance
    {
        get { return s_Instance; }
    }

    protected static PlayerInput s_Instance;

    public float InputHorizontal { get; protected set; }
    public bool InputSprint { get; protected set; }
    public float Jump { get; protected set; }
    public float Dash { get; protected set; }
    public float AttackLight { get; protected set; }
    public float Focused { get; protected set; }
    public float AttackHeavy { get; protected set; }
    public float AttackSuper { get; protected set; }

    public float InputQuickselectA { get; protected set; }
    public float InputQuickselectB { get; protected set; }
    public float InputQuickselectC { get; protected set; }
    public float InputQuickselectD { get; protected set; }

    private void Awake()
    {
        if (s_Instance == null)
            s_Instance = this;
        else
            throw new UnityException("There cannot be more than one PlayerInput script.  The instances are " + s_Instance.name + " and " + name + ".");
    }

    void OnEnable()
    {   
        EnablePlayerInput();
    }

    void OnDisable()
    {
        DisablePlayerInput();

        s_Instance = null;
    }

    #region INPUT SYSTEM - ACTIONS

    void EnablePlayerInput()
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

        inputReader.Horizontal += ReadHorizontalInput;
        inputReader.Jump += ReadJumpInput;
        inputReader.Dash += ReadDashInput;
        inputReader.AttackLight += ReadAttackLightInput;
        inputReader.Focused += ReadFocusedInput;
        inputReader.AttackHeavy += ReadAttackHeavyInput;
        inputReader.AttackSuper += ReadAttackSuperInput;

        inputReader.QuickselectA += ReadQuickselectAInput;
        inputReader.QuickselectB += ReadQuickselectBInput;
        inputReader.QuickselectC += ReadQuickselectCInput;
        inputReader.QuickselectD += ReadQuickselectDInput;
    }

    void DisablePlayerInput()
    {
        inputReader.DisablePlayerActions();

        inputReader.Horizontal -= ReadHorizontalInput;
        inputReader.Jump -= ReadJumpInput;
        inputReader.Dash -= ReadDashInput;
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
        InputHorizontal = moveDirection;
    }

    void ReadJumpInput(float jumpValue)
    {
        Jump = jumpValue;
    }

    void ReadDashInput(float dashValue)
    {
        Dash = dashValue;
    }

    void ReadAttackLightInput(float attackValue)
    {
        AttackLight = attackValue;
    }

    void ReadFocusedInput(float attackValue)
    {
        Focused = attackValue;
    }

    void ReadAttackHeavyInput(float attackValue)
    {
        AttackHeavy = attackValue;
    }

    void ReadAttackSuperInput(float attackValue)
    {
        AttackSuper = attackValue;
    }

    void ReadQuickselectAInput(float quickA)
    {
        InputQuickselectA = quickA;
    }

    void ReadQuickselectBInput(float quickB)
    {
        InputQuickselectB = quickB;
    }

    void ReadQuickselectCInput(float quickC)
    {
        InputQuickselectC = quickC;
    }

    void ReadQuickselectDInput(float quickD)
    {
        InputQuickselectD = quickD;
    }

    #endregion // INPUT SYSTEM - READ

    public virtual void HorizontalInput()
    {

    }
}
