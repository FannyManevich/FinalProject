using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Input Channel")]
public class InputChannel : ScriptableObject
{
    public event Action<Vector2> OnMoveEvent;
    public void HandleMove(InputAction.CallbackContext context)
    {
        Debug.Log($"OnMove {context.phase} Value {context.ReadValue<Vector2>()} {context.control.device}");
        OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
    }
}