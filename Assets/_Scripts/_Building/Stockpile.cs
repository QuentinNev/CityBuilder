using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stockpile : Building
{
    public s_Storage storage;
    List<Item> storedItems;
    /// <summary>
    /// Get the list of the stored item, use Add or Remove to change item from storage
    /// </summary>
    public List<Item> StoredItems { get { return storedItems; } }
    // -----------------------------------
    [SerializeField]
    List<string> items;

    private void Update()
    {
        items.Clear();

        foreach(Item item in storedItems)
        {
            items.Add(item.data.name);
        }
    }
    //------------------------------------

    protected override void Awake()
    {
        base.Awake();
        storedItems = new List<Item>();
    }

    void Start()
    {
        BuildingManager.singleton.buildings.Add(this);
        StockpileManager.singleton.AddStockpile(this);
    }

    public float invTotalWeight
    {
        get
        {
            return storedItems.Sum(item => item.data.weight);
        }
    }

    public bool CanDropItemInto(Item item)
    {
        return (invTotalWeight + item.data.weight > storage.storageCapacity);
    }

    public void AddItemToStorage(Item item)
    {
        storedItems.Add(item);
        OnStoredItemsUpdated(EventArgs.Empty);
    }

    public virtual void RemoveItemFromStorage(Item item)
    {
        storedItems.Remove(item);
        OnStoredItemsUpdated(EventArgs.Empty);
    }

    public event EventHandler StoredItemsUpdate;
    protected virtual void OnStoredItemsUpdated(EventArgs e)
    {
        StoredItemsUpdate?.Invoke(this, e);
    }
}
