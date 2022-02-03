using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Items/Meal", order = 1)]
public class s_Meal : s_Item, ICraftable
{
    public List<s_Item> GetRecipe()
    {
        return ingredients;
    }
}
