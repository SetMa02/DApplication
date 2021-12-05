using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class User : MonoBehaviour
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string DeliveryAddress { get; set; }
    public List<Game> Basket { get; set; }
}
