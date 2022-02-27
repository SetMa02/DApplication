using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cancel : MonoBehaviour
{
    [SerializeField] private CanvasGroup _editWindow;

    private Button _cancelButton;
    private void Start()
    {
        _cancelButton = GetComponent<Button>();
        _cancelButton.onClick.AddListener(CancelButtonClick);
    }

    public void CancelButtonClick()
    {
        _editWindow.alpha = 0;
        _editWindow.interactable = false;
        _editWindow.blocksRaycasts = false;
    }
}
