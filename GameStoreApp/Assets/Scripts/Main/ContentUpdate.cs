using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using DefaultNamespace;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Storage;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class ContentUpdate : MonoBehaviour
{
    public List<GameObject> Elements;
    public static StorageReference gsReference;

    [SerializeField] private FireBase _fireBase;
    [SerializeField] private Auth _auth;
    [SerializeField] private GameObject _container;
    [SerializeField] private Element _prefab;

    private FirebaseStorage _storage;
    private StorageReference _storageReference;
    private  StorageReference _fileReference; 
    private CanvasGroup _mainFrame;
  
    private void Start()
    {
       
        Elements = new List<GameObject>() { };
        _mainFrame = gameObject.GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        ClearChildren();
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
    
    public IEnumerator LoadUserData()
    {
        ClearChildren();
        string name = "error";
        var DBTask = _fireBase.DBreference.Child("Games").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            Snapshot.DbSnapshot = snapshot;
            
            int countOfGames = Convert.ToInt32(snapshot.ChildrenCount);
            Snapshot.GamesCount = countOfGames;
            
            for (int i = 0; i < countOfGames; i++)
            {
                Elements.Add(_prefab.gameObject);
            }
            
            for (int i = 0; i < Elements.Count; i++)
            {
                Element element = Elements[i].GetComponent<Element>();
                string DBElement = (i + 1).ToString();
                element.MainPanel = _mainFrame;
                name = snapshot.Child(DBElement).Child("Name").Value.ToString();
               
                gsReference =
                    _fireBase.Storage.GetReferenceFromUrl("gs://diplomapplication-a861f.appspot.com/" + snapshot.Child(DBElement).Child("Image").Value);
                
                StartCoroutine(SendLoadRequest(gsReference, i, name, element));
            }
        }
    }

    public  IEnumerator SendLoadRequest( StorageReference reference, int i, string name, Element element)
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
        StartCoroutine(LoadImage(url, i, name, element));
    }
    
    
    public IEnumerator LoadImage(string URL, int i, string name, Element element)
    {
        Debug.Log("Starting load image...");
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(URL);
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            element.Name.text = name;
            element.Icon.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
            element.id = i+1;
            if(FavouriteGames.Games.Contains( element.id))
            {
                element.IsFavourite = true;
                Debug.Log( element.id + " fav game");
            }
            else
            {
                element.IsFavourite = false;
            }
            Debug.Log("Done");
            Instantiate(Elements[i], _container.transform, false);
        }
    }
    
    private void ClearChildren()
    {
        int i = 0;

        GameObject[] allChildren = new GameObject[_container.transform.childCount];

        foreach (Transform child in _container.transform)
        {
            allChildren[i] = child.gameObject;
            i += 1;
        }
        
        foreach (GameObject child in allChildren)
        {
            DestroyImmediate(child.gameObject);
        }
    }
    
}
    
