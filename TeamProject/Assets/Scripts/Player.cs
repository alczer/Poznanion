using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    int points;
    string name;
    public void Init(string n)
    {
        this.name = n;
        this.points = 0;
    }


}
