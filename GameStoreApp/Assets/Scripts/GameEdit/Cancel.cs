using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cancel : MonoBehaviour
{
    [SerializeField] private CanvasGroup _closeWindow;
    [SerializeField] private CanvasGroup _targetWindow;

    private Button _cancelButton;
    private void Start()
    {
        _cancelButton = GetComponent<Button>();
        _cancelButton.onClick.AddListener(CancelButtonClick);
    }

    public void CancelButtonClick()
    {
        _closeWindow.alpha = 0;
        _closeWindow.interactable = false;
        _closeWindow.blocksRaycasts = false;

        _targetWindow.alpha = 1;
        _targetWindow.interactable = true;
        _targetWindow.blocksRaycasts = true;
    }
}
