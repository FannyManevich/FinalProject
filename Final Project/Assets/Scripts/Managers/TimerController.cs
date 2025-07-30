using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] Slider timerSlider;
    [SerializeField] int MinutesPerDay;
    private int fixToMin;
    public bool stopTimer;

    void Start()
    {
        if (uiManager == null)
        {
            Debug.LogError("UIManager is not assigned in the TimerController's Inspector!");
        }
        ResetTimer();
    }
    void FixedUpdate()
    {
        if (stopTimer == false)
        {
            timerSlider.value--;
            if (timerSlider.value <= 0)
            {
                Debug.Log("day ended");
                stopTimer = true;

                if (uiManager != null)
                {
                    uiManager.TriggerEndOfDay();
                }
                else
                {
                    Debug.LogError("DayEndManager component not found on the same GameObject as TimerController.");
                }
            }
        }
    }

    public void ResetTimer()
    {
        fixToMin = 50 * 60 * MinutesPerDay;
        timerSlider.maxValue = fixToMin;
        timerSlider.value = fixToMin;
        stopTimer = false;
    }
}
