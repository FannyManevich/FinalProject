// Ignore Spelling: Dialogue

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public Image portraitImage;
    public TextMeshProUGUI nameText;

    private void OnEnable()
    {
        DialogueManager.Instance.OnDialogueStart += DisplayDialogue;
    }

    private void OnDisable()
    {
        DialogueManager.Instance.OnDialogueStart -= DisplayDialogue;
    }

    public void DisplayDialogue(PlayerSO player)
    {
        portraitImage.sprite = player.playerPortrait;
        nameText.text = player.playerName;
    }
}
