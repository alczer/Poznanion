using UnityEngine;
using System.Collections;
public enum PlayerColor { RED, GREEN, BLUE, YELLOW, BLACK }
public class Player{
    
    public int points;
    public string name;
    public int meeples;
    public PlayerColor color;
    public Color rgbaColor;
    public Player(string n, PlayerColor col,Color rgba)
    {
        this.name = n;
        this.points = 0;
        this.color = col;
        this.meeples = 5;
        this.rgbaColor = rgba;
    }



}
