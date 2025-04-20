using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public Transform playerSpawnPoint;
    void Start()
    {

        if (PlayerSelector.selectedPlayer != null)
        {
            Instantiate(PlayerSelector.selectedPlayer, playerSpawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("You need to select a player prefab.");
        }
    }

}
