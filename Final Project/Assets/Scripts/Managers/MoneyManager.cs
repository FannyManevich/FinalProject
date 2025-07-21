using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI MoneyText;
    public int currentMoney { get; private set; } = 0;

    public void AddMoney(int Price)
    {
        currentMoney += Price;
        MoneyText.text = "" + currentMoney;
    }
    public void SubtractMoney(int amount)
    {
        currentMoney -= amount;
        UpdateText();
    }
    public void AddTip(int amount)
    {
        currentMoney += amount;
        UpdateText();
    }
    public void ResetMoney()
    {
        currentMoney = 0;
        MoneyText.text = "0";
    }
    private void UpdateText()
    {
        if (MoneyText != null)
        {
            MoneyText.text = currentMoney.ToString();
        }
        else
        {
            Debug.LogWarning("MoneyText is not assigned!");
        }         
    }
}