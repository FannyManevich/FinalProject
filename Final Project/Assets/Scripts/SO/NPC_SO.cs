using UnityEngine;

[CreateAssetMenu(fileName = "NPC", menuName = "SO/NPC", order = 2)]
public class NPC_SO : ScriptableObject
{
    public int NPC_Id;
    public string NPC_name;
    public Sprite NPC_portrait;
    public RuntimeAnimatorController NPC_Animation;
    public PlantSO plantRequest;
}
