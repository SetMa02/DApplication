using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Storage;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ContentUpdate : MonoBehaviour
{
    [SerializeField] private FireBase _fireBase;
    [SerializeField] private Auth _auth;
    [SerializeField] private GameObject _container;
    [SerializeField] private Element _prefab;
    
    private List<Element> _elements = new List<Element>();
    private FirebaseStorage _storage;
    private StorageReference _storageReference;
    private  StorageReference _fileReference; 
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
                string DBElement = (i + 1).ToString();
                
                _elements[i].Name.text = snapshot.Child(DBElement).Child("Name").Value.ToString();
                _elements[i].id = i+1;
                
                StorageReference gsReference =
                    _fireBase.Storage.GetReferenceFromUrl("gs://diplomapplication-a861f.appspot.com/" + snapshot.Child(DBElement).Child("Image").Value);
                
                StartCoroutine(SendLoadRequest(gsReference, i));
            }
        }
    }

    IEnumerator SendLoadRequest( StorageReference reference, int i)
    {
        string url = "null";
        bool isReady = false;
        reference.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                url = task.Result.ToString();
                isReady = true;
                Debug.Log("Download URL: " + url);
            }
        });
        yield return new WaitUntil(() => isReady == true);
        StartCoroutine(LoadImage(url, i));
    }
    
    
    public IEnumerator LoadImage(string URL, int i)
    {
        Debug.Log("Starting load image...");
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(URL);
        yield return request.SendWebRequest();
        if (request.isDone)
        { 
            _elements[i].Icon.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
            Debug.Log("Done");
            Instantiate(_elements[i], _container.transform, false);
        }
    }
}
    
