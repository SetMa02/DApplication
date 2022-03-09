using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeClick : MonoBehaviour
{
    [SerializeField] private Button _empButton;
    [SerializeField] private CanvasGroup _empCanvasGroup;
    [SerializeField] private CanvasGroup _mainPanelCanvasGroup;
    [SerializeField] private MenuClick _menuClick;

    private void Start()
    {
        _empButton = GetComponent<Button>();
        _empButton.onClick.AddListener(OnEmployeeClick);
    }

    public void Cancel()
    {
        _empCanvasGroup.alpha = 0;
        _empCanvasGroup.interactable = false;
        _empCanvasGroup.blocksRaycasts = false;

        _mainPanelCanvasGroup.alpha = 1;
        _mainPanelCanvasGroup.interactable = true;
    }
    
    private void OnEmployeeClick()
    {
        _menuClick.CloseMenu();

        _empCanvasGroup.alpha = 1;
        _empCanvasGroup.interactable = true;
        _empCanvasGroup.blocksRaycasts = true;

        _mainPanelCanvasGroup.alpha = 0.5f;
        _mainPanelCanvasGroup.interactable = false;
    }

    
}
