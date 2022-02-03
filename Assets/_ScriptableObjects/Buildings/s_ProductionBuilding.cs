using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Buildings/ProductionBuilding", order = 1)]
public class s_ProductionBuilding : ScriptableObject
{
    public s_Item producedItem;
    public float storageCapacity;
    public bool automated;
}
