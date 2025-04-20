using UnityEngine;
using UnityEngine.UI;

public class WaterManager : MonoBehaviour
{
   // public event Action<int> WaterChangedEvent;

    public int WaterScore = 3;
    public Image[] water;
    public Sprite fullDrop;
    public Sprite emptyDrop;

    private void Awake()
    {
        UpdateWaterUI();
    }

    public void WaterUpdate()
    {
        if (WaterScore > 0)
        {
            WaterScore -= 1;
            water[WaterScore].sprite = emptyDrop;
        }

        UpdateWaterUI();
   //     WaterChangedEvent?.Invoke(WaterScore);
    }

    public void IncreaseHealth()
    {
        if (WaterScore < water.Length)
        {
            water[WaterScore].sprite = fullDrop;
            WaterScore += 1;
        }
        else
        {
            Debug.Log("Water at maximum!");
        }

        UpdateWaterUI();
        //WaterChangedEvent?.Invoke(WaterScore);
    }

    private void UpdateWaterUI()
    {
        for (int i = 0; i < water.Length; i++)
        {
            water[i].sprite = (i < WaterScore) ? fullDrop : emptyDrop;
        }
    }
}
