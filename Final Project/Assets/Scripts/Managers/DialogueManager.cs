using UnityEngine;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public event Action<PlayerSO> OnDialogueStart;
    public PlayerSO[] playerSO;

    void Start()
    {
       // dialogueUI.DisplayDialogue(playerSO[0]);
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartDialogue(PlayerSO player)
    {
        Debug.Log("Starting dialogue with: " + player.playerName);
        OnDialogueStart?.Invoke(player);
    }
}
