using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System;
public class Test : MonoBehaviour
{

    Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit;
    public static Color hexToColor(string hex)
    {
        hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }
    Color yellowColor = hexToColor("ffff00");
    Color blueColor = hexToColor("0000ff");
    Color greenColor = hexToColor("00ff00");
    Color redColor = hexToColor("ff0000");

    void Update()
    {
        if (Physics.Raycast(myRay, out hit))
        {
            if (Input.GetMouseButtonUp(0))
            {

                Texture2D tex = (Texture2D)hit.collider.gameObject.GetComponent<Renderer>().material.mainTexture; // Get texture of object under mouse pointer
                if (tex.GetPixelBilinear(hit.textureCoord2.x, hit.textureCoord2.y) == blueColor)
                {

                    Debug.Log("Clicked CASTLE !!!!!!!!!!!!!!!");
                }


                Debug.Log("Clicked me!");

            }

        }

    }
    //     Texture2D tex = (Texture2D)hit.collider.gameObject.GetComponent<Renderer>().material.mainTexture; // Get texture of object under mouse pointer
    //    if (tex.GetPixelBilinear(hit.textureCoord2.x, hit.textureCoord2.y) == blueColor)
    //   {
    //
    //                   Debug.Log("Clicked CASTLE !!!!!!!!!!!!!!!");
    //              }
    //             Debug.Log("Clicked me!");

}