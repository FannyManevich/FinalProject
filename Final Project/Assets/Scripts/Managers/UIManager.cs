using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Managers;

public class UIManager : MonoBehaviour
{   
    [Header("Managers:")]
    [SerializeField] private BeaconSO beacon;     
    [SerializeField] private ShiftManager shiftManager;
    [SerializeField] private TimerController timerController;

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
    [SerializeField] GameObject endShiftPanel;
    [SerializeField] GameObject endShiftContent;
    [SerializeField] GameObject endShiftButtons;

    [Header("Pages:")]
    [SerializeField] Sprite[] SpritesSideA;
    [SerializeField] Sprite[] SpritesSideB;
    [SerializeField] GameObject SideA;
    [SerializeField] GameObject SideB;
    private int PageNumber = 0;
    private void OnEnable()
    {
        if (beacon?.gameStateChannel != null)
        {
            beacon.gameStateChannel.StateEnter += HandleGameStateChange;
        }
        if(beacon?.inputChannel != null)
        {
            beacon.inputChannel.OnBookEvent += OpenBook; 
            beacon.inputChannel.OnHelpEvent += OpenHelp;
            beacon.inputChannel.OnCancelEvent += CloseAllPanels;
        }
        homeButton.onClick.AddListener(OnHomeClicked);
        bookButton.onClick.AddListener(OpenBook);
        helpButton.onClick.AddListener(OpenHelp);
        restartButton.onClick.AddListener(OnRestartShiftClicked);
        nextDayButton.onClick.AddListener(OnRestartShiftClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
        closeBookButton.onClick.AddListener(CloseAllPanels);
        closeHelpButton.onClick.AddListener(CloseAllPanels);
    }
    private void OnDisable()
    {
        if (beacon?.gameStateChannel != null)
        {
            beacon.gameStateChannel.StateEnter -= HandleGameStateChange;
        }
        if (beacon?.inputChannel != null)
        {
            beacon.inputChannel.OnBookEvent -= OpenBook;
            beacon.inputChannel.OnHelpEvent -= OpenHelp;
            beacon.inputChannel.OnCancelEvent -= CloseAllPanels;
        }
        homeButton.onClick.RemoveListener(OnHomeClicked);
        bookButton.onClick.RemoveListener(OpenBook);
        helpButton.onClick.RemoveListener(OpenHelp);
        restartButton.onClick.RemoveListener(OnRestartShiftClicked);
        nextDayButton.onClick.RemoveListener(OnRestartShiftClicked);
        quitButton.onClick.RemoveListener(OnQuitClicked);
        closeBookButton.onClick.RemoveListener(CloseAllPanels);
        closeHelpButton.onClick.RemoveListener(CloseAllPanels);
    }
    private void HandleGameStateChange(GameState newState)
    {
        CloseAllPanels();
        switch (newState)
        {
            case GameState.Playing:
                CloseAllPanels();
                break;
            case GameState.Book:
                bookPanel.SetActive(true);
                closeBookButton.gameObject.SetActive(true);
                break;
            case GameState.Help:
                helpPanel.SetActive(true);
                closeHelpButton.gameObject.SetActive(true);
                break;
            case GameState.EndShift:
                endShiftPanel.SetActive(true);
                shiftManager?.ShowEndOfShiftPanel();
                break;
        }
    }
    public void OpenBook()
    {
        //Debug.Log("OpenBook called");
        GameStateManager.Instance.ChangeGameState(GameState.Book);
    }
    public void OpenHelp()
    { 
        //Debug.Log("OpenHelp called");
        GameStateManager.Instance.ChangeGameState(GameState.Help);      
    }
    public void OnHomeClicked()
    {
        //Debug.Log("OnHomeClicked called");
        GameStateManager.Instance.ChangeGameState(GameState.MainMenu);        
    }
    public void OnRestartShiftClicked()
    {
        //Debug.Log("OnRestartShiftClicked called");
        timerController?.ResetTimer();
        shiftManager?.ResetDayStats();
        GameStateManager.Instance.ChangeGameState(GameState.Playing);
    }
    public void CloseAllPanels()
    {
        Debug.Log("CloseAllPanels called");
        bookPanel.SetActive(false);
        helpPanel.SetActive(false);        
        closeBookButton.gameObject.SetActive(false);
        closeHelpButton.gameObject.SetActive(false);
        GameStateManager.Instance.ChangeGameState(GameState.Playing);
    }
    public void OnQuitClicked()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }    
    public void TriggerEndOfDay()
    {
        Debug.Log("In UIManager: End of day triggered. Opening shift panel.");
        GameStateManager.Instance.ChangeGameState(GameState.EndShift);
        if (shiftPanel != null)
        {
            shiftPanel.SetActive(true);
            endShiftPanel.SetActive(true);
            endShiftContent.SetActive(true);
            endShiftButtons.SetActive(true);
        }
        if (shiftManager != null)
        {
            shiftManager.ShowEndOfShiftPanel();
        }
    }
    public void NextPage()
    {
        if (PageNumber < SpritesSideA.Length-1)
        {
            PageNumber++;
            SideA.GetComponent<Image>().sprite = SpritesSideA[PageNumber];
            SideB.GetComponent<Image>().sprite = SpritesSideB[PageNumber];
        }
    }
    public void PrevPage()
    {
        if (PageNumber > 0)
        {
            PageNumber--;
            SideA.GetComponent<Image>().sprite = SpritesSideA[PageNumber];
            SideB.GetComponent<Image>().sprite = SpritesSideB[PageNumber];
        }
    }
}