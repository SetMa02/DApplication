using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Storage;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ContentUpdate : MonoBehaviour
{
    [SerializeField] private FireBase _fireBase;
    [SerializeField] private Auth _auth;
    [SerializeField] private GameObject _container;
    [SerializeField] private Element _prefab;
    
    private List<Element> _elements = new List<Element>();

    private CanvasGroup _mainFrame;

    private void Start()
    {
        _mainFrame = gameObject.GetComponent<CanvasGroup>();
    }

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
        StartCoroutine(LoadUserData());
        _mainFrame.alpha = 1;
        _mainFrame.interactable = true;
        _mainFrame.blocksRaycasts = true;
    }
    
    private IEnumerator LoadUserData()
    {

        var DBTask = _fireBase.DBreference.Child("Games").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            int countOfGames = Convert.ToInt32(snapshot.Child("Count").Value.ToString());
            for (int i = 0; i < countOfGames; i++)
            {
                _elements.Add(_prefab);
            }
            
            for (int i = 0; i < _elements.Count; i++)
            {
                Instantiate(_elements[i], _container.transform, false);
                string DBElement = (i + 1).ToString();
                _elements[i].Name.text = snapshot.Child(DBElement).Child("Name").Value.ToString();
                _elements[i].id = i+1;
                
                StorageReference image = _fireBase.StorageReference.Child(snapshot.Child(DBElement).Child("Image").Value.ToString());
                image.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
                {
                    if(!task.IsFaulted && !task.IsCanceled)
                    {
                        StartCoroutine(LoadImage(Convert.ToString(task.Result), i));
                    }
                    else
                    {
                        Debug.Log(task.Exception);
                    }
                });
                
            }
        }
    }
    
    IEnumerator LoadImage(string MediaUrl, int el)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl); //Create a request
        yield return request.SendWebRequest(); //Wait for the request to complete
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
           _elements[el].Icon.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
        }
    }
}