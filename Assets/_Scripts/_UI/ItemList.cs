using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemList : MonoBehaviour
{
    public TMPro.TextMeshProUGUI weightText;

    [SerializeField]
    ItemListElement itemPrefab;

    [SerializeField]
    Transform itemsParent;
    public List<ItemListElement> elements;

    private void Awake()
    {
        elements = new List<ItemListElement>();
    }

    public void UpdateList(List<Item> items, float weight, float capacity)
    {
        foreach (ItemListElement element in elements)
        {
            Destroy(element.gameObject);
        }

        elements.Clear();

        foreach (Item item in items)
        {
            CreateElement(item);
        }

        weightText.SetText(weight + " / " + capacity);
    }

    public void CreateElement(Item item)
    {
        // Check if item is already present in list
        ItemListElement element = null;
        if (elements.Count > 0)
        {
            foreach (ItemListElement elem in elements)
            {
                if (elem.items[0].data == item.data)
                {
                    element = elem;
                    break;
                }
            }
        }

        // If there's one, increment it's count
        if (element)
        {
            element.items.Add(item);
            element.countText.SetText(element.items.Count.ToString());
        }
        // Else create a new element
        else
        {
            element = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity, itemsParent);
            element.icon.sprite = item.data.icon;

            if (element.items == null)
                element.items = new List<Item>();

            element.items.Add(item);
            element.countText.SetText(element.items.Count.ToString());
            elements.Add(element);
        }
    }
}
