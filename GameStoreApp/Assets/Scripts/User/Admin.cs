using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Admin : MonoBehaviour
{
    public static int Id;
    public static string Login;
    public static string Password;
    public static bool IsAdmin = false;
    public static bool CanAdd = false;
    public static bool CanChange = false;
    public static bool CanDelete = false;
    public static bool CanAddEmployee = false;
    public static bool CanDeleteEmployee = false;

    public static void DisableRights()
    { 
        IsAdmin = false;
      CanAdd = false;
      CanChange = false;
      CanDelete = false;
      CanAddEmployee = false;
      CanDeleteEmployee = false;
    }
}
