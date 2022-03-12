using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentEmployee : MonoBehaviour
{
    private int _id;
    [SerializeField] private Text _loginText;
    [SerializeField] private Text _passwordText;
    [SerializeField] private Toggle _canAddToggle;
    [SerializeField] private Toggle _canChangeToggle;
    [SerializeField] private Toggle _canDeleteToggle;
    [SerializeField] private Toggle _canAddempToggle;
    [SerializeField] private Toggle _canDeleteEmpToggle;
    [SerializeField] private Button _deleteButton;

    public void ReceiveData(int id,string login, string password, bool canAdd, bool canDelete, bool canChange, bool canAddEmp,
        bool canDeleteEmp)
    {
        _id = id;
        _loginText.text = login;
        _passwordText.text = password;
        _canAddToggle.isOn = canAdd;
        _canChangeToggle.isOn = canChange;
        _canDeleteToggle.isOn = canDelete;
        _canAddempToggle.isOn = canAddEmp;
        _canDeleteEmpToggle.isOn = canDeleteEmp;
    }
}
