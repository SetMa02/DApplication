using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Extensions;
using Firebase.Storage;
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
        if (CurrentGame.IsNewGame == true)
        {
            int newId = Convert.ToInt32(Snapshot.DbSnapshot.Child("Count").Value);
            newId++;
            CurrentGame.Id = newId;
        }
        
        var DbTaskName = _fireBase.DBreference.Child("Games").Child(CurrentGame.Id.ToString()).Child("Name")
            .SetValueAsync(_currentGame._name.text);
        var DbTaskDescritpion = _fireBase.DBreference.Child("Games").Child(CurrentGame.Id.ToString()).Child("Description")
            .SetValueAsync(_currentGame._desc.text);
        var DbTaskGenre = _fireBase.DBreference.Child("Games").Child(CurrentGame.Id.ToString()).Child("Genres")
            .SetValueAsync(_currentGame._genre.text);
        var DbTaskPrice = _fireBase.DBreference.Child("Games").Child(CurrentGame.Id.ToString()).Child("Price")
            .SetValueAsync(_currentGame._price.text);
        var DbTaskPlatform = _fireBase.DBreference.Child("Games").Child(CurrentGame.Id.ToString()).Child("Platform")
            .SetValueAsync(_currentGame.NewPlatform);

        if (CurrentGame.IsNewGame == true)
        {
            StartCoroutine(UploadImage());
        }
        
        yield return new WaitUntil(predicate: () => DbTaskName.IsCompleted && DbTaskDescritpion.IsCompleted &&
            DbTaskGenre.IsCompleted && DbTaskPrice.IsCompleted && DbTaskPlatform.IsCompleted);

        if (DbTaskName.IsCompleted && DbTaskDescritpion.IsCompleted &&
            DbTaskGenre.IsCompleted && DbTaskPrice.IsCompleted && DbTaskPlatform.IsCompleted)
        {
            Debug.Log("Data saved");
            _cancel.CancelButtonClick();
        }
    }

    public IEnumerator UploadImage()
    {
        var DbTaskImageName = _fireBase.DBreference.Child("Games").Child(CurrentGame.Id.ToString()).Child("Image")
            .SetValueAsync(CurrentGame.ImageName);
        
        var newMetadata = new MetadataChange();
        newMetadata.ContentType = "image/jpeg";

        //Create a reference to where the file needs to be uploaded
        StorageReference uploadRef = _fireBase.StorageReference.Child(CurrentGame.ImageName);
        Debug.Log("File upload started");
        uploadRef.PutBytesAsync(CurrentGame.ImageData, newMetadata).ContinueWithOnMainThread((task) => { 
            if(task.IsFaulted || task.IsCanceled){
                Debug.Log(task.Exception.ToString());
            }
            else{
                Debug.Log("File Uploaded Successfully!");
            }
        });


        yield return new WaitUntil(predicate: () => DbTaskImageName.IsCompleted);

        if (DbTaskImageName.IsCompleted)
        {
            Debug.Log("Image Uploaded successfully");
        }

    }
}
