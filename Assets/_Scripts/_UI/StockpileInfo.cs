using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StockpileInfo : MonoBehaviour
{
    [HideInInspector]
    public Stockpile watchedStockpile;

    [SerializeField]
    ItemList itemList;

    public void Setup(Stockpile stockpile)
    {
        if (watchedStockpile)
            Unsubscribe(watchedStockpile);

        watchedStockpile = stockpile;
        Subscribe(watchedStockpile);
        
        itemList.UpdateList(stockpile.StoredItems, stockpile.invTotalWeight, stockpile.storage.storageCapacity);
    }

    public void SetInfos()
    {

    }

    public void Subscribe(Stockpile stockpile)
    {
        watchedStockpile.StoredItemsUpdate += UpdateItemList;
    }

    public void UpdateItemList(object sender, System.EventArgs e)
    {
        Stockpile stockpile = (sender as Stockpile);
        itemList.UpdateList(stockpile.StoredItems, stockpile.invTotalWeight, stockpile.storage.storageCapacity);
    }

    public void Unsubscribe(Stockpile stockpile)
    {
        watchedStockpile.StoredItemsUpdate -= UpdateItemList;
    }
}
