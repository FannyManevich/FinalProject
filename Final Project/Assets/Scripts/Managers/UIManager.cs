using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Managers;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private int PageNumber=1;

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

    [Header("Pages:")]
    [SerializeField] Sprite[] SpritesSideA;
    [SerializeField] Sprite[] SpritesSideB;
    [SerializeField] GameObject SideA;
    [SerializeField] GameObject SideB;

    private void OnEnable()
    {
        GameStateManager.Instance.OnGameStateChange += HandleGameStateChange;
        beacon.inputChannel.OnBookEvent += OpenBook;
        beacon.inputChannel.OnHelpEvent += OpenHelp;
        beacon.inputChannel.OnCancelEvent += CloseAllPanels;

        homeButton.onClick.AddListener(OnHomeClicked);
        bookButton.onClick.AddListener(OpenBook);
        helpButton.onClick.AddListener(OpenHelp);
        homeButton.onClick.AddListener(OnHomeClicked);
        restartButton.onClick.AddListener(OnRestartShiftClicked);
        nextDayButton.onClick.AddListener(ResetNextShift);
        quitButton.onClick.AddListener(OnQuitClicked);
        closeBookButton.onClick.AddListener(CloseAllPanels);
        closeHelpButton.onClick.AddListener(CloseAllPanels);
    }
    private void OnDisable()
    {
        GameStateManager.Instance.OnGameStateChange -= HandleGameStateChange;
        beacon.inputChannel.OnBookEvent -= OpenBook;
        beacon.inputChannel.OnHelpEvent -= OpenHelp;
        beacon.inputChannel.OnCancelEvent -= CloseAllPanels;

        homeButton.onClick.RemoveListener(OnHomeClicked);
        bookButton.onClick.RemoveListener(OpenBook);
        helpButton.onClick.RemoveListener(OpenHelp);
        homeButton.onClick.RemoveListener(OnHomeClicked);
        restartButton.onClick.RemoveListener(OnRestartShiftClicked);
        nextDayButton.onClick.RemoveListener(ResetNextShift);
        quitButton.onClick.RemoveListener(OnQuitClicked);
        closeBookButton.onClick.RemoveListener(CloseAllPanels);
        closeHelpButton.onClick.RemoveListener(CloseAllPanels);

    }

    public void OpenBook()
    {
        //Debug.Log("OpenBook called");
        bookPanel.SetActive(true);
        closeBookButton.gameObject.SetActive(true);
        GameStateManager.Instance.SetState(GameState.Book);
    }
    public void OpenHelp()
    {
        GameStateManager.Instance.ChangeState(GameState.UI);
        //Debug.Log("OpenHelp called");
        helpPanel.SetActive(true);
        closeHelpButton.gameObject.SetActive(true);
    }
    public void OnHomeClicked()
    {
        GameStateManager.Instance.ChangeState(GameState.UI);
        //Debug.Log("OnHomeClicked called");
        SceneManager.LoadScene("Main-Menu");
    }
    public void OnRestartShiftClicked()
    {
        //Debug.Log("OnRestartShiftClicked called");
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
    private void HandleGameStateChange(GameState newState)
    {

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