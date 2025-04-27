using UnityEngine;
using System;

public class DialogueManager : MonoBehaviour
{
    public event Action OnPlayerDialogue;
    public event Action<PlantSO> OnNpcDialogue;
    public event Action EndPdialogue;
    public event Action EndNpcDialogue;

    public void StartPlayerDialogue()
    {
        Debug.Log("Player is talking.");
        OnPlayerDialogue?.Invoke();
    }

    public void EndPlayerDialogue()
    {
        EndPdialogue?.Invoke();
    }
    public void StartNpcDialogue(PlantSO plantRequest)
    {       
        Debug.Log("In DialogueManager: Starting NPC dialogue.  PlantSO: " + plantRequest);
        OnNpcDialogue?.Invoke(plantRequest);
    }   

    public void EndnpcDialogue()
    {
        EndNpcDialogue?.Invoke();
    }
}