using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public Image filling;
    public TMPro.TextMeshProUGUI numbers;

    const float MAX_SIZE = 200;

    public void UpdatebBar(float currentValue, float maxValue)
    {
        float fillingSize = currentValue / maxValue * MAX_SIZE;
        RectTransform rect = (filling.transform as RectTransform);
        rect.sizeDelta = new Vector2(fillingSize, rect.sizeDelta.y);
        rect.localPosition = new Vector2((fillingSize - MAX_SIZE) / 2f, rect.localPosition.y);
        numbers.SetText(currentValue + " / " + maxValue);
    }
}
