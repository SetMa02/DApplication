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
    
    public static int Id;
    public static RawImage Icon;
    public static string Name;
    public static string Description;
    public static string Genres;
    public static string Platform;
    public static string Price;
    
    public void SetData()
    {
        _image.texture = Icon.texture;
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
