using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddGameButtonClick : MonoBehaviour
{
    [SerializeField] private CanvasGroup _editWindow;
    [SerializeField] private CurrentGame _currentGame;
    private Button _addButton;

    private void Start()
    {
        _addButton = GetComponent<Button>();
        _addButton.onClick.AddListener(AddButtonClick);
        
    }

    private void AddButtonClick()
    {
        LoadGame.LoadGameData(Convert.ToInt32(Snapshot.DbSnapshot.Child("Count").Value), null, 
            "", "","","","");
        
        _currentGame.SetData();
        _editWindow.alpha = 1;
        _editWindow.blocksRaycasts = true;
        _editWindow.interactable = true;

        CurrentGame.IsNewGame = true;
    }
}
