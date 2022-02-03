using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class is a blank item and shouldn't be used
/// </summary>
public class Item
{
    /// <summary>
    /// ScriptableObject representing the item
    /// </summary>
    public s_Item data;
    /// <summary>
    /// Quality of the item, will affect item values and properties
    /// </summary>
    public enum Quality { Awful, Poor, Normal, Good, Excellent, Masterwork, Legendary }
    public Quality quality = Quality.Normal;
    public Item(s_Item data, Quality quality = Quality.Normal)
    {
        this.data = data;
        this.quality = quality;
    }    
}

/// <summary>
/// Food is a decaying item; it will rot after some time
/// </summary>
public class Food : Item
{
    /// <summary>
    /// List of ingredients used to create the food if applicable
    /// </summary>
    public List<s_Food> ingredients;
    public float decay;

    public Food(s_Item data, Quality quality, List<s_Food> ingredients) : base(data, quality)
    {
        this.ingredients = ingredients;
    }
}