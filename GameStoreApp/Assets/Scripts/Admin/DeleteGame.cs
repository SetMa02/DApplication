using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;

public class DeleteGame : MonoBehaviour
{
    [SerializeField] private FireBase _fireBase;
    [SerializeField] private CanvasGroup _gameWindow;
    [SerializeField] private WarningMessage _warningMessage;
    [SerializeField] private ErrorMessage _errorMessage;
    [SerializeField] private GameWindow _game;
    [SerializeField] private ContentUpdate _contentUpdate;
    private Button _deleteButton;
    
    private void Start()
    {
        _deleteButton = GetComponent<Button>();
        _deleteButton.onClick.AddListener(Delete);
    }

    private void Delete()
    {
        StartCoroutine(DeletCurrentGame());
    }

    private IEnumerator DeletCurrentGame()
    {
        var DbTask = _fireBase.DBreference.Child("Games").GetValueAsync();
        yield return new WaitUntil(predicate: () => DbTask.IsCompleted);
        if (DbTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DbTask.Exception}");
        }
        else
        {
            int deleteId = _game.Id;
            DataSnapshot snapshot = DbTask.Result;
            int count = Convert.ToInt32(snapshot.Child("Count").Value);

            var DbTask1 = _fireBase.DBreference.Child("Games").Child(deleteId.ToString()).RemoveValueAsync();
            yield return new WaitUntil(predicate: () => DbTask1.IsCompleted);
            if (DbTask1.IsCompleted == true)
            {
                int id;
                string name;
                string description;
                string image;
                string platform;
                string genre;
                string price;
                for (int i = (deleteId+1); i <= count; i++)
                {
                    id = i;
                    name = snapshot.Child(i.ToString()).Child("Name").Value.ToString();
                    description = snapshot.Child(i.ToString()).Child("Description").Value.ToString();
                    platform = snapshot.Child(i.ToString()).Child("Platform").Value.ToString();
                    image = snapshot.Child(i.ToString()).Child("Image").Value.ToString();
                    genre = snapshot.Child(i.ToString()).Child("Genres").Value.ToString();
                    price = snapshot.Child(i.ToString()).Child("Price").Value.ToString();
                    id--;
                    var DbTaskName = _fireBase.DBreference.Child("Games").Child(id.ToString()).Child("Name")
                        .SetValueAsync(name);
                    var DbTaskDesc = _fireBase.DBreference.Child("Games").Child(id.ToString()).Child("Description")
                        .SetValueAsync(description);
                    var DbTaskPlatform = _fireBase.DBreference.Child("Games").Child(id.ToString()).Child("Platform")
                        .SetValueAsync(platform);
                    var DbTaskimage = _fireBase.DBreference.Child("Games").Child(id.ToString()).Child("Image")
                        .SetValueAsync(image);
                    var DbTaskGenres = _fireBase.DBreference.Child("Games").Child(id.ToString()).Child("Genres")
                        .SetValueAsync(genre);
                    var DbTaskPrice = _fireBase.DBreference.Child("Games").Child(id.ToString()).Child("Price")
                        .SetValueAsync(price);
                    var DbRemove = _fireBase.DBreference.Child("Games").Child((id+1).ToString()).RemoveValueAsync();
                    
                    
                    yield return new WaitUntil(predicate: () => DbTaskName.IsCompleted && DbTaskDesc.IsCompleted && 
                                                                DbTaskPlatform.IsCompleted && DbTaskimage.IsCompleted 
                                                                && DbTaskGenres.IsCompleted && DbTaskPrice.IsCompleted
                                                                && DbRemove.IsCompleted);
                    
                }
                _game.CloseWindow();
                _game.GameFrame.alpha = 0;
                StartCoroutine(_contentUpdate.LoadUserData());
                _errorMessage.ShowMessageAndClose(_game.GameFrame, "Успешно удалено");

            }

        }
    }
}
