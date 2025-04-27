using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject generalPanel;
    [SerializeField] GameObject selectPlayerPanel;

    public TextMeshProUGUI panelTitleText;
    public TextMeshProUGUI panelBodyText;
    
    [SerializeField] Button settingsButton;
    [SerializeField] Button creditsButton;
    [SerializeField] Button helpButton;
    [SerializeField] Button quitButton;
    [SerializeField] Button cancelButton;

    public GameObject malePlayerPrefab;
    public GameObject femalePlayerPrefab;
    public PlayerSO itanSO;
    public PlayerSO shiraSO;

    public void OnPlayClicked()
    {
        Debug.Log("Play Button clicked");
        selectPlayerPanel.SetActive(true);
    }

    public void OnMaleSelected()
    {
        Debug.Log("Male button clicked");
        Time.timeScale = 1f;
        PlayerSelector.selectedPlayer = malePlayerPrefab;
        PlayerSelector.SelectPlayer(malePlayerPrefab, itanSO);
        GameStateManager.Instance.TransitionToScene("Store");
    }

    public void OnFemaleSelected()
    {
        Debug.Log("Female button clicked");
        Time.timeScale = 1f;
        PlayerSelector.selectedPlayer = femalePlayerPrefab;
        PlayerSelector.SelectPlayer(femalePlayerPrefab, shiraSO);
        GameStateManager.Instance.TransitionToScene("Store");
    }

    public void ActiveGeneralPanel(string type)
    {
        cancelButton.gameObject.SetActive(true);
        generalPanel.SetActive(true);

        Debug.Log(type);

        switch (type)
        {
            case "Settings":
                panelTitleText.text = "Settings";
                panelBodyText.text = "You can mute music here";
                break;
            case "Credits":
                panelTitleText.text = "Credits";
                panelBodyText.text = "Mentor: " +
                    "       Ella Luna Pleasance" +
                    " Programmers:" +
                    "       Fany Manevich, Amir Melicson " +
                    "Animators:" +
                    "       Tal Kerklies, Yarin Peled";
                break;
            case "Help":
                panelTitleText.text = "Help";
                panelBodyText.text = "Choose player." +
                    "Move with arrows or WSDA. " +
                    "Help the customers choose the plant of there dreams.";
                break;
            default:
                panelTitleText.text = "Panel not working";
                panelBodyText.text = "ERROR";
                break;
        }

    }

    public void OnQuitClicked()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    public void ClosePanelButton()
    {
        cancelButton.gameObject.SetActive(false);
        generalPanel.SetActive(false);
    }
}