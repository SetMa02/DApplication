using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadGame : MonoBehaviour
{
    
    public static void LoadGameData(int id,RawImage rawImage, string name, string desc, string genre, string price, string platform)
    {
        CurrentGame.Id = id;
        if (rawImage != null)
        {
            CurrentGame.Icon = rawImage;
        }
        CurrentGame.Name = name;
        CurrentGame.Description = desc;
        CurrentGame.Genres = genre;
        CurrentGame.Price = price;
        CurrentGame.Platform = platform;
        CurrentGame.Platform = platform;
    }

  
    
}
