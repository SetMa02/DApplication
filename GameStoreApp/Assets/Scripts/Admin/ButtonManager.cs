using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private Auth _auth;
    [SerializeField] private CanvasGroup _favButtonCanvasGroup;
    [SerializeField]private CanvasGroup _menuCanvasGroup;
    [SerializeField] private CanvasGroup _addGameCanvasGroup;
    [SerializeField] private CanvasGroup _deleteGameCanvasGroup;
    [SerializeField] private CanvasGroup _editGameCanvasGroup;
    [SerializeField] private CanvasGroup _favouriteCanvasGroup;
    [SerializeField] private CanvasGroup _deleteEmpCanvasCroup;
    [SerializeField] private CanvasGroup _favGameBtn;
    [SerializeField] private CanvasGroup _addEmpBtn;
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
            _favGameBtn.alpha = 0;
            _favGameBtn.interactable = false;
            _favGameBtn.blocksRaycasts = false;
            _favouriteCanvasGroup.alpha = 0;
            _favouriteCanvasGroup.interactable = false;
            _favouriteCanvasGroup.blocksRaycasts = false;
            _favButtonCanvasGroup.alpha = 0;
            _favButtonCanvasGroup.interactable = false;
            _favButtonCanvasGroup.blocksRaycasts = false;
            _menuCanvasGroup.alpha = 1;
            _menuCanvasGroup.interactable = true;
            _menuCanvasGroup.blocksRaycasts = true;
            if (Admin.CanAdd == true)
            {
                OpenPanel(_addGameCanvasGroup);
            }
            else
            {
                ClosePanel(_addGameCanvasGroup);
            }
            
             if (Admin.CanDelete == true)
             {
                 OpenPanel(_deleteGameCanvasGroup);
             }
             else
             {
                 ClosePanel(_deleteGameCanvasGroup);
             }

             if (Admin.CanChange == true)
             {
                 OpenPanel(_editGameCanvasGroup);
             }
             else
             {
                 ClosePanel(_editGameCanvasGroup);
             }

             if (Admin.CanAddEmployee == true)
             {
                 OpenPanel(_addEmpBtn);
             }
             else
             {
                 ClosePanel(_addEmpBtn);
             }

             if (Admin.CanDeleteEmployee == true)
             {
                 OpenPanel(_deleteEmpCanvasCroup);
             }
             else
             {
                 ClosePanel(_deleteEmpCanvasCroup);
             }
        }
    }

    private void OpenPanel(CanvasGroup panel)
    {
       panel.alpha = 1;
       panel.interactable = true;
       panel.blocksRaycasts = true;
    }

    private void ClosePanel(CanvasGroup panel)
    {
        panel.alpha = 0;
        panel.interactable = false;
        panel.blocksRaycasts = false;
    }
}
