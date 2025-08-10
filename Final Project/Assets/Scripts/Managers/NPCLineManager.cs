using UnityEngine;
using System;
public class NPCLineManager : MonoBehaviour
{
    public static event Action LineEnterEvent;
    public static event Action LineLeaveEvent;
    public static event Action RegisterReleaseEvent;

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