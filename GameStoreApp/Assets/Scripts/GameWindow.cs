using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameWindow : MonoBehaviour
{
    [SerializeField]private RawImage _icon;
    [SerializeField]private TMP_InputField _name;
    [SerializeField]private Text _description;
    [SerializeField]private Button _starBtn;
    [SerializeField]private Button _backBtn;

    private GameObject _mainPanel;
    private CanvasGroup _mainFrame;
    private CanvasGroup _gameFrame;

    private void Start()
    {
        _gameFrame = gameObject.GetComponent<CanvasGroup>();
        _mainPanel = GameObject.FindGameObjectWithTag("MainFrame");
        _mainFrame = _mainPanel.GetComponent<CanvasGroup>();
        
        _backBtn.onClick.AddListener(CloseWindow);
    }

    public void CloseWindow()
    {
        _gameFrame.alpha = 0;
        _gameFrame.interactable = false;
        _gameFrame.blocksRaycasts = false;

        _mainFrame.alpha = 1;
        _mainFrame.interactable = true;
        _mainFrame.blocksRaycasts = true;
    }
    public void ReciveData(RawImage icon, string name, string description, string platform, string genre, string price)
    {
        Debug.Log("Receiving data... ");
        string fullDescription = $"Цена: {price}\n" +
                                 $"Жанр: {genre} \n" +
                                 $"Платформа {platform}\n" +
                                 $"{description}";
        _icon.texture = icon.texture;
        _name.text = name;
        _name.readOnly = true;
        _description.text = fullDescription;
    }
    
    
}
