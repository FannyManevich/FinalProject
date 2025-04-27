// Ignore Spelling: Dialogue Npc

using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputReader : MonoBehaviour, Input.IPlayerActions, Input.IUIActions
{
    public event Action EndNpcDialogue;

    public InputChannel inputChannel;
    private Input inputActions;
    
    private void Awake()
    {
        inputActions = new Input();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Interact.performed += OnInteract;
        inputActions.Player.OpenBook.performed += OnOpenBook;

        inputActions.UI.LeftClick.performed += OnLeftClick;
        inputActions.UI.Cancel.performed += OnCancel;

        inputActions.Enable();
    }

    private void OnDestroy()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;

        inputActions.Player.Interact.performed -= OnInteract;
        inputActions.Player.OpenBook.performed -= OnOpenBook;

        inputActions.UI.LeftClick.performed -= OnLeftClick;
        inputActions.UI.Cancel.performed -= OnCancel;

        inputActions.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        inputChannel.HandleMove(context);

        Debug.Log("InputReader: Movement " + direction);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        inputChannel.RaiseInteract();
        Debug.Log("InputReader: Interact pressed (E)");
    }

    public void OnOpenBook(InputAction.CallbackContext context)
    {
        inputChannel.RaiseInteract();
        Debug.Log("InputReader: Interact pressed (Q)");
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Left click detected!");
        }
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            EndNpcDialogue?.Invoke();
            Debug.Log("cancel detected!");
        }
    }
}
