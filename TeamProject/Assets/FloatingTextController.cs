using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FloatingTextController : MonoBehaviour
{
    private static FloatingText popupText;
    private static GameObject canvas;

    public static void Initialize()
    {
        canvas = GameObject.Find("Canvas");
        if (!popupText)
            popupText = Resources.Load<FloatingText>("PopupTextParent");
    }

    public static void CreateFloatingText2(string text, float x, float y, Color color)
    {
        FloatingText instance = Instantiate(popupText);
        instance.transform.SetParent(canvas.transform, false);
        //poziomo , pionowo, nic
        instance.transform.position = Camera.main.WorldToScreenPoint(new Vector3(x,0,y));
        instance.SetText(text);
        instance.SetColor(color);
    }


}