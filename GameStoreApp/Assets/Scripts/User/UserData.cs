using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Firebase.Database;
using UnityEngine;
using UnityEngine.Events;

public class UserData : User
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
        var DBTask = _fireBase.DBreference.Child("users").Child(_fireBase.User.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            Login = _fireBase.User.Email;
            DeliveryAddress = "";
            Basket = new List<Game>();
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            Login = _fireBase.User.Email; 
            DeliveryAddress = snapshot.Child("DeliveryAddress").Value.ToString();
            if (snapshot.Child("Basket").Child(0.ToString()).Child("Name").Value == null)
            {
                Basket = new List<Game>();
            }
            else
            {
                for (int i = 0; i <= Basket.Count; i++)
                {
                    Game game = new Game();
                
                    game.Name = snapshot.Child("Basket").Child(i.ToString()).Child("Name").Value.ToString();
                    game.Genres = snapshot.Child("Basket").Child(i.ToString()).Child("Genres").Value.ToString();
                    game.Price = Int32.Parse(snapshot.Child("Basket").Child(i.ToString()).Child("Price").Value.ToString());
                    game.Image = snapshot.Child("Basket").Child(i.ToString()).Child("Image").Value.ToString();
                    game.Platform = snapshot.Child("Basket").Child(i.ToString()).Child("Platform").Value.ToString();
                    game.Tags = snapshot.Child("Basket").Child(i.ToString()).Child("Tags").Value.ToString();
                
                    Basket.Add(game);
                }
            }
            Debug.Log("Data is loaded");
            
        }
    }
    
    

}
