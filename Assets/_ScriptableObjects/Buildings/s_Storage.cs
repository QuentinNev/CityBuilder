using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stockpile", menuName = "Buildings/Stockpile", order = 1)]
public class s_Storage : ScriptableObject
{
    public float storageCapacity;
    public List<s_Item> storableItems;
}