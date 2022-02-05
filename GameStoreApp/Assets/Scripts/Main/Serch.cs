using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Firebase.Extensions;
using Firebase.Storage;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Serch : MonoBehaviour
{
    [SerializeField] private GameObject _container;
    [SerializeField] private CanvasGroup _mainFrame;
    [SerializeField]private InputField _inputField;
    [SerializeField] private ContentUpdate _contentUpdate;
    [SerializeField] private Element _prefab;

    private List<Element> _elements = new List<Element>();

    private void Start()
    {
        _inputField.onEndEdit.AddListener(delegate {SerchGames();});
    }

    private void SerchGames()
    {   
        ClearChildren();
        
        if (_inputField.text == "")
        {
            StartCoroutine(_contentUpdate.LoadUserData());
        }
        else
        {
            for (int i = 0; i < Snapshot.GamesCount; i++)
            {
                _elements.Add(_prefab);
                string name = Snapshot.DbSnapshot.Child($"{i+1}").Child("Name").Value.ToString();
                Debug.Log(name);
                if (name.Contains(_inputField.text))
                {
                    Debug.Log("Founded");
                    string DBElement = (i+1).ToString();
                    _elements[i].MainPanel = _mainFrame;
                    name = Snapshot.DbSnapshot.Child(DBElement).Child("Name").Value.ToString();
                    
                    StartCoroutine(SendLoadRequest(ContentUpdate.gsReference, i, name));
                    
                }
            }
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
    
    
    IEnumerator SendLoadRequest( StorageReference reference, int i, string name)
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
        StartCoroutine(LoadImage(url, i, name));
    }
    
    
    public IEnumerator LoadImage(string URL, int i, string name)
    {
        Debug.Log("Starting load image...");
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(URL);
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            _elements[i].Name.text = name;
            _elements[i].Icon.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
            _elements[i].id = i+1;
            if(FavouriteGames.Games.Contains( _elements[i].id))
            {
                _elements[i].IsFavourite = true;
                Debug.Log( _elements[i].id + " fav game");
            }
            else
            {
                _elements[i].IsFavourite = false;
            }
            Debug.Log("Done");
            Instantiate( _elements[i], _container.transform, false);
        }
    }
}
