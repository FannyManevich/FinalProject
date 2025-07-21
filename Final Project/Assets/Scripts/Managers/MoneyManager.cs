using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI MoneyText;
    public int CurrentMoney { get; private set; } = 0;

    public void AddMoney(int Price)
    {
        CurrentMoney += Price;
        MoneyText.text = "" + CurrentMoney;
    }
    public void SubtractMoney(int amount)
    {
        CurrentMoney -= amount;
        UpdateText();
    }
    public void AddTip(int amount)
    {
        CurrentMoney += amount;
        UpdateText();
    }
    public void ResetMoney()
    {
        CurrentMoney = 0;
        MoneyText.text = "0";
    }
    private void UpdateText()
    {
        if (MoneyText != null)
        {
            MoneyText.text = CurrentMoney.ToString();
        }
        else
        {
            Debug.LogWarning("MoneyText is not assigned!");
        }         
    }
}