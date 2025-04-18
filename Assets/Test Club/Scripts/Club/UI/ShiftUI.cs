using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ShiftUI : MonoBehaviour
{
    [SerializeField]
    private ShiftManager shiftManager;
    private ShiftHostsUI shiftHostsUi;
    [SerializeField]
    private Transform dimBackground;
    [SerializeField]
    private Transform shiftTimer;
    [SerializeField]
    private Transform shiftHostsList;

    private void Awake()
    {
        shiftHostsUi = GetComponentInChildren<ShiftHostsUI>();
        shiftManager.OnShiftStarted += OnShiftStartedHandler;
        HideShiftUI();
    }

    public void OnShiftStartedHandler(List<GameObject> hosts)
    {
        shiftHostsUi.SetHostsForShiftList(hosts);
        DisplayStartShiftUI();
    }

    private void ShowUIComponent(Transform uiComponent)
    {
        if (uiComponent != null && !uiComponent.gameObject.activeSelf)
        {
            uiComponent.gameObject.SetActive(true);
        }
    }

    private void HideUIComponent(Transform uiComponent)
    {
        if (uiComponent != null && uiComponent.gameObject.activeSelf)
        {
            uiComponent.gameObject.SetActive(false);
        }
    }

    public void DisplayStartShiftUI()
    {
        ShowUIComponent(shiftTimer);
        ShowUIComponent(shiftHostsList);
    }

    public void HideShiftUI()
    {
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void ShowDimBackground()
    {
        ShowUIComponent(dimBackground);
    }

    public void HideDimBackground()
    {
        HideUIComponent(dimBackground);
    }

    private void OnDestroy()
    {
        shiftManager.OnShiftStarted -= OnShiftStartedHandler;
    }
}
