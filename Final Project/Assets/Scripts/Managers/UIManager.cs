using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Managers;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Managers:")]
    [SerializeField] BeaconSO beacon;     
    [SerializeField] private ShiftManager shiftManager;
    [SerializeField] private TimerController timerController;
    [SerializeField] private GameStateManager gameStateManager;

    [Header("Buttons:")]
    [SerializeField] Button bookButton;
    [SerializeField] Button helpButton;
    [SerializeField] Button homeButton;
    [SerializeField] Button restartButton;
    [SerializeField] Button nextDayButton;
    [SerializeField] Button closeBookButton;
    [SerializeField] Button closeHelpButton;    
    [SerializeField] Button quitButton;   

    [Header("Panels:")]
    [SerializeField] GameObject bookPanel;
    [SerializeField] GameObject helpPanel;
    [SerializeField] GameObject shiftPanel;

    private void OnEnable()
    {
        GameStateManager.Instance.OnGameStateChange += HandleGameState;
        beacon.inputChannel.OnBookEvent += OpenBook;
        beacon.inputChannel.OnHelpEvent += OpenHelp;
        beacon.inputChannel.OnCancelEvent += CloseAllPanels;
    }
    private void OnDisable()
    {
        GameStateManager.Instance.OnGameStateChange -= HandleGameState;
        beacon.inputChannel.OnBookEvent -= OpenBook;
        beacon.inputChannel.OnHelpEvent -= OpenHelp;
        beacon.inputChannel.OnCancelEvent -= CloseAllPanels;

    }

    public void OpenBook()
    {
        Debug.Log("OpenBook called");
        bookPanel.SetActive(true);
        closeBookButton.gameObject.SetActive(true);
        GameStateManager.Instance.SetState(GameState.Book);
    }
    public void OpenHelp()
    {
        GameStateManager.Instance.ChangeState(GameState.UI);
        Debug.Log("OpenHelp called");
        helpPanel.SetActive(true);
        closeHelpButton.gameObject.SetActive(true);
    }
    public void OnHomeClicked()
    {
        GameStateManager.Instance.ChangeState(GameState.UI);
        Debug.Log("OnHomeClicked called");
        SceneManager.LoadScene("MainMenu");
    }
    public void OnRestartShiftClicked()
    {
        Debug.Log("OnRestartShiftClicked called");
        timerController.ResetTimer();
        shiftManager.ResetDayStats();      
    }
    public void OpenShiftPanel()
    {
        shiftPanel.SetActive(true);
    }
    public void CloseAllPanels()
    {
        Debug.Log("CloseAllPanels called");
        bookPanel.SetActive(false);
        helpPanel.SetActive(false);        
        closeBookButton.gameObject.SetActive(false);
        closeHelpButton.gameObject.SetActive(false);
        bookPanel.SetActive(false);
        helpPanel.SetActive(false);
        GameStateManager.Instance.SetState(GameState.Playing);
    }
    public void OnQuitClicked()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }    
    public void ResetNextShift()
    {
        shiftManager.ResetDayStats();

        shiftPanel.SetActive(false);

        quitButton.gameObject.SetActive(false);
        nextDayButton.gameObject.SetActive(false);

        this.GetComponent<TimerController>().ResetTimer();
    }
}
