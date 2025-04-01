using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ShiftUI : MonoBehaviour
{
    private ShiftManager shiftManager;
    private ShiftHostsUI shiftHostsUi;

    private Transform dimBackground;
    private Transform shiftTimer;
    private Transform shiftHostsList;

    private void Awake()
    {
        shiftHostsUi = FindFirstObjectByType<ShiftHostsUI>();
        shiftManager = FindFirstObjectByType<ShiftManager>();
        shiftManager.OnShiftStarted += OnShiftStartedHandler;
        InitUI();
    }

    private void InitUI()
    {

        dimBackground = transform.Find("DimBackground");
        if (dimBackground)
        {
            dimBackground.gameObject.SetActive(false);
        }
        shiftTimer = transform.Find("");
        if (shiftTimer != null)
        {
            shiftTimer.gameObject.SetActive(false);
        }
        shiftHostsList = transform.Find("");
        if (shiftHostsList != null)
        {
            shiftHostsList.gameObject.SetActive(false);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
        HideUIComponent(shiftHostsList);
        HideUIComponent(shiftTimer);
        HideUIComponent(dimBackground);
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
