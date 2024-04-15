using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "TLS Scriptable Objects/InputReader")]
public class InputReaderSO : ScriptableObject, InputActions.IPlayerActions, InputActions.IUIActions
{
    private InputActions _gameInput;

    public event UnityAction<float> Movement;
    public event UnityAction<float> Sprint;
    public event UnityAction Jump;
    public event UnityAction Attack;

    public event UnityAction<Vector2> UINavigate;
    public event UnityAction UISubmit;
    public event UnityAction UICancel;
    public event UnityAction<Vector2> UIOnPoint;
    //public event UnityAction UIOnClick;

    #region On Enable and Disable
    private void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new InputActions();
        }

        _gameInput.Player.SetCallbacks(instance:this);
        _gameInput.UI.SetCallbacks(instance:this);
    }
    private void OnDisable()
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

    public void OnMovement(InputAction.CallbackContext context)
    {
        Movement?.Invoke(context.ReadValue<float>());
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        Sprint?.Invoke(context.ReadValue<float>());
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Jump?.Invoke();
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Attack?.Invoke();
        }
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
