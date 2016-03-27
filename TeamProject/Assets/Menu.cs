using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 50), "New Game"))
        {
            
            Application.LoadLevel("game");
        }
        if (GUI.Button(new Rect(10, 70, 100, 50), "Exit"))
        {
            Application.Quit();
            Debug.Log("wyszedlo");
        }

    }
	// Use this for initialization
	void Start () {
	Debug.Log("weszlo");

	}
	
	// Update is called once per frame
	void Update () {
       if (Input.GetKey(KeyCode.Escape))
   	 {
       		 Application.Quit();
          		 Debug.Log("wyszedlo");
   	 }
	}
}
