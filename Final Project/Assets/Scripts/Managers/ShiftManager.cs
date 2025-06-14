using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShiftManager : MonoBehaviour
{
    [Header("Panels & Buttons:")]
    [SerializeField] GameObject endOfShiftPanel;
    [SerializeField] Button quitButton;
    [SerializeField] Button nextDayButton;

    [Header("Text:")]
    [SerializeField] TextMeshProUGUI dayNumberText;
    [SerializeField] TextMeshProUGUI dailySalesGoalText;
    [SerializeField] TextMeshProUGUI totalRevenueText;   
    [SerializeField] TextMeshProUGUI servedNPCText;
    [SerializeField] TextMeshProUGUI lostNPCText;
    [SerializeField] TextMeshProUGUI revenueLostText;
    [SerializeField] TextMeshProUGUI profitText;
    [SerializeField] TextMeshProUGUI totalBalanceText;

    private int day = 1;
    private float todaysGoal = 100;
    private float totalBalance = 0;
    private float profit = 0;
    private int customersServed = 0;
    private int customersLost = 0;
    private float revenue = 0;
    private float revenueLost = 0;

    public void AddRevenue(float amount) => revenue += amount;
    public void AddRevenueLost(float amount) => revenueLost += amount;
    public void AddCustomerServed() => customersServed++;
    public void AddCustomerLost() => customersLost++;

    public void ShowEndOfShiftPanel()
    {
        endOfShiftPanel.SetActive(true);

        dayNumberText.text = $"Day {day}";
        dailySalesGoalText.text = $"Daily Goal: ${todaysGoal:F0}";
        totalRevenueText.text = $"Revenue: ${revenue:F0}";
        servedNPCText.text = $"Customers Served: {customersServed}";
        lostNPCText.text = $"Customers Lost: {customersLost}";
        revenueLostText.text = $"Revenue Lost: ${revenueLost:F0}";
        profitText.text = $"Profit: ${profit:F0}";
        totalBalanceText.text = $"Total Balance: ${totalBalance + profit:F0}";
    }

    public void NextDayClicked()
    {
        day++;
        todaysGoal *= 1.1f;
        todaysGoal = 0;
        totalBalance += profit;
        ResetNextShift();

        endOfShiftPanel.SetActive(false);
        quitButton.gameObject.SetActive(false);
        nextDayButton.gameObject.SetActive(false);
    }

    public void ResetNextShift()
    {
        profit = 0;
        revenue = 0;
        revenueLost = 0;
        customersServed = 0;
        customersLost = 0;
        this.GetComponent<TimerController>().ResetTimer();
    }

    public void OnQuitClicked()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
