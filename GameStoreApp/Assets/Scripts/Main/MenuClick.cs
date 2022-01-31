using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuClick : MonoBehaviour
{
    [SerializeField]private Animator _menuAnim;
    
    private Animator _animator;
    private GameObject _mainPanel;
    private CanvasGroup _mainFrame;
    private bool _isOpen;
    private Button _button;

    private void Start()
    {
        _button = gameObject.GetComponent<Button>();
        _button.onClick.AddListener(btnClick);
        _isOpen = false;
    }

    private void btnClick()
    {
        switch (_isOpen)
        {
            case true:
                OpenMenu();
                break;
            case false:
                CloseMenu();
                break;
        }
    }

    private void OpenMenu()
    {
        _animator.Play("BtnClick");
        _menuAnim.Play("OpenMenu");
        _isOpen = true;
    }

    private void CloseMenu()
    {
        _animator.Play("BtnReverse");
        _menuAnim.Play("CloseMenu");
        _isOpen = false;
    }
   
}
