using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

public class FireBase : MonoBehaviour
{
    public DependencyStatus dependencyStatus;
    public FirebaseAuth Auth;    
    public FirebaseUser User;
    public DatabaseReference DBreference;

    
    void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }
    
    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
       
        Auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }
}
