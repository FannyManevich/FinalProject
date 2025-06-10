using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DayEndManager : MonoBehaviour
{
    [SerializeField] GameObject dayEndScreen;
    [SerializeField] TextMeshProUGUI MoneyMadeText;
    [SerializeField] TextMeshProUGUI WonText;
    [SerializeField] Button againButton;
    [SerializeField] Button continueButton;
    private int ThisDayMoneyGoal;
    private int CurrentMoney;
    // Start is called before the first frame update
    void Start()
    {
        dayEndScreen.SetActive(false);
        ThisDayMoneyGoal = 100;
        againButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
    }

    public void EndDay()
    {
        CurrentMoney = this.GetComponent<MoneyManager>().currentMoney;
        dayEndScreen.SetActive(true);
        MoneyMadeText.text="Money Made: "+ CurrentMoney + "/"+ ThisDayMoneyGoal;
        if(CurrentMoney >= ThisDayMoneyGoal)
        {
            WonText.text = "You Won!!";
            continueButton.gameObject.SetActive(true);
        }
        else
        {
            WonText.text = "You lost :(";
            againButton.gameObject.SetActive(true);
        }
    }

    public void OnQuitClicked()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void OnContinueClicked()
    {
        dayEndScreen.SetActive(false);
        ThisDayMoneyGoal += 100;
        this.GetComponent<TimerController>().ResetTimer();
        //ResetDay
        againButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
    }

    public void OnPlayAgainClicked()
    {
        dayEndScreen.SetActive(false);
        ThisDayMoneyGoal = 100;
        this.GetComponent<TimerController>().ResetTimer();
        //ResetDay
        againButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
    }
}
