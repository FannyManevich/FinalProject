using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "SO/Player", order = 0)]
public class PlayerSO : ScriptableObject
{
    public String playerName;
    public Sprite playerPortrait;

    [TextArea(3, 10)]
    public string[] sentences;
}

