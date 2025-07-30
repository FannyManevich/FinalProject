using UnityEngine;
using System;

public class NPCEventManager : MonoBehaviour
{

    public static event Action LineEnterEvent;
    public static event Action LineLeaveEvent;
    public static event Action RegisterReleaseEvent;

    public static event Action<int> OnSaleSuccess;
    public static event Action<int> OnCustomerWalkout;
    public static event Action OnMismatchedSale;
    public static event Action OnCustomerArrived;

    public static void SaleSuccess(int amount) => OnSaleSuccess?.Invoke(amount);
    public static void CustomerWalkout(int penalty) => OnCustomerWalkout?.Invoke(penalty);
    public static void MismatchedSale() => OnMismatchedSale?.Invoke();
    public static void CustomerArrived() => OnCustomerArrived?.Invoke();

    public int TotalCustomers { get; private set; }
    public int ServedCustomers { get; private set; }
    public int WalkoutCustomers { get; private set; }
    public int MismatchedSales { get; private set; }

    public static void EnterLine()
    {
        LineEnterEvent?.Invoke();
    }

    public static void LeaveLine()
    {
        LineLeaveEvent?.Invoke();
    }

    public static void RegisterRelease()
    {
        RegisterReleaseEvent?.Invoke();
    }
}
