using UnityEngine;
using System.Collections;
public enum PlayerColor { RED, GREEN, BLUE, YELLOW, BLACK }
public enum PlayerType { AI, HUMAN}

public class Player{
    
    public int points;
    public string name;
    public PlayerColor color;
    public PlayerType type;

    public Player(string n, PlayerColor col, PlayerType typ = PlayerType.HUMAN)
    {
        this.name = n;
        this.points = 0;
        this.color = col;
        this.type = typ;
    }



}
