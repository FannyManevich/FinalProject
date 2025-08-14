using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "SO/Player", order = 0)]
public class PlayerSO : ScriptableObject
{
    public string playerName;
    public Sprite playerPortrait;
    public PlantSO plantPicked;
}

