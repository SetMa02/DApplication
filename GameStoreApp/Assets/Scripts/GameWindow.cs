using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using DefaultNamespace;
using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameWindow : MonoBehaviour
{
    [SerializeField] private RawImage _icon;
    [SerializeField] private TMP_InputField _name;
    [SerializeField] private Text _description;
    [SerializeField] private Button _starBtn;
    [SerializeField] private Button _backBtn;
    [SerializeField] private FireBase _fireBase;

    private GameObject _mainPanel;
    private CanvasGroup _mainFrame;
    private CanvasGroup _gameFrame;
    private int _id;
    private bool isFavourite = false;

    private void Start()
    {
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
        _id = id;

        if (FavouriteGames.Games.Contains(id))
        {
            isFavourite = true;
        }
        else
        {
            isFavourite = false;
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

    private void StarBtnClick()
    {
        if (isFavourite == false)
        {
            StartCoroutine(SetFavourite(_id));
        }

        if (isFavourite == true)
        {
            StartCoroutine(RemoveFavourite(_id));
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

            ContentUpdate.Elements[id - 1].IsFavourite = true;
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
            ContentUpdate.Elements[id - 1].IsFavourite = false;
            _starBtn.image.color = Color.white;
            isFavourite = false;
            Debug.Log("Favourite game removed!");
        }
    }
}