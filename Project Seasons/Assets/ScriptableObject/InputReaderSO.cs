using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "TLS Scriptable Objects/InputReader")]
public class InputReaderSO : ScriptableObject, InputActions.IPlayerActions, InputActions.IUIActions
{
    InputActions _gameInput;

    #region GAMEPLAY INPUT PROPERTIES
    public event UnityAction<float> Horizontal;
        
    public event UnityAction<float> Jump;
    public event UnityAction<float> Dash;

    public event UnityAction<float> AttackLight;
    public event UnityAction<float> Focused;
    public event UnityAction<float> AttackHeavy;
    public event UnityAction<float> AttackSuper;

    public event UnityAction<float> QuickselectA;
    public event UnityAction<float> QuickselectB;
    public event UnityAction<float> QuickselectC;
    public event UnityAction<float> QuickselectD;

    #endregion // GAMEPLAY INPUT PROPERTIES

    #region USER INTERFACE INPUT PROPERTIES

    public event UnityAction<Vector2> UINavigate;

    public event UnityAction UISubmit;
    public event UnityAction UICancel;
    
    public event UnityAction<Vector2> UIOnPoint;

    #endregion // USER INTERFACE INPUT PROPERTIES

    #region On Enable and Disable

    void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new InputActions();
        }

        _gameInput.Player.SetCallbacks(instance:this);
        _gameInput.UI.SetCallbacks(instance:this);
    }
    
    void OnDisable()
    {
        _gameInput.Player.Disable();
        _gameInput.UI.Disable();
    }

    #endregion

    #region Player Actions
    
    public void EnablePlayerActions()
    {
        _gameInput.Player.Enable();
    }
    
    public void DisablePlayerActions()
    {
        _gameInput.Player.Disable();
    }

    public void OnHorizontal(InputAction.CallbackContext context)
    {
        Horizontal?.Invoke(context.ReadValue<float>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Jump?.Invoke(context.ReadValue<float>());
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        Dash?.Invoke(context.ReadValue<float>());
    }
    
    public void OnAttackLight(InputAction.CallbackContext context)
    {
        AttackLight?.Invoke(context.ReadValue<float>());
    }

    public void OnFocused(InputAction.CallbackContext context)
    {
        Focused?.Invoke(context.ReadValue<float>());
    }

    public void OnAttackHeavy(InputAction.CallbackContext context)
    {
        AttackHeavy?.Invoke(context.ReadValue<float>());
    }

    public void OnAttackSuper(InputAction.CallbackContext context)
    {
        AttackSuper?.Invoke(context.ReadValue<float>());
    }

    public void OnSlotSouth(InputAction.CallbackContext context)
    {
        QuickselectA?.Invoke(context.ReadValue<float>());
    }

    public void OnSlotWest(InputAction.CallbackContext context)
    {
        QuickselectB?.Invoke(context.ReadValue<float>());
    }

    public void OnSlotNorth(InputAction.CallbackContext context)
    {
        QuickselectC?.Invoke(context.ReadValue<float>());
    }

    public void OnSlotEast(InputAction.CallbackContext context)
    {
        QuickselectD?.Invoke(context.ReadValue<float>());
    }

    #endregion

    #region UI Actions

    public void EnableUIActions()
    {
        _gameInput.UI.Enable();
    }
    public void DisableUIActions()
    {
        _gameInput.UI.Disable();
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        UINavigate?.Invoke(context.ReadValue<Vector2>());
    }
    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            UISubmit?.Invoke();
        }
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            UICancel?.Invoke();
        }
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        UIOnPoint?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("OnClick)");
        }
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
        Debug.Log("OnScrollWheel)");
    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("OnMiddleClick)");
        }
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("OnRightClick)");
        }
    }

    public void OnTrackedDevicePosition(InputAction.CallbackContext context)
    {
        Debug.Log("OnTrackedDevicePosition)");
    }

    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
    {
        Debug.Log("OnTrackedDeviceOrientation)");
    }

    public void OnNextTab(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("OnNextTab)");
        }
    }

    public void OnPreviousTab(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("OnPreviousTab)");
        }
    }

    #endregion
}
