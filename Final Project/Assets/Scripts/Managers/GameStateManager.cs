using UnityEngine;
using System;
using Assets.Scripts.Managers;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public event Action<GameState> OnGameStateChange; 

    public static GameStateManager Instance { get; private set; }
    public GameStateChannel gameStateChannel;

    public Input playerInput;
    public PlayerSO selectedPlayerSO;
    public GameObject currentPlayer;

    public GameState currentState;
    public GameState CurrentGameState => currentState;

    public Transform playerSpawnPoint;
    void Start()
    {
        //if (PlayerSelector.selectedPlayer != null)
        //{
        //    currentPlayer = Instantiate(PlayerSelector.selectedPlayer, playerSpawnPoint.position, Quaternion.identity);

        //    PlayerBehavior playerComponent = currentPlayer.GetComponent<PlayerBehavior>();

        //    if (playerComponent != null)
        //    {
        //        selectedPlayerSO = PlayerSelector.selectedPlayerSO;
        //    }
        //    else
        //    {
        //        Debug.LogError("Spawned player is missing Player component!");
        //    }
        SetState(currentState); 
        OnGameStateChange += HandleGameStateChange;     
        //}
        //else
        //{
        //    Debug.LogError("Select a player prefab.");
        //}        
    }

    private void FindPlayerSpawnPoint()
    {
        if (playerSpawnPoint == null)
        {
            GameObject spawn = GameObject.FindWithTag("PlayerSpawn");
            if (spawn != null)
            {
                playerSpawnPoint = spawn.transform;
                Debug.Log("Found PlayerSpawn point automatically.");
            }
            else
            {
                Debug.LogError("No GameObject with 'PlayerSpawn' tag found!");
            }
        }
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void SpawnPlayer()
    {
        if (PlayerSelector.selectedPlayer != null)
        {
            currentPlayer = Instantiate(PlayerSelector.selectedPlayer, playerSpawnPoint.position, Quaternion.identity);

            PlayerBehavior playerComponent = currentPlayer.GetComponent<PlayerBehavior>();

            if (playerComponent != null)
            {
                selectedPlayerSO = PlayerSelector.selectedPlayerSO;
            }
            else
            {
                Debug.LogError("In GameStateManager:  Spawned player is missing PlayerBehavior script!");
            }
        }
        else
        {
            Debug.LogError("In GameStateManager:  Select a Player.");
        }
    }
    public void SetCurrentState(GameState state)
    {
        currentState = state;
        if (gameStateChannel != null)
        {
            gameStateChannel.StateEntered(state);
        }
        else
        {
            Debug.LogError("GameStateChannel is not assigned!");
        }
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
                SwitchToPlayerInput();
                Debug.Log("Switched to Player action map.");
                break;

            case GameState.MainMenu:
                SwitchToUIInput();
                Debug.Log("Switched to UI action map.");                
                break;

            case GameState.UI:
                SwitchToUIInput();
                Debug.Log("Switched to UI action map.");
                break;

            case GameState.Book:
                SwitchToPlayerInput();
                Debug.Log("Switched to Player action map.");
                break;
            case GameState.Dialogue:
                SwitchToUIInput();
                break;
            case GameState.EndGame:
                SwitchToUIInput();
                break;

            default:
                Debug.LogWarning(newState);
                break;
        }
    }

    void SwitchToUIInput()
    {
        playerInput.UI.Enable();
        playerInput.Player.Disable();
    }

    void SwitchToPlayerInput()
    {
        playerInput.UI.Disable();
        playerInput.Player.Enable();
    }
    public void ChangeState(GameState newState)
    {
        if (gameStateChannel != null)
        {
            gameStateChannel.StateExited(currentState);
            currentState = newState;
            gameStateChannel.StateEntered(currentState);
        }
        else
        {
            Debug.LogError("GameStateChannel is not assigned!");
        }
    }

    public void SetState(GameState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
            OnGameStateChange?.Invoke(currentState);
            Debug.Log("State changed to: " + currentState);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Store")
        {
            Debug.Log("Store scene loaded! Spawning player...");
            FindPlayerSpawnPoint();

            GameObject existingPlayer = GameObject.FindGameObjectWithTag("Player");

            if (existingPlayer != null)
            {
                    Debug.Log("Player already exists in scene.");
                    currentPlayer = existingPlayer;
            }
            else if (PlayerSelector.selectedPlayer != null)
            {
                    Debug.Log("Spawning new player based on selection");
                    SpawnPlayer();
            }
            else
            {
                    Debug.LogError("No player selected in PlayerSelector!");
            }            
        }
    }

    private void BookOpened()
    {
        if (currentState == GameState.Book)
        {
            currentState = GameState.Playing;
            Time.timeScale = 1f;
        }
        else
        {
            currentState = GameState.Book;
            Time.timeScale = 0f;
        }

        OnGameStateChange?.Invoke(currentState);
    }

    public void TransitionToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Debug.Log("In GameStateManager:  Transitioning to: " + sceneName);
    } 
}