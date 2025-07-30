using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayEndManager : MonoBehaviour
{
   // [Header("Managers:")]
   // //[SerializeField] ShiftManager shiftManager;

   // [Header("Panels:")]
   // [SerializeField] GameObject dayEndPanel;
   // [SerializeField] GameObject dayEndButtons;

   // //[Header("Text:")]
   // //[SerializeField] TextMeshProUGUI MoneyMadeText;
   // //[SerializeField] TextMeshProUGUI WonText;

   // [Header("Buttons:")]
   //// [SerializeField] Button againButton;
   // [SerializeField] Button nextDayButton;
   // [SerializeField] Button quitButton;

   // private int ThisDayMoneyGoal;
   // private int CurrentMoney;

   // void Start()
   // {
   //     dayEndPanel.SetActive(false);
   //     dayEndButtons.SetActive(false);
   //     ThisDayMoneyGoal = 100;
   //     nextDayButton.gameObject.SetActive(false);
   // }

   // public void EndDay()
   // {
   //    // CurrentMoney = this.GetComponent<MoneyManager>().currentMoney;
   //     dayEndPanel.SetActive(true);
   //     dayEndButtons.SetActive(true);
   //     //shiftManager.
   //         //MoneyMadeText.text="Money Made: "+ CurrentMoney + "/"+ ThisDayMoneyGoal;
   //     if(CurrentMoney >= ThisDayMoneyGoal)
   //     {
   //         //WonText.text = "You Won!!";
   //         //continueButton.gameObject.SetActive(true);
   //     }
   //     else
   //     {
   //         //WonText.text = "You lost :(";
   //         //againButton.gameObject.SetActive(true);
   //     }
   // }

   // public void OnQuitClicked()
   // {
   //     Application.Quit();
   //     #if UNITY_EDITOR
   //     UnityEditor.EditorApplication.isPlaying = false;
   //     #endif
   // }

   // public void OnContinueClicked()
   // {
   //     dayEndPanel.SetActive(false);
   //     dayEndButtons.SetActive(false);
   //     ThisDayMoneyGoal += 100;
   //     this.GetComponent<TimerController>().ResetTimer();
   //     this.GetComponent<MoneyManager>().ResetMoney();
   //     //ResetDay
   //     nextDayButton.gameObject.SetActive(false);
   //     quitButton.gameObject.SetActive(false);
   // }

   // public void OnPlayAgainClicked()
   // {
   //     dayEndPanel.SetActive(false);
   //     dayEndButtons.SetActive(false);
   //     ThisDayMoneyGoal = 100;
   //     this.GetComponent<TimerController>().ResetTimer();
   //     //ResetDay
   //     nextDayButton.gameObject.SetActive(false);
   //     quitButton.gameObject.SetActive(false);
   // }
}