using UnityEngine;

[CreateAssetMenu(fileName = "NPC", menuName = "NPC", order = 1)]
public class NPC_SO : ScriptableObject
{
    public string NPCname;
    public Sprite NPCportrait;

    [TextArea(3, 10)]
    public string[] sentences;
}
