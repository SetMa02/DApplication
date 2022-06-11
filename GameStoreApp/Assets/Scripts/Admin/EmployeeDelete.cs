using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Admin;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeDelete : MonoBehaviour
{
    [SerializeField] private FireBase _fireBase;
    [SerializeField] private CurrentEmployee _currentEmployee;
    [SerializeField] private ErrorMessage _message;
    [SerializeField] private EmployeeContentUpdater _content;
    private Button _deleteButton;
    private void Start()
    {
        _deleteButton = GetComponent<Button>();
        _deleteButton.onClick.AddListener(DeleteButtonClick);
    }
    private void DeleteButtonClick()
    {
        StartCoroutine(DeleteCurrentEmployee());
        Debug.Log("!!!");
    }
    private IEnumerator DeleteCurrentEmployee()
    {
        var loginTAsk =
            _fireBase.Auth.SignInWithEmailAndPasswordAsync(_currentEmployee.LoginText.text,
                _currentEmployee.PasswordText.text);
        yield return new WaitUntil(predicate: () => loginTAsk.IsCompleted);
        if (loginTAsk.IsCompleted)
        {
            Debug.Log("!!!");
            int deleteId = _currentEmployee.Id;
            var deleteEmployeeData = _fireBase.DBreference.Child("Admins").Child(deleteId.ToString())
                .RemoveValueAsync();
            var dataTask = _fireBase.DBreference.Child("Admins").GetValueAsync();
            
            yield return new WaitUntil(predicate: () => deleteEmployeeData.IsCompleted && dataTask.IsCompleted);

            if (deleteEmployeeData.IsCompleted && dataTask.IsCompleted)
            {
                Debug.Log("!!!");
                DataSnapshot snapshot = dataTask.Result;
                int id;
                string login;
                string password;
                string canAdd;
                string UserId;
                string canChange;
                string canDelete;
                string canAddEmp;
                string canDeleteEmp;
                for (int i = (deleteId+1); i <= snapshot.ChildrenCount; i++)
                {
                    id = i;
                    login = snapshot.Child(i.ToString()).Child("Login").Value.ToString();
                    password = snapshot.Child(i.ToString()).Child("Password").Value.ToString();
                    UserId = snapshot.Child(i.ToString()).Child("UserId").Value.ToString();
                    canAdd = snapshot.Child(i.ToString()).Child("CanAdd").Value.ToString();
                    canChange = snapshot.Child(i.ToString()).Child("CanChange").Value.ToString();
                    canDelete = snapshot.Child(i.ToString()).Child("CanDelete").Value.ToString();
                    canAddEmp = snapshot.Child(i.ToString()).Child("CanAddEmployee").Value.ToString();
                    canDeleteEmp = snapshot.Child(i.ToString()).Child("CanDeleteEmployee").Value.ToString();
                    id--;
                    var DbTaskLogin = _fireBase.DBreference.Child("Admins").Child(id.ToString()).Child("Login")
                        .SetValueAsync(login);
                    var DbTaskPassword = _fireBase.DBreference.Child("Admins").Child(id.ToString()).Child("Password")
                        .SetValueAsync(password);
                    var DbTaskAdd = _fireBase.DBreference.Child("Admins").Child(id.ToString()).Child("CanAdd")
                        .SetValueAsync(canAdd);
                    var DbTaskChange = _fireBase.DBreference.Child("Admins").Child(id.ToString()).Child("CanChange")
                        .SetValueAsync(canChange);
                    var DbTaskDelete = _fireBase.DBreference.Child("Admins").Child(id.ToString()).Child("CanDelete")
                        .SetValueAsync(canDelete);
                    var DbTaskAddEmp = _fireBase.DBreference.Child("Admins").Child(id.ToString()).Child("CanAddEmployee")
                        .SetValueAsync(canAddEmp);
                    var DbTaskDeleteEmp= _fireBase.DBreference.Child("Admins").Child(id.ToString()).Child("CanDeleteEmployee")
                        .SetValueAsync(canDeleteEmp);
                    var DbTaskUserId = _fireBase.DBreference.Child("Admins").Child(id.ToString()).Child("UserId")
                        .SetValueAsync(UserId);
                    var DbRemove = _fireBase.DBreference.Child("Admins").Child((id+1).ToString()).RemoveValueAsync();

                    yield return new WaitUntil(predicate: () => DbTaskLogin.IsCompleted && DbTaskPassword.IsCompleted &&
                                                                DbTaskAdd.IsCompleted && DbTaskChange.IsCompleted
                                                                && DbTaskDelete.IsCompleted && DbTaskAddEmp.IsCompleted
                                                                && DbRemove.IsCompleted &&
                                                                DbTaskDeleteEmp.IsCompleted &&
                                                                DbTaskUserId.IsCompleted);
                }
                Firebase.Auth.FirebaseUser user = _fireBase.Auth.CurrentUser;
                if (user != null) {
                    user.DeleteAsync().ContinueWith(task => {
                        if (task.IsCanceled) {
                            Debug.LogError("DeleteAsync was canceled.");
                            return;
                        }
                        if (task.IsFaulted) {
                            Debug.LogError("DeleteAsync encountered an error: " + task.Exception);
                            return;
                        }
                        Debug.Log("User deleted successfully.");
                        
                    });
                }
                
                var ReturnTask = _fireBase.Auth.SignInWithEmailAndPasswordAsync(Admin.Login, Admin.Password);

                yield return new WaitUntil(predicate: () => ReturnTask.IsCompleted);
                if (ReturnTask.IsCompleted)
                {
                    _message.ShowMessage(_currentEmployee._canvas, "Пользователь удалён");
                    _currentEmployee.CloseWindow();
                    StartCoroutine(_content.LoadEmployees());
                }
            }
        }
    }
}
