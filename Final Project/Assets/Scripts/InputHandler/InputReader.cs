// Ignore Spelling: Dialogue Npc
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputReader : MonoBehaviour, Input.IPlayerActions, Input.IUIActions
{
    public event Action EndNpcDialogue;

    [SerializeField] BeaconSO beacon;
    public Input inputActions;
    public void EnablePlayerInput() => inputActions.Player.Enable();
    public void EnableUIInput() => inputActions.UI.Enable();
    public void DisableUIInput() => inputActions.UI.Disable();

    private void Awake()
    {
        inputActions = new Input();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Interact.performed += OnInteract;

        inputActions.UI.Book.performed += OnBook;
        inputActions.UI.Help.performed += OnHelp;
        inputActions.UI.Cancel.performed += OnCancel;
        inputActions.UI.Click.performed += OnClick;

        inputActions.Enable();
    }
    private void OnDestroy()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Interact.performed -= OnInteract;

        inputActions.UI.Book.performed -= OnBook;
        inputActions.UI.Help.performed -= OnHelp;
        inputActions.UI.Cancel.performed -= OnCancel;
        inputActions.UI.Click.performed -= OnClick;

        inputActions.Disable();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        beacon.inputChannel.HandleMove(context);

        //Debug.Log("InputReader: Movement " + direction);
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        beacon.inputChannel.RaiseInteract();
        //Debug.Log("InputReader: Interact pressed (E)");
    }
    public void OnBook(InputAction.CallbackContext context)
    {
        beacon.inputChannel.RaiseBook();
        Debug.Log("InputReader: Book pressed (B)");
    }
    public void OnHelp(InputAction.CallbackContext context)
    {
        beacon.inputChannel.RaiseHelp();
        Debug.Log("InputReader: Help pressed (H)");
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            EndNpcDialogue?.Invoke();
            beacon.inputChannel.RaiseCancel();
            Debug.Log("InputReader: Cancel detected!");
        }
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            beacon.inputChannel.RaiseLeftClick();
            // Debug.Log("Left click detected!");
        }
    }
    public void OnPoint(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }
}