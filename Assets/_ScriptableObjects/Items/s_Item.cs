using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "Items/New Item", order = 1)]
public class s_Item : ScriptableObject
{
    public Sprite icon;
    public float value;
    public float weight;
    public float workQuantity;
    [SerializeField]
    protected List<s_Item> ingredients;
}