using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveGame : MonoBehaviour
{
    [SerializeField] private FireBase _fireBase;
    [SerializeField] private Cancel _cancel;
    [SerializeField] private CurrentGame _currentGame;
    private Button _saveButton;

    private void Start()
    {
        _saveButton = GetComponent<Button>();
        _saveButton.onClick.AddListener(SaveButtonClck);
    }

    private void SaveButtonClck()
    {
        StartCoroutine(SaveData());
    }


    public IEnumerator SaveData()
    {
        var DbTaskName = _fireBase.DBreference.Child("Games").Child(CurrentGame.Id.ToString()).Child("Name")
            .SetValueAsync(_currentGame._name.text);
        var DbTaskDescritpion = _fireBase.DBreference.Child("Games").Child(CurrentGame.Id.ToString()).Child("Description")
            .SetValueAsync(_currentGame._desc.text);
        var DbTaskGenre = _fireBase.DBreference.Child("Games").Child(CurrentGame.Id.ToString()).Child("Genres")
            .SetValueAsync(_currentGame._genre.text);
        var DbTaskPrice = _fireBase.DBreference.Child("Games").Child(CurrentGame.Id.ToString()).Child("Price")
            .SetValueAsync(_currentGame._price.text);

        yield return new WaitUntil(predicate: () => DbTaskName.IsCompleted && DbTaskDescritpion.IsCompleted &&
            DbTaskGenre.IsCompleted && DbTaskPrice.IsCompleted);

        if (DbTaskName.IsCompleted && DbTaskDescritpion.IsCompleted &&
            DbTaskGenre.IsCompleted && DbTaskPrice.IsCompleted)
        {
            Debug.Log("Data saved");
            _cancel.CancelButtonClick();
        }
    }
}
