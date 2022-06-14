using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Auth : MonoBehaviour
{
    [SerializeField] private FireBase _fireBase;
    [SerializeField] private TMP_InputField _login;
    [SerializeField] private TMP_InputField _password;
    [SerializeField] private Button _loginButton;
    [SerializeField] private GameObject _registr;
    [SerializeField] private ErrorMessage _errorMessage;
    [SerializeField] private TMP_Text _loginText;

    public event UnityAction UserSigned;
    public bool IsAdmin = false;

    private CanvasGroup _frame;
    private Button _regButton;


    private void Start()
    {
        _frame = GetComponent<CanvasGroup>();
        _loginButton.onClick.AddListener(LoginButton);
        _regButton = _registr.GetComponent<Button>();
        _regButton.onClick.AddListener(MoveToRegistration);
    }

    public void LoginButton()
    {
        StartCoroutine(Login(_login.text, _password.text));
    }

    public void MoveToRegistration()
    {
        _loginText.text = "Регистрация";
        var regText = _registr.GetComponent<TMP_Text>();
        regText.text = "<--Назад";
        _login.text = "";
        _password.text = "";
        _regButton.onClick.RemoveAllListeners();
        _regButton.onClick.AddListener(MoveToLogin);
        _loginButton.onClick.RemoveAllListeners();
        _loginButton.onClick.AddListener(RegistrButtonClick);
    }

    public void MoveToLogin()
    {
        _loginText.text = "Авторизация";
        var regText = _registr.GetComponent<TMP_Text>();
        regText.text = "Зарегестрироваться";
        _login.text = "";
        _password.text = "";
        _loginButton.onClick.RemoveAllListeners();
        _regButton.onClick.RemoveAllListeners();
        _regButton.onClick.AddListener(MoveToRegistration);
        _loginButton.onClick.AddListener(LoginButton);
    }

    private void RegistrButtonClick()
    {
        StartCoroutine(Register(_login.text, _password.text));
    }


    private IEnumerator Login(string _email, string _password)
    {
        var LoginTask = _fireBase.Auth.SignInWithEmailAndPasswordAsync(_email, _password);

        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError) firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Введите логин";
                    break;
                case AuthError.MissingPassword:
                    message = "Введите пароль";
                    break;
                case AuthError.WrongPassword:
                    message = "Неверный пароль";
                    break;
                case AuthError.InvalidEmail:
                    message = "Неверый логин";
                    break;
                case AuthError.UserNotFound:
                    message = "Такого аккаунта не существует";
                    break;
            }

            _errorMessage.ShowMessage(_frame, message);
        }
        else
        {
            _fireBase.User = LoginTask.Result;
            _frame.alpha = 0;
            _frame.interactable = false;
            _frame.blocksRaycasts = false;

            var DBTask = _fireBase.DBreference.Child("Admins").GetValueAsync();

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            DataSnapshot snapshot = DBTask.Result;

            Admin.Login = _login.text;
            Admin.Password = this._password.text;

            for (int i = 1; i <= snapshot.ChildrenCount; i++)
            {
                if (snapshot.Child(i.ToString()).Child("UserId").Value.ToString() == _fireBase.User.UserId)
                {
                    Admin.IsAdmin = true;
                    Admin.Id = i;
                    Debug.Log("User signed as ADMIN");

                    if (snapshot.Child(i.ToString()).Child("CanChange").Value.Equals(true))
                    {
                        Admin.CanChange = true;
                        Debug.Log("change");
                    }

                    if (snapshot.Child(i.ToString()).Child("CanAdd").Value.Equals(true))
                    {
                        Admin.CanAdd = true;
                        Debug.Log("add");
                    }

                    if (snapshot.Child(i.ToString()).Child("CanDelete").Value.Equals(true))
                    {
                        Admin.CanDelete = true;
                        Debug.Log("delete");
                    }

                    if (snapshot.Child(i.ToString()).Child("CanAddEmployee").Value.Equals(true))
                    {
                        Admin.CanAddEmployee = true;
                        Debug.Log("empAdd");
                    }

                    if (snapshot.Child(i.ToString()).Child("CanDeleteEmployee").Value.Equals(true))
                    {
                        Admin.CanDeleteEmployee = true;
                        Debug.Log("EmpDelete");
                    }
                }
            }

            Debug.LogFormat("User signed in successfully: {0} ", _fireBase.User.Email);

            UserSigned?.Invoke();
        }
    }




    private IEnumerator Register(string _email, string _password)
    {
        var RegisterTask = _fireBase.Auth.CreateUserWithEmailAndPasswordAsync(_email, _password);

        yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

        if (RegisterTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
            FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError) firebaseEx.ErrorCode;

            string message = "Register Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WeakPassword:
                    message = "Weak Password";
                    break;
                case AuthError.EmailAlreadyInUse:
                    message = "Email Already In Use";
                    break;
            }

            _errorMessage.ShowMessage(_frame, message);
        }
        else
        {
            _fireBase.User = RegisterTask.Result;
            MoveToLogin();
        }
    }
}