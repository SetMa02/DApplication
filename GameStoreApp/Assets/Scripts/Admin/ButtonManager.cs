using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private Auth _auth;
    [SerializeField] private CanvasGroup _favButtonCanvasGroup;
    [SerializeField]private CanvasGroup _menuCanvasGroup;
    [SerializeField] private CanvasGroup _addGameCanvasGroup;
    [SerializeField] private CanvasGroup _employeesCanvasGroup;
    [SerializeField] private CanvasGroup _deleteGameCanvasGroup;
    [SerializeField] private CanvasGroup _editGameCanvasGroup;
    
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
            _favButtonCanvasGroup.alpha = 0;
            _favButtonCanvasGroup.interactable = false;
            _favButtonCanvasGroup.blocksRaycasts = false;

            _menuCanvasGroup.alpha = 1;
            _menuCanvasGroup.interactable = true;
            _menuCanvasGroup.blocksRaycasts = true;

            if (Admin.CanAdd == true)
            {
                _addGameCanvasGroup.alpha = 1;
                _addGameCanvasGroup.interactable = true;
                _addGameCanvasGroup.blocksRaycasts = true;
            }

             if (Admin.CanAddEmployee == true)
            {
                _employeesCanvasGroup.alpha = 1;
                _employeesCanvasGroup.interactable = true;
                _employeesCanvasGroup.blocksRaycasts = true;
            }

             if (Admin.CanDelete == true)
             {
                 _deleteGameCanvasGroup.alpha = 1;
                 _deleteGameCanvasGroup.interactable = true;
                 _deleteGameCanvasGroup.blocksRaycasts = true;
             }

             if (Admin.CanChange == true)
             {
                 _editGameCanvasGroup.alpha = 1;
                 _editGameCanvasGroup.interactable = true;
                 _editGameCanvasGroup.blocksRaycasts = true;
             }
        }
    }
}
