using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Admin;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;

public class SaveEmployee : MonoBehaviour
{
    [SerializeField] private FireBase _fireBase;
    [SerializeField] private InputField _logiInput;
    [SerializeField] private InputField _passwordInput;
    [SerializeField] private Toggle _canAddToggle;
    [SerializeField] private Toggle _canChangeToggle;
    [SerializeField] private Toggle _canDeleteToggle;
    [SerializeField] private Toggle _canAddempToggle;
    [SerializeField] private Toggle _canDeleteEmpToggle;
    [SerializeField] private Button _saveButton;
    [SerializeField] private ErrorMessage _warningMessage;
    [SerializeField] private CanvasGroup _canvas;
    [SerializeField] private CanvasGroup _employeeCanvas;
    [SerializeField] private EmployeeContentUpdater _contentUpdater;
    private void Start()
    {
        _saveButton.onClick.AddListener(CreateButtonClick);
    }

    private void CreateButtonClick()
    {
        StartCoroutine(AddEmployee());

    }

    private IEnumerator AddEmployee()
    {
       
        
        var RegisterTask = _fireBase.Auth.CreateUserWithEmailAndPasswordAsync(_logiInput.text, _passwordInput.text);

        yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

        if (RegisterTask.IsCompleted)
        {

            var LoginTask = _fireBase.Auth.SignInWithEmailAndPasswordAsync(_logiInput.text,_passwordInput.text);
       
            yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);
            
            if (LoginTask.IsCompleted)
            {
                Debug.Log("!!!");
                var dbStatus = _fireBase.DBreference.Child("Admins").GetValueAsync();
                
                yield return new WaitUntil(predicate: () => dbStatus.IsCompleted);
                if (dbStatus.IsCompleted)
                {
                    Debug.Log("Saving Started");
                    DataSnapshot snapshot = dbStatus.Result;
                    var newUserId = _fireBase.DBreference.Child("Admins").Child((snapshot.ChildrenCount + 1).ToString())
                        .Child("UserId").SetValueAsync(_fireBase.User.UserId);
                    
                    var newUserLogin = _fireBase.DBreference.Child("Admins").Child((snapshot.ChildrenCount + 1).ToString())
                        .Child("Login").SetValueAsync(_logiInput.text);
                    
                    var newUserPassword = _fireBase.DBreference.Child("Admins").Child((snapshot.ChildrenCount + 1).ToString())
                        .Child("Password").SetValueAsync(_passwordInput.text);
                    
                    var addGames = _fireBase.DBreference.Child("Admins").Child((snapshot.ChildrenCount + 1).ToString())
                        .Child("CanAdd").SetValueAsync(_canAddToggle.isOn);
                    
                    var ChangeGames = _fireBase.DBreference.Child("Admins").Child((snapshot.ChildrenCount + 1).ToString())
                        .Child("CanChange").SetValueAsync(_canChangeToggle.isOn);
                    
                    var DeleteGames = _fireBase.DBreference.Child("Admins").Child((snapshot.ChildrenCount + 1).ToString())
                        .Child("CanDelete").SetValueAsync(_canDeleteToggle.isOn);
                    
                    var addEmployee = _fireBase.DBreference.Child("Admins").Child((snapshot.ChildrenCount + 1).ToString())
                        .Child("CanAddEmployee").SetValueAsync(_canAddempToggle.isOn);
                    
                    var DeleteEmployee = _fireBase.DBreference.Child("Admins").Child((snapshot.ChildrenCount + 1).ToString())
                        .Child("CanDeleteEmployee").SetValueAsync(_canDeleteEmpToggle.isOn);

                    yield return new WaitUntil(predicate: () =>
                        newUserId.IsCompleted && newUserLogin.IsCompleted && newUserPassword.IsCompleted &&
                        addGames.IsCompleted && addEmployee.IsCompleted && ChangeGames.IsCompleted &&
                        DeleteGames.IsCompleted && DeleteEmployee.IsCompleted);

                    if (newUserId.IsCompleted && newUserLogin.IsCompleted && newUserPassword.IsCompleted &&
                        addGames.IsCompleted && addEmployee.IsCompleted && ChangeGames.IsCompleted &&
                        DeleteGames.IsCompleted && DeleteEmployee.IsCompleted)
                    {  
                        var returnTask = _fireBase.Auth.SignInWithEmailAndPasswordAsync(Admin.Login,Admin.Password);
       
                        yield return new WaitUntil(predicate: () => returnTask.IsCompleted);

                        if (returnTask.IsCompleted)
                        {
                            _canvas.alpha = 0;
                            _canvas.interactable = false;
                            _canvas.blocksRaycasts = false;
                            StartCoroutine(_contentUpdater.LoadEmployees());
                        }
                    }

                }
              
            }
        }

    }
}
