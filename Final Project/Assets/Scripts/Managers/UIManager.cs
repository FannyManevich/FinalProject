using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button bookButton;
    public Button settingsButton;
    public Button helpButton;
    public Button closeBookButton;
    public Button closeSettingsButton;
    public Button closeHelpButton;

    [SerializeField] GameObject bookPanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject helpPanel;

    // public InputReader inputReader;

    public void OpenBook()
    {
        bookPanel.SetActive(true);
        closeBookButton.gameObject.SetActive(true);       
    }

    //public void OpenSettings()
    //{
    //    settingsPanel.SetActive(true);
    //    closeSettingsButton.gameObject.SetActive(true);
    //}
    public void OpenHelp()
    {
        helpPanel.SetActive(true);
        closeHelpButton.gameObject.SetActive(true);
    }

    //private void HandleCancel()
    //{
    //    if (bookPanel.activeSelf)
    //    {
    //        CloseBook();
    //    }
    //    else if (settingsPanel.activeSelf)
    //    {
    //        CloseSettings();
    //    }
    //}
    public void CloseBook()
    {
        Debug.Log("CloseBook called");
        bookPanel.SetActive(false);
        closeBookButton.gameObject.SetActive(false);

    }

    //public void CloseSettings()
    //{
    //    Debug.Log("CloseSettings called");
    //    settingsPanel.SetActive(false);
    //    closeSettingsButton.gameObject.SetActive(false);
    //}

    public void CloseHelp()
    {
        Debug.Log("CloseHelp called");
        helpPanel.SetActive(false);
        closeHelpButton.gameObject.SetActive(false);
    }
}
