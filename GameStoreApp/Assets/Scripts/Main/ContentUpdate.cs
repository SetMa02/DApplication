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
    [SerializeField] private GameObject _prefab;
    
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
            
            
            for (int i = 0; i < Convert.ToInt32(snapshot.Child("Count").Value.ToString()); i++)
            {
                Instantiate(_prefab, _container.transform, false);
            }
            
            for (int i = 1; i < Int32.Parse(snapshot.Child("Count").Value.ToString()); i++)
            {
                _elements[i - 1].id = i;
                _elements[i - 1].Name.text = snapshot.Child(i.ToString()).Child("Name").Value.ToString();
                
                StorageReference image = _fireBase.StorageReference.Child(snapshot.Child(i.ToString()).Child("Image").Value.ToString());
                image.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
                {
                    if(!task.IsFaulted && !task.IsCanceled)
                    {
                        StartCoroutine(LoadImage(Convert.ToString(task.Result), i-1));
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