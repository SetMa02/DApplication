using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Storage;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FavouryContentUpdater : MonoBehaviour
{
    [SerializeField] private Button _favButton;
    [SerializeField] private GameObject _container;
    [SerializeField] private FireBase _fireBase;
    [SerializeField] private Element _prefab;
    [SerializeField] private CanvasGroup _mainFrame;
    [SerializeField] private ContentUpdate _content;
    [SerializeField] private GameObject _emailButton;

    private List<Element> _currentGames = new List<Element>();

    private bool _isPressed;
    private void Start()
    {
        _isPressed = false;
        _favButton.image.color = Color.white;
        _favButton.onClick.AddListener(btnClick);
    }

    private void btnClick()
    {
        ClearChildren();
        if (_isPressed == true)
        {
            ShowAll();
            _isPressed = false;
            _emailButton.SetActive(false);
        }   
        else if (_isPressed == false)
        {
            ShowFavourite();
            _isPressed = true;
            _emailButton.SetActive(true);
        }
    }

    private void ShowFavourite()
    {
        StartCoroutine(LoadFavourites());
        _favButton.image.color = Color.yellow;
       
    }

    private void ShowAll()
    {
        StartCoroutine(_content.LoadUserData());
        _favButton.image.color = Color.white;
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

    private IEnumerator LoadFavourites()
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

            int countOfGames = Convert.ToInt32(snapshot.ChildrenCount);
            for (int i = 0; i < countOfGames; i++)
            {
                _currentGames.Add(_prefab);
            }
            
            for (int i = 0; i < _currentGames.Count; i++)
            {
                string DBElement = (i + 1).ToString();
                _currentGames[i].MainPanel = _mainFrame;
                name = snapshot.Child(DBElement).Child("Name").Value.ToString();
               
                StorageReference gsReference =
                    _fireBase.Storage.GetReferenceFromUrl("gs://diplomapplication-a861f.appspot.com/" + snapshot.Child(DBElement).Child("Image").Value);
                
                StartCoroutine(SendLoadRequest(gsReference, i, name));
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
    
    
        IEnumerator LoadImage(string URL, int i, string name)
        {
            Debug.Log("Starting load image...");
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(URL);
            yield return request.SendWebRequest();
            if (request.isDone)
            {
                _currentGames[i].Name.text = name;
                _currentGames[i].Icon.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
                _currentGames[i].id = i+1;
                if(FavouriteGames.Games.Contains( _currentGames[i].id))
                {
                    _currentGames[i].IsFavourite = true;
                    Debug.Log( _currentGames[i].id + " fav game");
                }
                else
                {
                    _currentGames[i].IsFavourite = false;
                }
                Debug.Log("Done");

                if ( _currentGames[i].IsFavourite == true)
                {
                    Instantiate( _currentGames[i], _container.transform, false);
                }
            }
        }
    }
    
}
