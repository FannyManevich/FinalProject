using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject selectPlayerPanel;
    [SerializeField] GameObject creditsPanel;
    [SerializeField] GameObject helpPanel;

    [Header("Buttons")]
    [SerializeField] Button playButton;
    [SerializeField] Button creditsButton;
    [SerializeField] Button helpButton;
    [SerializeField] Button quitButton; 

    [SerializeField] Button cancelSelectButton;
    [SerializeField] Button cancelHelpButton;
    [SerializeField] Button cancelCreditsButton;

    [Header("Players")]
    public PlayerSO itanSO;
    public PlayerSO shiraSO;
    public GameObject malePlayerPrefab;
    public GameObject femalePlayerPrefab;

    public void OnPlayClicked()
    { 
        Debug.Log("Play Button clicked");

        cancelSelectButton.gameObject.SetActive(true);      
        selectPlayerPanel.SetActive(true);
    }

    public void OnMaleSelected()
    {
        Debug.Log("In MainMenuManager:  " + "Male button clicked");
        Time.timeScale = 1f;
        PlayerSelector.selectedPlayer = malePlayerPrefab;
        PlayerSelector.SelectPlayer(malePlayerPrefab, itanSO);
        GameStateManager.Instance.TransitionToScene("Store");

        //GameStateManager.Instance.TransitionToScene("Store_Fix");       
        //GameStateManager.Instance.TransitionToScene("Scenechanges");
    }

    public void OnFemaleSelected()
    {
        Debug.Log("In MainMenuManager:  " + "Female button clicked");
        Time.timeScale = 1f;
        PlayerSelector.selectedPlayer = femalePlayerPrefab;
        PlayerSelector.SelectPlayer(femalePlayerPrefab, shiraSO); 
        GameStateManager.Instance.TransitionToScene("Store");

        //GameStateManager.Instance.TransitionToScene("Store_Fix");      
        //GameStateManager.Instance.TransitionToScene("Scenechanges");
    }

    public void OpenCreditsPanel()
    {
        cancelCreditsButton.gameObject.SetActive(true);
        creditsPanel.SetActive(true);

        Debug.Log("In MainMenuManager:  " + "OpenCreditsPanel()");
    }

    public void OpenHelpPanel()
    {
        cancelHelpButton.gameObject.SetActive(true);
        helpPanel.gameObject.SetActive(true);

        Debug.Log("In MainMenuManager:  " + "OpenHelpPanel()");
    }

    public void ClosePlayPanel()
    {
        cancelSelectButton.gameObject.SetActive(false);
        selectPlayerPanel.gameObject.SetActive(false);
    }

    public void CloseHelpPanel()
    {
        cancelHelpButton.gameObject.SetActive(false);
        helpPanel.gameObject.SetActive(false);
    }

    public void CloseCreditsPanel()
    {
        cancelCreditsButton.gameObject.SetActive(false);
        creditsPanel.gameObject.SetActive(false);
    }

    public void OnQuitClicked()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}