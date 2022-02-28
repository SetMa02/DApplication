using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentGame : MonoBehaviour
{
    public RawImage _image;
    public InputField _name;
    public InputField _desc;
    public InputField _genre;
    public InputField _price;
    public Dropdown _platform;
    
    public  String NewPlatform;
    public static int Id;
    public static RawImage Icon;
    public static string Name;
    public static string Description;
    public static string Genres;
    public static string Platform;
    public static string Price;
    public static string ImageName;
    public static byte[] ImageData;
    public static bool IsNewGame = false;

    private void Start()
    {
        _platform.onValueChanged.AddListener(delegate{
            PlatformChanged();
        });
    }

    private void PlatformChanged()
    {
        switch (_platform.value)
        {
            case 0:
                NewPlatform = "Ps4";
                break;
            case 1:
                NewPlatform = "Ps5";
                break;
            case 2:
                NewPlatform = "Xbox one";
                break;
            case 3:
                NewPlatform = "Xbox one X";
                break;
            case 4:
                NewPlatform = "PC";
                break;
            case 5:
                NewPlatform = "Nintendo switch";
                break;
        }
    }

    public void SetData()
    {
        try
        {
            _image.texture = Icon.texture;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        _name.text = Name;
        _desc.text = Description;
        _genre.text = Genres;
        _price.text = Price;
        switch (Platform)
        {
            case "Ps4":
                _platform.value = 0;
                break;
            case "Ps5":
                _platform.value = 1;
                break;
            case "Xbox one":
                _platform.value = 2;
                break;
            case "Xbox one X":
                _platform.value = 3;
                break;
            case "PC":
                _platform.value = 4;
                break; 
            case "Nintendo switch":
                _platform.value = 5;
                break;
        }
        
    }
  
}
