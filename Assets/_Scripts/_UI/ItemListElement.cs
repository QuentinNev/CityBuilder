using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemListElement : MonoBehaviour
{
    public Image icon;
    public TMPro.TextMeshProUGUI countText;
    public List<Item> items;

    private void Awake()
    {
        items = new List<Item>();
    }
}
