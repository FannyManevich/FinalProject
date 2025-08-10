using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Input Channel", menuName = "Channels/Input Channel", order = 1)]
public class InputChannel : ScriptableObject
{
    public event Action<Vector2> OnMoveEvent;
    public event Action OnInteractEvent;

    public event Action OnBookEvent;
    public event Action OnHelpEvent;
    public event Action OnCancelEvent;
    public event Action OnLeftClickEvent;
    public void HandleMove(InputAction.CallbackContext context)
    {
        //Debug.Log($"OnMove {context.phase} Value {context.ReadValue<Vector2>()} {context.control.device}");
        OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
    }
    public void RaiseInteract()
    {
        //Debug.Log("InputChannel : OnInteractEvent invoked.");
        OnInteractEvent?.Invoke();
    }
    public void RaiseBook()
    {
        //Debug.Log("InputChannel : OnBookEvent invoked.");
        OnBookEvent?.Invoke();
    }
    public void RaiseHelp()
    {
        //Debug.Log("InputChannel : OnHelpEvent invoked.");
        OnHelpEvent?.Invoke();
    }
    public void RaiseCancel()
    {
        //Debug.Log("InputChannel : OnCancelEvent invoked.");
        OnCancelEvent?.Invoke();
    }
    public void RaiseLeftClick()
    {
        //Debug.Log("InputChannel : OnLeftClickEvent invoked.");
        OnLeftClickEvent?.Invoke();
    }
}