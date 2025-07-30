using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShiftManager : MonoBehaviour
{
    [Header("Managers:")]
    [SerializeField] private MoneyManager moneyManager;

    [Header("Win/Loss Logic:")]
    [SerializeField] private TextMeshProUGUI winLossStatusText;
    [SerializeField] private Button nextDayButton;

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
    private float todaysGoal = 1000;
    private float totalBalance = 0;
    private float profit = 0;
    private int customersServed = 0;
    private int customersLost = 0;
    private float revenue = 0;
    private float revenueLost = 0;

    public int Day => day;
    public float TodaysGoal => todaysGoal;
    public float TotalBalance => totalBalance;
    public float Profit => profit;
    public float Revenue => revenue;
    public float RevenueLost => revenueLost;
    public int CustomersServed => customersServed;
    public int CustomersLost => customersLost;

    public void AddRevenue(float amount) => revenue += amount;
    public void AddRevenueLost(float amount) => revenueLost += amount;
    public void AddCustomerServed() => customersServed++;
    public void AddCustomerLost() => customersLost++;

    public void ShowEndOfShiftPanel()
    {
        profit = moneyManager.CurrentMoney;

        //dayNumberText.text = $"Day {day}";

        dailySalesGoalText.text = $"Daily Goal: ${todaysGoal:F0}";

        //totalRevenueText.text = $"Revenue: ${moneyManager.CurrentMoney:F0}";
        //servedNPCText.text = $"Customers Served: {customersServed}";
        //lostNPCText.text = $"Customers Lost: {customersLost}";
        //revenueLostText.text = $"Revenue Lost: ${revenueLost:F0}";
        profitText.text = $"Profit: ${profit:F0}";
        totalBalanceText.text = $"Total Balance: ${totalBalance + profit:F0}";

        if (profit >= todaysGoal)
        {
            winLossStatusText.text = "You reached the daily goal!";
            winLossStatusText.color = Color.green;
            nextDayButton.gameObject.SetActive(true);
        }
        else
        {
            winLossStatusText.text = "You didn't reach the daily goal.";
            winLossStatusText.color = Color.red;
            nextDayButton.gameObject.SetActive(false);
        }
        if (moneyManager != null) moneyManager.ResetMoney();
    }
    public void ResetDayStats()
    {
        totalBalance += profit;
        day++;
        todaysGoal *= 1.1f;
        profit = 0;
        revenue = 0;
        revenueLost = 0;
        customersServed = 0;
        customersLost = 0;
    }   
}
