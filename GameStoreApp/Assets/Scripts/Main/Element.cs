using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Extensions;
using Firebase.Storage;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Element : MonoBehaviour
{
    
    public int id;
    public RawImage Icon;
    public TMP_Text Name;
    public Button Click;
    public CanvasGroup MainPanel;
    public bool IsFavourite = false;

    private GameObject _window;
    private GameWindow _gameWindow;
    private CanvasGroup _canvasGroup;
    
   
   
    private void Start()
   {
      _window = GameObject.FindGameObjectWithTag("GameWindow");
      _gameWindow = _window.GetComponent<GameWindow>();
      _canvasGroup = _window.GetComponent<CanvasGroup>();
      Click.onClick.AddListener(GameClick);
   }

   private void GameClick()
   {
       Debug.Log("Window opening...");
       MainPanel.alpha = 0.7f;
       MainPanel.interactable = false;
       MainPanel.blocksRaycasts = false;
       
       _gameWindow.ReciveData(Icon, Name.text, 
           Snapshot.DbSnapshot.Child(id.ToString()).Child("Description").Value.ToString(), 
           Snapshot.DbSnapshot.Child(id.ToString()).Child("Platform").Value.ToString(),
           Snapshot.DbSnapshot.Child(id.ToString()).Child("Genres").Value.ToString(),
           Snapshot.DbSnapshot.Child(id.ToString()).Child("Price").Value.ToString(),
           id);

       _canvasGroup.alpha = 1;
       _canvasGroup.interactable = true;
       _canvasGroup.blocksRaycasts = true;

   }
   
}
