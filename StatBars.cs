using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBars : MonoBehaviour
{
    public int maxWidth = 142;
    public RectTransform healthValueRectTransform;
    public RectTransform manaValueRectTransform;
    public RectTransform fuelValueRectTransform;

    public void SetValueBarSize(string stat, float value)
    {
        switch (stat)
        {
            case "health":
                SetBarWidth(healthValueRectTransform, value);
                break;
            case "mana":
                SetBarWidth(manaValueRectTransform, value);
                break;
            case "fuel":
                SetBarWidth(fuelValueRectTransform, value);
                break;
            default:
                Debug.LogWarning("Invalid stat name passed to SetValueBarSize: " + stat);
                break;
        }
    }
    
    private void SetBarWidth(RectTransform rectTransform, float value)
    {
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = new Vector2(maxWidth * value, rectTransform.sizeDelta.y);
            Debug.Log("Update size to " + maxWidth * value);
        }
        else
        {
            Debug.LogWarning("RectTransform is null");
        }
    }

    
}
