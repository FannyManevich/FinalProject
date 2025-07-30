using TMPro;
using UnityEngine;

public class ShiftManager : MonoBehaviour
{
    [Header("Panels:")]
    [SerializeField] private GameObject endOfDayPanel;

    [Header("Sales Text:")]
    [SerializeField] private TextMeshProUGUI dailySalesGoalText;
    [SerializeField] private TextMeshProUGUI dayNumberText;
    [SerializeField] private TextMeshProUGUI totalRevenueText;
    [SerializeField] private TextMeshProUGUI profitText;
    [SerializeField] private TextMeshProUGUI revenueLostText;
    [SerializeField] private TextMeshProUGUI totalBalanceText;
    [SerializeField] private TextMeshProUGUI mismatchedPlantsText;

    [Header("Customers Text:")]
    [SerializeField] private TextMeshProUGUI totalCustomersText;
    [SerializeField] private TextMeshProUGUI customersServedText;
    [SerializeField] private TextMeshProUGUI customersLostText;
    
    private int day = 1;
    private float todaysGoal = 100;
    private float totalBalance = 0;
    private float profit = 0;
    private int totalCustomers = 0;
    private int customersServed = 0;
    private int customersLost = 0;
    private float revenue = 0;
    private float revenueLost = 0;
    //private int mismatchedSales = 0;
    private int mismatchedPlants = 0;
    private void OnEnable()
    {
        NPCEventManager.OnSaleSuccess += SuccessfulSale;
        NPCEventManager.OnCustomerWalkout += Walkouts;
        NPCEventManager.OnMismatchedSale += MismatchedSale;
        NPCEventManager.OnCustomerArrived += HandleCustomerArrival;
    }
    private void OnDisable()
    {
        NPCEventManager.OnSaleSuccess -= SuccessfulSale;
        NPCEventManager.OnCustomerWalkout -= Walkouts;
        NPCEventManager.OnMismatchedSale -= MismatchedSale;
        NPCEventManager.OnCustomerArrived -= HandleCustomerArrival;
    }

    public void AddRevenue(float amount) => revenue += amount;
    public void AddRevenueLost(float amount) => revenueLost += amount;
    public void AddCustomerServed() => customersServed++;
    public void AddCustomerLost() => customersLost++;

    public void ShowEndOfShiftPanel()
    {
        profit = revenue - revenueLost;

        dayNumberText.text = $"Day {day}";
        dailySalesGoalText.text = $"Daily Goal: ${todaysGoal:F0}";

        totalRevenueText.text = $"Revenue: ${revenue:F0}";
        profitText.text = $"Prof" +
            $"" +
            $"" +
            $"" +
            $"" +
            $"it: ${profit:F0}";
        revenueLostText.text = $"Revenue Lost: ${revenueLost:F0}";     
        totalBalanceText.text = $"Total Balance: ${totalBalance + profit:F0}";

        customersServedText.text = $"Customers Served: {customersServed}";
        customersLostText.text = $"Customers Lost: {customersLost}";
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
    private void SuccessfulSale(int amount)
    {
        AddRevenue(amount);
        AddCustomerServed();
    }

    public void Walkouts(int penalty)
    {
        AddRevenueLost(penalty);
        AddCustomerLost();
    }
    public void MismatchedSale()
    {
       // mismatchedSale++;
    }
    private void HandleCustomerArrival()
    {
        totalCustomers++;
    }
    public void EndDay()
    {
        UpdatePanelUI();
        endOfDayPanel.SetActive(true);
    }
    private void UpdatePanelUI()
    {
        float profit = revenue - revenueLost;
        int totalCustomers = customersServed + customersLost;

        dayNumberText.text = $"DAY {day}";
        totalRevenueText.text = $"REVENUE: {revenue:F0}";
        profitText.text = $"PROFIT: {profit:F0}";
        revenueLostText.text = $"REVENUE LOST: {revenueLost:F0}";
        totalBalanceText.text = $"TOTAL BALANCE: {totalBalance:F0}";

        totalCustomersText.text = $"TOTAL: {totalCustomers}";
        customersServedText.text = $"SERVED: {customersServed}";
        customersLostText.text = $"WALKOUTS: {customersLost}";
        mismatchedPlantsText.text = $"MISMATCHED PLANT: {mismatchedPlants}";
    }
    public void StartNextDay()
    {
        endOfDayPanel.SetActive(false);
        totalBalance += (revenue - revenueLost);
        day++;

        revenue = 0;
        revenueLost = 0;
        customersServed = 0;
        customersLost = 0;
        mismatchedPlants = 0;

        FindObjectOfType<TimerController>().ResetTimer();
    }
}