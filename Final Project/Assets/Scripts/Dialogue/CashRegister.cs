using UnityEngine;

public class CashRegister : MonoBehaviour
{
    private bool playerInZone = false;
    private bool npcInZone = false;
    public DialogueManager dialogueManager;
    public NPC_SO[] NPCToTalkTo;

    void Start()
    {
        if (dialogueManager == null)
            dialogueManager = FindObjectOfType<DialogueManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInZone = true;

        if (other.CompareTag("NPC"))
            npcInZone = true;

       // if (playerInZone && npcInZone)
       //     DialogueManager.Instance.StartDialogue(NPCToTalkTo);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInZone = false;

        if (other.CompareTag("NPC"))
            npcInZone = false;
    }
}
