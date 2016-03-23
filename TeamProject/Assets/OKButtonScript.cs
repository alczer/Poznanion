using UnityEngine;
using System.Collections;

public class OKButtonScript : MonoBehaviour {
    public GameObject ExampleButton;
    public void OnButtonClick()
    {
        ExampleButton.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //ExampleButton.SetActive(true);
        }
    }
}
