using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logout : MonoBehaviour
{
    [SerializeField] private CanvasGroup _pageAuth;
    [SerializeField] private CanvasGroup _pageMain;
    [SerializeField] private FireBase _fireBase;

    public void LogoutButtonClick()
    {
        Application.LoadLevel(0);
    }
}
