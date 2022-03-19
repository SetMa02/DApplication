using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ErrorMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text _errorMessage;
    [SerializeField] private Button _okButton;
    [SerializeField] private CanvasGroup _errorFrame;

    private CanvasGroup _currentFrame;
    
     private CanvasGroup _targetFrame;

    private void Start()
    {
        _okButton.onClick.AddListener(OnOkClick);
    }

    public void ShowMessage(CanvasGroup frame, string text)
    {
        _targetFrame = frame;
        _errorMessage.text = text;
        _errorFrame.blocksRaycasts = true;
        _errorFrame.alpha = 1;
        _targetFrame.alpha = 0.5f;
        _errorFrame.interactable = true;
        _targetFrame.interactable = false;
        _okButton.onClick.AddListener(OnOkClick);
    }

    public void ShowMessageAndClose(CanvasGroup targetFrame,CanvasGroup currentFarame, string text)
    {
        _targetFrame = targetFrame;
        _currentFrame = currentFarame;
        _errorMessage.text = text;
        _errorFrame.blocksRaycasts = true;
        _errorFrame.alpha = 1;
        _currentFrame.alpha = 0.5f;
        _errorFrame.interactable = true;
        _currentFrame.interactable = false;
        _okButton.onClick.AddListener(OkClickAndClose);
    }

    private void OnOkClick()
    {
        _errorFrame.alpha = 0;
        _errorFrame.blocksRaycasts = false;
        _errorFrame.interactable = false;
        _currentFrame.interactable = true;
        _currentFrame.alpha = 1;
    }

    private void OkClickAndClose()
    {
        _errorFrame.alpha = 0;
        _errorFrame.blocksRaycasts = false;
        _errorFrame.interactable = false;
        _currentFrame.interactable = false;
        _currentFrame.blocksRaycasts = false;
        _currentFrame.alpha = 0;
        _targetFrame.alpha = 1;
        _targetFrame.interactable = true;
        _targetFrame.blocksRaycasts = true;

    }
}
