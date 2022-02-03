using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProductionBuilding : Stockpile
{
    public s_ProductionBuilding data;
    public float workDone = 0;

    void Start()
    {
        BuildingManager.singleton.buildings.Add(this);
        CreateJob();
    }

    [ContextMenu("Create Job")]
    public void CreateJob()
    {
        Job job = null;
        switch (data.producedItem)
        {
            case ICraftable craftable:
                job = new Craft(null, this);
                break;
            default:
                job = new Produce(null, this);
                break;
        }

        JobManager.singleton.AddJob(job);
    }

    public float GetStorageTotalWegiht()
    {
        return StoredItems.Sum(item => item.data.weight);
    }

    public void Produce(Pawn pawn)
    {
        workDone += 1f * Time.deltaTime;
        pawn.actionTimeRemaining = 1f;
        if (workDone > data.producedItem.workQuantity)
        {
            workDone = 0f;

            bool canProduceItem = false;

            switch (data.producedItem)
            {
                case ICraftable craftable:
                    Craft craft = (pawn.currentJob as Craft);

                    if (craft.usedIngredients == null)
                    {
                        Debug.LogError("Craft job in : " + name + " for : " + craftable + " was missing used ingredients because WTF BBQ");
                        craft.usedIngredients = s_Crafter.FindIngredients(this, craftable);
                    }

                    if (craft.usedIngredients.Count == craftable.GetRecipe().Count)
                    {
                        //print("Consumed " + craft.usedIngredients.Count + " ingredients to craft a " + (craft.sourceBuilding as ProductionBuilding).data.producedItem.name);
                        foreach (Item item in craft.usedIngredients)
                        {
                            RemoveItemFromStorage(item);
                        }

                        canProduceItem = true;
                    }
                    else
                    {
                        Debug.LogError(name + " was missing ingredients to craft : " + craftable + "\nCancelling job...");
                        canProduceItem = false;
                    }
                    break;

                default:
                    canProduceItem = true;
                    break;
            }

            if (canProduceItem)
                GenerateItem();

            JobManager.singleton.RemoveJob(pawn.currentJob);
            if (GetStorageTotalWegiht() < data.storageCapacity)
            {
                CreateJob();
            }

            pawn.jobStep = 0;
        }
    }

    public void GenerateItem(Item.Quality quality = Item.Quality.Normal)
    {
        Item newItem = null;
        switch (data.producedItem)
        {
            case s_Food food:
                newItem = new Food(data.producedItem, quality, null);
                break;

            default:
                newItem = new Item(data.producedItem);
                break;
        }

        AddItemToStorage(newItem);

        Haul haulJob = new Haul(null, this, newItem, null);
        JobManager.singleton.AddJob(haulJob);
        pendingJobs.Add(haulJob);
    }

    public override void RemoveItemFromStorage(Item item)
    {
        base.RemoveItemFromStorage(item);

        if (GetStorageTotalWegiht() <= data.storageCapacity)
        {
            CreateJob();
        }
    }
}
