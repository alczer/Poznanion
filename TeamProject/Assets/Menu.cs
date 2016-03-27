using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
    private bool Escape=false;
    void Exit()
    {
        if( Escape==true)
        {
            if (GUI.Button(new Rect(110, 70, 100, 50), "Yes"))
            {
                Application.Quit();
                Debug.Log("wyszedlo");
            }
            if (GUI.Button(new Rect(210, 70, 100, 50), "No"))
            {
                Escape = false;
            }
        }

    }
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 50), "New Game"))
        {
            Application.LoadLevel("game");
        }
        if (GUI.Button(new Rect(10, 70, 100, 50), "Exit"))
        {
            Escape = true;
            Debug.Log("wyszedlo");
        }
        Exit();
    }
	// Use this for initialization
	void Start () 
    {
	    Debug.Log("weszlo");
	}
	
	// Update is called once per frame
	void Update () {
       if (Input.GetKeyUp(KeyCode.Escape))
   	 {
         if (Escape == true)
         {
             Application.Quit();
             Debug.Log("wyszedlo");
         }
         else
             Escape = true;
   	 }
	}
}
