using UnityEngine;
using UnityEngine.UI;

public class DifficultyLevelManager : MonoBehaviour
{
    // public event Action<int> LevelChangedEvent;

    public int levelScore = 3;
    public Image[] level;
    public Sprite fullLevel;
    public Sprite emptyLevel;

    private void Awake()
    {
        UpdateLevelUI();
    }

    public void LevelUpdate()
    {
        if (levelScore > 0)
        {
            levelScore -= 1;
            level[levelScore].sprite = emptyLevel;
        }

        UpdateLevelUI();
        //     LevelChangedEvent?.Invoke(levelScore);
    }

    public void IncreaseHealth()
    {
        if (levelScore < level.Length)
        {
            level[levelScore].sprite = fullLevel;
            levelScore += 1;
        }
        else
        {
            Debug.Log("Water at maximum!");
        }

        UpdateLevelUI();
        //LevelChangedEvent?.Invoke(levelScore);
    }
    private void UpdateLevelUI()
    {
        for (int i = 0; i < level.Length; i++)
        {
            level[i].sprite = (i < levelScore) ? fullLevel : emptyLevel;
        }
    }

}
