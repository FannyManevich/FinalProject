using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Input Channel", menuName = "Channels/Input Channel", order = 1)]
public class InputChannel : ScriptableObject
{
    public event Action<Vector2> OnMoveEvent;
    public event Action OnInteractEvent;
    public void HandleMove(InputAction.CallbackContext context)
    {
        Debug.Log($"OnMove {context.phase} Value {context.ReadValue<Vector2>()} {context.control.device}");
        OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void RaiseInteract()
    {
        OnInteractEvent?.Invoke();
    }
}