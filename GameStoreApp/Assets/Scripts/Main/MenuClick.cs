using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuClick : MonoBehaviour

{
    [SerializeField] private GameObject _menuPanel;
    
    private bool _isOpen;
    private Button _button;

    private void Start()
    {
        _isOpen = false;
        _button = gameObject.GetComponent<Button>();
        _button.onClick.AddListener(BtnClick);
        _isOpen = false;
    }

    private void BtnClick()
    {
        if (_isOpen == false)
        {
            OpenMenu();
            return;
        }
        else if(_isOpen == true)
        {
            CloseMenu();
            return;
        }
    }

    private void OpenMenu()
    {
        transform.LeanRotate(new Vector3(0, 0, -90), 0.25f);
        _menuPanel.transform.LeanMoveLocal(new Vector3(349, -816), 0.25f);
        _isOpen = true;
    }

    private void CloseMenu()
    {
        transform.LeanRotate(new Vector3(0, 0, 0), 0.25f);
        _menuPanel.transform.LeanMoveLocal(new Vector3(750, -816), 0.25f);
        _isOpen = false;
    }
   
}
