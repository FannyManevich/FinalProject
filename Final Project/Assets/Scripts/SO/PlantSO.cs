using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Plant", menuName = "SO/Plant", order = 1)]
public class PlantSO : ScriptableObject
{
    public int price;
    public int waterRequirement;
    public int sunRequirement;
    public int difficultyLevel;
    public Sprite image;   
    public Transform getTransform;
}