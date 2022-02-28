using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using DefaultNamespace;
using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class GameWindow : MonoBehaviour
{
    [SerializeField] private RawImage _icon;
    [SerializeField] private TMP_InputField _name;
    [SerializeField] private Text _description;
    [SerializeField] private Button _starBtn;
    [SerializeField] private Button _backBtn;
    [SerializeField] private FireBase _fireBase;
    [SerializeField] private ContentUpdate _contentUpdate;
    [SerializeField] private Text _favText;
    [SerializeField] private Button _editButton;
    [SerializeField] private CanvasGroup _editWindow;

    public CurrentGame CurrentGame;
    private GameObject _mainPanel;
    private CanvasGroup _mainFrame;
    private CanvasGroup _gameFrame;
    private int _id;
    private string _desc;
    private bool isFavourite = false;
    private string _genre;
    private string _platform;
    private string _price;

    private void Start()
    {
        _editButton.onClick.AddListener(EditButtonClick);
        _gameFrame = gameObject.GetComponent<CanvasGroup>();
        _mainPanel = GameObject.FindGameObjectWithTag("MainFrame");
        _mainFrame = _mainPanel.GetComponent<CanvasGroup>();

        _backBtn.onClick.AddListener(CloseWindow);
        _starBtn.onClick.AddListener(StarBtnClick);
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

    public void ReciveData(RawImage icon, string name, string description, string platform, string genre, string price,
        int id)
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
        _desc = description;
        _genre = genre;
        _price = price;
        _platform = platform;
        _id = id;

        if (FavouriteGames.Games.Contains(id))
        {
            isFavourite = true;
            _favText.text = "Убрать избранные";
        }
        else
        {
            isFavourite = false;
            _favText.text = "В избранные";
        }

        if (isFavourite == true)
        {
            _starBtn.image.color = Color.yellow;
        }
        else if (isFavourite == false)
        {
            _starBtn.image.color = Color.white;
        }
    }

    private void EditButtonClick()
    {
        global::CurrentGame.IsNewGame = false;
        LoadGame.LoadGameData(_id,_icon, _name.text, _desc, _genre, _price, _platform);
        _editWindow.alpha = 1;
        _editWindow.interactable = true;
        _editWindow.blocksRaycasts = true;
        CurrentGame.SetData();
    }

    private void StarBtnClick()
    {
        if (isFavourite == false)
        {
            StartCoroutine(SetFavourite(_id));
            _favText.text = "Убрать избранные";
        }

        if (isFavourite == true)
        {
            StartCoroutine(RemoveFavourite(_id));
            _favText.text = "В избранные";
        }
    }

    private IEnumerator SetFavourite(int id)
    {
        var dbTask = _fireBase.DBreference.Child("Users").Child(_fireBase.User.UserId).GetValueAsync();
        yield return new WaitUntil(predicate: () => dbTask.IsCompleted);

        if (dbTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {dbTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = dbTask.Result;
            int count = Convert.ToInt32(snapshot.Child("Count").Value);
            count++;

            var dbTask1 = _fireBase.DBreference.Child("Users").Child(_fireBase.User.UserId).Child("Count")
                .SetValueAsync(count.ToString());
            var dbTask2 = _fireBase.DBreference.Child("Users").Child(_fireBase.User.UserId).Child(count.ToString())
                .SetValueAsync(id.ToString());

            yield return new WaitUntil(predicate: () => dbTask1.IsCompleted && dbTask2.IsCompleted);
            if (dbTask1.Exception != null || dbTask2.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {dbTask1.Exception}");
                Debug.LogWarning(message: $"Failed to register task with {dbTask2.Exception}");
            }
            else
            {
                _starBtn.image.color = Color.yellow;
                Debug.Log("Favourite game set!");
            }

            Element element = _contentUpdate.Elements[id - 1].GetComponent<Element>();
            element.IsFavourite = true;
            isFavourite = true;
            FavouriteGames.Games.Add(id);
        }
    }

    private IEnumerator RemoveFavourite(int id)
    {
        var dbTask = _fireBase.DBreference.Child("Users").Child(_fireBase.User.UserId).GetValueAsync();
        yield return new WaitUntil(predicate: () => dbTask.IsCompleted);

        if (dbTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {dbTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = dbTask.Result;
            int count = Convert.ToInt32(snapshot.Child("Count").Value);
            int favId = 0;

            for (int i = 1; i <= count; i++)
            {
                if (Convert.ToInt32(snapshot.Child(i.ToString()).Value) == id)
                {
                    favId = i;
                }
            }

            int gameId;

            for (int i = favId; i <= count; i++)
            {
                if (i == favId)
                {
                    var dbTask3 = _fireBase.DBreference.Child("Users").Child(_fireBase.User.UserId)
                        .Child(favId.ToString())
                        .RemoveValueAsync();
                    yield return new WaitUntil(predicate: () => dbTask3.IsCompleted);
                }
                else if (favId <= count)
                {
                    gameId = Convert.ToInt32(snapshot.Child(i.ToString()).Value);

                    var dbTask1 = _fireBase.DBreference.Child("Users").Child(_fireBase.User.UserId).Child(i.ToString())
                        .RemoveValueAsync();

                    int newId = i - 1;
                    var dbTask2 = _fireBase.DBreference.Child("Users").Child(_fireBase.User.UserId).Child(newId.ToString())
                        .SetValueAsync(gameId.ToString());
                    yield return new WaitUntil(predicate: () => dbTask1.IsCompleted && dbTask2.IsCompleted);
                }
                
            }

            count--;
            var dbTask4 = _fireBase.DBreference.Child("Users").Child(_fireBase.User.UserId).Child("Count")
                .SetValueAsync(count.ToString());
            yield return new WaitUntil(predicate: () => dbTask4.IsCompleted);

            FavouriteGames.Games.Remove(id);
            Element element = _contentUpdate.Elements[id - 1].GetComponent<Element>();
            element.IsFavourite = false;
            _starBtn.image.color = Color.white;
            isFavourite = false;
            Debug.Log("Favourite game removed!");
        }
    }
}