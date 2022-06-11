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

public class Serch : MonoBehaviour
{
    [SerializeField] private GameObject _container;
    [SerializeField] private FireBase _fireBase;
    [SerializeField] private CanvasGroup _mainFrame;
    [SerializeField]private InputField _inputField;
    [SerializeField] private ContentUpdate _contentUpdate;
    [SerializeField] private Element _prefab;
    
    public static StorageReference gsReference;
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
        {;
            StartCoroutine(SendLoadRequest());
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
    
    IEnumerator SendLoadRequest()
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
                _elements.Add(_prefab);
            }
            
            for (int i = 0; i < _elements.Count; i++)
            {
                if (snapshot.Child($"{i+1}").Child("Name").Value.ToString().Contains(_inputField.text))
                {
                    Element element = _elements[i].GetComponent<Element>();
                    string DBElement = (i + 1).ToString();
                    element.MainPanel = _mainFrame;
                    name = snapshot.Child(DBElement).Child("Name").Value.ToString();
               
                    gsReference =
                        _fireBase.Storage.GetReferenceFromUrl("gs://diplomapplication-a861f.appspot.com/" + snapshot.Child(DBElement).Child("Image").Value);
                
                    StartCoroutine(_contentUpdate.SendLoadRequest(gsReference, i, name, element));
                }
            }
        }
    }
}
