using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class CurrentEmployee : MonoBehaviour
{
    public int Id;
    public Text LoginText;
    public Text PasswordText;
    [SerializeField] private Toggle _canAddToggle;
    [SerializeField] private Toggle _canChangeToggle;
    [SerializeField] private Toggle _canDeleteToggle;
    [SerializeField] private Toggle _canAddempToggle;
    [SerializeField] private Toggle _canDeleteEmpToggle;
    public CanvasGroup _canvas; 

    public void ReceiveData(int id,string login, string password, bool canAdd, bool canDelete, bool canChange, bool canAddEmp,
        bool canDeleteEmp)
    {
        Id = id;
        LoginText.text = login;
        PasswordText.text = password;
        _canAddToggle.isOn = canAdd;
        _canChangeToggle.isOn = canChange;
        _canDeleteToggle.isOn = canDelete;
        _canAddempToggle.isOn = canAddEmp;
        _canDeleteEmpToggle.isOn = canDeleteEmp;
    }

    public void CloseWindow()
    {
        _canvas.alpha = 0;
        _canvas.interactable = false;
        _canvas.blocksRaycasts = false;
    }
}
