using UnityEngine;
using System;
using Assets.Scripts.Managers;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{  
    public static GameStateManager Instance { get; private set; }
    
    [Header("Beacon:")]
    [SerializeField] private BeaconSO beacon;

    [Header("Managers:")]
    [SerializeField] private UIManager uiManager;

    public event Action<GameState> OnGameStateChange;

    [Header("Settings:")]
    [SerializeField] private string storeSceneName = "Store";
    [SerializeField] private GameState startingState = GameState.MainMenu;
    [SerializeField] private Transform playerSpawnPoint;

    public GameState CurrentGameState { get; private set; }
    public PlayerSO SelectedPlayerSO { get; private set; }
    public GameObject CurrentPlayerGO { get; private set; }

    private Input playerInput;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        InputReader inputReader = FindObjectOfType<InputReader>();
        if (inputReader != null)
        {
            playerInput = inputReader.inputActions;
        }
        else
        {
            Debug.LogError("In GameStateManager: No InputReader found in scene.");
        }
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void Start()
    {
        ChangeGameState(startingState);          
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnGameStateChange += HandleGameStateChange;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        OnGameStateChange -= HandleGameStateChange;
    }
    private void HandleGameStateChange(GameState newState)
    {
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not assigned.");
            return;
        }
        switch (newState)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                SwitchToPlayerInput();
                //Debug.Log("Switched to Player action map.");
                break;
            case GameState.MainMenu:
                Time.timeScale = 0f;
                //Debug.Log("Switched to UI action map.");
                TransitionToScene("Main-Menu");
                break;
            case GameState.Panels:
                Time.timeScale = 0f;
                SwitchToUIInput();
                //Debug.Log("Switched to UI action map.");
                break;
            case GameState.Dialogue:
                Time.timeScale = 0f;
                // SwitchToUIInput();
                break;
            case GameState.EndShift:
                Time.timeScale = 0f;
                SwitchToUIInput();
                break;
            default:
                Debug.LogWarning("HandleGameStateChange: No input map defined for state " + newState);
                break;
        }
    }
    void SwitchToUIInput()
    {
        playerInput.Player.Disable();

        if (!playerInput.UI.enabled)
            playerInput.UI.Enable();
    }
    void SwitchToPlayerInput()
    {
        playerInput.Player.Enable();

        if (!playerInput.UI.enabled)
            playerInput.UI.Enable();
    }
    public void ChangeGameState(GameState newState)
    {
        if (CurrentGameState == newState) return;

        if (beacon != null && beacon.gameStateChannel != null)
        {
            beacon.gameStateChannel.StateExited(CurrentGameState);
        }

        CurrentGameState = newState;
        //Debug.Log("Game State changed to: " + newState);

        if (beacon != null && beacon.gameStateChannel != null)
        {
            beacon.gameStateChannel.StateEntered(newState);
        }
        OnGameStateChange?.Invoke(newState);
    } 
    public void TransitionToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        //Debug.Log("In GameStateManager:  Transitioning to: " + sceneName);
    } 
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == storeSceneName)
        {
            FindPlayerSpawnPoint();
            HandlePlayerSpawning();
        }
    }
    private void FindPlayerSpawnPoint()
    {
        if (playerSpawnPoint != null) return;

        GameObject spawnObject = GameObject.FindWithTag("PlayerSpawn");
        if (spawnObject != null)
        {
            playerSpawnPoint = spawnObject.transform;
           // Debug.Log("In GameStateManager: Found PlayerSpawn point via tag.");
        }
        else
        {
            Debug.LogError("In GameStateManager: No player spawn point assigned or found with PlayerSpawn tag.");
        }
    }
    private void HandlePlayerSpawning()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            Debug.Log("In GameStateManager:  Player object already exists in the scene.");
            return;
        }

        if (PlayerSelector.selectedPlayer != null && playerSpawnPoint != null)
        {
            CurrentPlayerGO = Instantiate(PlayerSelector.selectedPlayer, playerSpawnPoint.position, Quaternion.identity);
            SelectedPlayerSO = PlayerSelector.selectedPlayerSO;
            //Debug.Log("In GameStateManager: Spawned player: " + SelectedPlayerSO.name);
        }
    } 
}