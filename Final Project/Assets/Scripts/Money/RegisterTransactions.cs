using Assets.Scripts.Managers;
using UnityEngine;

public class RegisterTransactions : MonoBehaviour
{
    [SerializeField] private MoneyManager cash;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Shosh"))
        {
            //cash.AddMoney(x);
        }     
    }    
}