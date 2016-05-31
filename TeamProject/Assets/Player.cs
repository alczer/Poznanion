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
    public GameObject glow;

    public Player(string n, PlayerColor col, Color rgba, PlayerType typ = PlayerType.HUMAN)
    {
        this.name = n;
        this.points = 0;
        this.color = col;
        this.type = typ;
        this.meeples = 5;
        this.rgbaColor = rgba;
    }

    public void Clone(Player other)
    {
        this.name = other.name;
        this.points = other.points;
        this.color = other.color;
        this.type = other.type;
        this.meeples = other.meeples;
        this.rgbaColor = other.rgbaColor;
        this.glow = other.glow;
    }

    public void ChangeScore(int score)
    {
        this.points += score;

    }
}
