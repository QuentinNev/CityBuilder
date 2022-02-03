using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Profession/Crafter", order = 1)]
public class s_Crafter : s_Profession
{
    public override void DoJob(Pawn pawn)
    {
        if (pawn.currentJob == null)
        {
            Craft productionJob = JobManager.GetAvailableJob<Craft>();

            if (productionJob != null)
            {
                if (pawn.CheckIfDestinationIsReachable(productionJob.sourceBuilding.entrance.position))
                {
                    productionJob.assignedPawn = pawn;
                    pawn.jobs.Add(productionJob);
                }
            }
            else
                pawn.DelayJob();

            pawn.Status = "Looking for a job";
        }
        else
        {
            ProductionBuilding building = (pawn.currentJob as Craft).sourceBuilding as ProductionBuilding;
            switch (pawn.jobStep)
            {
                // Going to workplace
                case 0:
                    if ((pawn.transform.position - building.entrance.position).magnitude > building.radius)
                    {
                        if (!pawn.HasPath())
                            pawn.SetDestination(building.entrance.position);
                    }
                    else
                    {
                        pawn.StopMoving();
                        pawn.jobStep++;
                    }

                    pawn.Status = "Going to " + building.name;
                    break;

                // Working
                case 1:
                    ICraftable craftable = building.data.producedItem as ICraftable;
                    List<Item> usedIngredients = FindIngredients(building, craftable);

                    if (usedIngredients.Count == craftable.GetRecipe().Count)
                    {
                        (pawn.currentJob as Craft).usedIngredients = usedIngredients;
                        pawn.jobStep++;
                    }
                    else
                        pawn.DelayJob();

                    pawn.Status = "Gathering ingredients for " + building.data.producedItem.name;
                    break;

                case 2:
                    building.Produce(pawn);
                    pawn.Status = "Producing " + building.data.producedItem.name;
                    break;
            }
        }
    }

    /// <summary>
    /// Return a list of usable ingredients found in the building
    /// </summary>
    /// <param name="building"></param>
    /// <param name="craftable"></param>
    /// <returns></returns>
    public static List<Item> FindIngredients(ProductionBuilding building, ICraftable craftable)
    {
        List<Item> usedIngredients = new List<Item>();
        foreach (Item item in building.StoredItems)
        {
            if (craftable.GetRecipe().Contains(item.data))
            {
                usedIngredients.Add(item);

                if (usedIngredients.Count == craftable.GetRecipe().Count)
                    break;
            }
        }

        return usedIngredients;
    }
}
