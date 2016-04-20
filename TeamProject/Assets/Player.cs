using UnityEngine;
using System.Collections;
public enum PlayerColor { RED, GREEN, BLUE, YELLOW, BLACK }
public class Player{
    
    public int points;
    public string name;
    public PlayerColor color;
    public Player(string n, PlayerColor col)
    {
        this.name = n;
        this.points = 0;
        this.color = col;
    }



}
