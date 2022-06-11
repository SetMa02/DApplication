using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Firebase.Database;
using UnityEngine;
using UnityEngine.Events;

public class AdminData : Admin
{
    [SerializeField] private FireBase _fireBase;
    [SerializeField] private Auth _auth;

    private void OnEnable()
    {
        _auth.UserSigned += UserIsSigned;
    }
    private void OnDisable()
    {
        _auth.UserSigned -= UserIsSigned;
    }
    private void UserIsSigned()
    {
        StartCoroutine(GetUserData());
    }
    private IEnumerator GetUserData()
    { 
        var DBTask = _fireBase.DBreference.Child("Users").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            Snapshot.UsersSnapshot = DBTask.Result;
            FavouriteGames.Games = new List<int>() { };
            if (Snapshot.UsersSnapshot.Child(_fireBase.User.UserId).Exists)
            {
                int favCount = Convert.ToInt32(Snapshot.UsersSnapshot.Child(_fireBase.User.UserId).Child("Count").Value);
                if (favCount == 0)
                {
                    yield break;
                }
                if (favCount != 0)
                {
                    for (int i = 1; i <= favCount; i++)
                    {
                        int gameId = Convert.ToInt32(Snapshot.UsersSnapshot.Child(_fireBase.User.UserId).Child($"{i}").Value);
                        Debug.Log(gameId);
                        FavouriteGames.Games.Add(gameId);
                    }
                    Debug.Log("User data received!");
                }
            }
            else if (!Snapshot.UsersSnapshot.Child(_fireBase.User.UserId).Exists)
            {
                StartCoroutine(CreateUserData());
            }
        }
    }
    private IEnumerator CreateUserData()
    {
        var DBTask = _fireBase.DBreference.Child("Users").Child(_fireBase.User.UserId).Child("Count").SetValueAsync("0");
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            Debug.Log("User data created!");
            var DBTask1 = _fireBase.DBreference.Child("Users").GetValueAsync();
            yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);
            Snapshot.UsersSnapshot = DBTask1.Result;

        }
    }
}
