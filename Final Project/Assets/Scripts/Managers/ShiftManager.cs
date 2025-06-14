using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShiftManager : MonoBehaviour
{
    [Header("Panels & Buttons:")]
    [SerializeField] GameObject EndOfShiftPanel;
    [SerializeField] Button quit;
    [SerializeField] Button nextDay;

    [Header("Text:")]
    [SerializeField] TextMeshProUGUI earningsText;
    [SerializeField] TextMeshProUGUI servedNPC;
    [SerializeField] TextMeshProUGUI losrNPC;
    [SerializeField] TextMeshProUGUI revenueLost;



    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
