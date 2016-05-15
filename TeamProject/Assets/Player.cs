using UnityEngine;
using System.Collections;
public enum PlayerColor { RED, GREEN, BLUE, YELLOW, BLACK }
public enum PlayerType { AI, HUMAN}

public class Player{
    
    public int points;
    public string name;
    public PlayerColor color;
    public PlayerType type;
    public int meeples;
    public Color rgbaColor;

    public Player(string n, PlayerColor col, Color rgba, PlayerType typ = PlayerType.HUMAN)
    {
        this.name = n;
        this.points = 0;
        this.color = col;
        this.type = typ;
        this.meeples = 5;
        this.rgbaColor = rgba;
    }
    public void ChangeScore(int score)
    {
        this.points += score;

    }
}
