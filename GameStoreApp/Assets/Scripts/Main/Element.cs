using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Extensions;
using Firebase.Storage;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Element : MonoBehaviour
{
   public int id;
   
   public RawImage Icon;
   public TMP_Text Name;

   public void loadData(string url)
   {
      Debug.Log("Image Load");
      StartCoroutine(LoadImage(url));
   }
   public IEnumerator LoadImage(string URL)
   {
      Debug.Log("Starting load image...");
      UnityWebRequest request = UnityWebRequestTexture.GetTexture(URL);
      yield return request.SendWebRequest();
      if (request.isDone)
      { 
        Icon.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
         Debug.Log("Done");
      }
   }
}
