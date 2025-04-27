using Assets.Scripts.Managers;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameState Channel", menuName = "Channels/GameState Channel", order = 2)]
public class GameStateChannel : ScriptableObject
{
    public event Action<GameState> StateEnter;
    public event Action<GameState> StateExit;
    public void StateEntered(GameState state)
    {
        StateEnter?.Invoke(state);
    }

    public void StateExited(GameState state)
    {
        StateExit?.Invoke(state);
    }
}