using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class MenuClick : MonoBehaviour

{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private Auth _auth;
    [SerializeField] private CanvasGroup _favButton;

    private CanvasGroup _menuCanvasGroup;
    private bool _isOpen;
    private Button _button;

    private void OnEnable()
    {
        _auth.UserSigned += UserSignCheck;
    }

    private void OnDisable()
    {
        _auth.UserSigned -= UserSignCheck;
    }

    private void UserSignCheck()
    {
        if (Admin.IsAdmin == true)
        {
            _favButton.alpha = 0;
            _favButton.interactable = false;
            _favButton.blocksRaycasts = false;

            _menuCanvasGroup.alpha = 1;
            _menuCanvasGroup.interactable = true;
            _menuCanvasGroup.blocksRaycasts = true;
        }
    }
    

    private void Start()
    {
        _isOpen = false;
        _button = gameObject.GetComponent<Button>();
        _button.onClick.AddListener(BtnClick);
        _isOpen = false;

        _menuCanvasGroup = GetComponent<CanvasGroup>();
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
        _menuPanel.transform.LeanMoveLocal(new Vector3(349, 815.65f), 0.25f);
        _isOpen = true;
    }

    private void CloseMenu()
    {
        transform.LeanRotate(new Vector3(0, 0, 0), 0.25f);
        _menuPanel.transform.LeanMoveLocal(new Vector3(750, 815.65f), 0.25f);
        _isOpen = false;
    }
   
}
