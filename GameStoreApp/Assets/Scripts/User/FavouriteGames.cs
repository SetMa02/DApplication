using System.Collections.Generic;

namespace DefaultNamespace
{
    public class FavouriteGames
    {
        public static List<FavGame> Games = new List<FavGame>() {};
        
        public static void AddToGame(int id,string name, int price)
        {
            FavGame favGame = new FavGame(id, name, price);
            Games.Add(favGame);
        }
    } 
    
    
    
    public class FavGame
    {
        private int _id;
    private string _name;
    private int _price;

    public int Id => _id;
    public string Name => _name;
    public int Privce => _price;
    public FavGame(int id,string name, int price)
    {
        _id = id;
        _name = name;
        _price = price;
    }
    }
}