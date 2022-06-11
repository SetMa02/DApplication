using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminElement : MonoBehaviour
{
    public int Id;
    public string Login;
    public string Password;
    public bool canAdd = false;
    public bool canChange = false;
    public bool canDelete = false;
    public bool CanAddEmp = false;
    public bool canDeleteEmp = false;
    
    [SerializeField]private Text _login;
    [SerializeField]private Text _password;

    private GameObject _currentEmployeeGameObject;
    private CurrentEmployee _currentEmployee;
    private CanvasGroup _currentEmployeePanel;
    private Button _openEmployeeButton;
    private void Start()
    {
        _openEmployeeButton = GetComponent<Button>();
        _openEmployeeButton.onClick.AddListener(EmployeeButtonClick);
        _currentEmployeeGameObject = GameObject.FindGameObjectWithTag("CurrentEmployee");
        _currentEmployee = _currentEmployeeGameObject.GetComponent<CurrentEmployee>();
        _currentEmployeePanel = _currentEmployeeGameObject.GetComponent<CanvasGroup>();
    }
    
    private void EmployeeButtonClick()
    {
        _currentEmployee.ReceiveData(Id, Login, Password, canAdd, canDelete, canChange, CanAddEmp, canDeleteEmp);
        _currentEmployeePanel.alpha = 1;
        _currentEmployeePanel.interactable = true;
        _currentEmployeePanel.blocksRaycasts = true;
    }

    public void ElementCreate()
    {
        _login.text = Login;
        _password.text = Password;
    }
}
