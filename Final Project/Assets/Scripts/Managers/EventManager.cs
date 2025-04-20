using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action LineEnterEvent;
    public static event Action LineLeaveEvent;
    public static event Action RegisterRealeseEvent;

    public static void EnterLine()
    {
        LineEnterEvent?.Invoke();
    }

    public static void LeaveLine()
    {
        LineLeaveEvent?.Invoke();
    }

    public static void RegisterRealese()
    {
        RegisterRealeseEvent?.Invoke();
    }
}
