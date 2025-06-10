using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    [SerializeField] Slider timerSlider;
    [SerializeField] int MinutesPerDay;
    private int fixToMin;
    public bool stopTimer;

    // Start is called before the first frame update
    void Start()
    {
        ResetTimer();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (stopTimer == false)
        {
            timerSlider.value--;
            if (timerSlider.value <= 0)
            {
                Debug.Log("day ended");
                stopTimer = true;
                this.GetComponent<DayEndManager>().EndDay();
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
