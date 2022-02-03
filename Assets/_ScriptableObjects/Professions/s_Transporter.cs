using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Data", menuName = "Profession/Transporter", order = 1)]
public class s_Transporter : s_Profession
{
    public override void DoJob(Pawn pawn)
    {
        // Look for a new job
        if (pawn.jobs.Count == 0)
        {
            Haul haulJob = JobManager.GetAvailableJob<Haul>();

            if (haulJob != null)
            {
                if (pawn.CheckIfDestinationIsReachable(haulJob.sourceBuilding.entrance.position))
                {
                    if (pawn.CanPickUpItem(haulJob.item))
                    {
                        haulJob.assignedPawn = pawn;
                        pawn.jobs.Add(haulJob);
                        pawn.StopMoving();

                        // Check if item is required elsewhere than a simple stockpile
                        ProductionBuilding productionBuilding = GetAcceptableProductionBuilding(haulJob);

                        // Bring item to production building before storing it in a stockpile
                        if (productionBuilding)
                        {
                            haulJob.stockpileB = productionBuilding;
                        }
                        else
                            haulJob.stockpileB = StockpileManager.singleton.GetClosestStockpile(haulJob.sourceBuilding.entrance.position, (haulJob.sourceBuilding as Stockpile));


                        List<Haul> haulJobs = JobManager.GetAvailableJobs<Haul>();
                        foreach (Haul similarHaulJob in haulJobs)
                        {
                            // Check if pawn can carry more items
                            if (pawn.invTotalWeight + haulJob.item.data.weight < pawn.maxWeight)
                            {
                                if (haulJob.sourceBuilding == similarHaulJob.sourceBuilding)
                                {
                                    productionBuilding = GetAcceptableProductionBuilding(haulJob);

                                    // TODO : Avoid code duplicate for this if else
                                    if (!productionBuilding)
                                        haulJob.stockpileB = StockpileManager.singleton.GetClosestStockpile(haulJob.sourceBuilding.entrance.position, (haulJob.sourceBuilding as Stockpile));

                                    if (productionBuilding == haulJob.stockpileB)
                                    {
                                        pawn.jobs.Add(similarHaulJob);
                                        similarHaulJob.assignedPawn = pawn;
                                        similarHaulJob.stockpileB = productionBuilding;
                                    }
                                }
                            }
                            else
                                break;
                        }
                    }
                }

                pawn.Status = "Looking for a job";
            }
        }
        // Do the job
        else
        {
            Haul haulJob;
            switch (pawn.jobStep)
            {
                // Going to pickup item
                case 0:
                    haulJob = (pawn.jobs[0] as Haul);
                    if ((pawn.transform.position - haulJob.sourceBuilding.entrance.position).magnitude > haulJob.sourceBuilding.radius)
                    {
                        if (!pawn.HasPath())
                            pawn.SetDestination(haulJob.sourceBuilding.entrance.position);

                        pawn.Status = "Going to " + haulJob.sourceBuilding.name;
                    }
                    else
                    {
                        if (pawn.CanPickUpItem(haulJob.item))
                        {
                            pawn.StopMoving();
                            pawn.jobStep++;
                            pawn.actionTimeRemaining = 2f;
                        }
                    }
                    break;

                // Gathering item
                case 1:
                    if (!pawn.performingAction)
                    {
                        foreach (Job job in pawn.jobs)
                        {
                            if (!pawn.TryPickingUpItem((job as Haul).item, ((job as Haul).sourceBuilding as Stockpile)))
                            {
                                // Pawn can't carry this item, free the haul job
                                job.assignedPawn = null;
                                (job as Haul).stockpileB = null;
                            }
                        }
                        pawn.jobStep++;
                    }

                    pawn.Status = "Picking up " + (pawn.jobs[0] as Haul).item.data.name + " in " + (pawn.jobs[0] as Haul).sourceBuilding.name;
                    break;

                // Going to stockpile
                case 2:
                    haulJob = (pawn.jobs[0] as Haul);
                    if ((pawn.transform.position - haulJob.stockpileB.entrance.position).magnitude > haulJob.stockpileB.radius)
                    {
                        if (!pawn.HasPath())
                        {
                            pawn.SetDestination(haulJob.stockpileB.entrance.position);
                        }
                    }
                    else
                    {
                        pawn.StopMoving();
                        pawn.jobStep++;
                        pawn.actionTimeRemaining = 2f;
                    }

                    pawn.Status = "Going to " + haulJob.stockpileB.name;
                    break;

                case 3:
                    haulJob = (pawn.jobs[0] as Haul);
                    if (!pawn.performingAction)
                    {
                        List<Job> jobsToRemove = new List<Job>();
                        foreach (Job job in pawn.jobs)
                        {
                            haulJob = job as Haul;
                            pawn.DropItem(haulJob.item, haulJob.stockpileB);
                            jobsToRemove.Add(haulJob);
                        }

                        JobManager.singleton.RemoveJobs(jobsToRemove);
                        pawn.jobStep = 0;
                    }

                    if (haulJob != null)
                        pawn.Status = "Placing " + haulJob.item.data.name + " in " + ((haulJob.stockpileB) ? haulJob.stockpileB.name : "missing stockpile");
                    break;
            }
        }
    }

    ProductionBuilding GetAcceptableProductionBuilding(Haul haulJob)
    {
        return (ProductionBuilding)BuildingManager.singleton.GetBuilding<ProductionBuilding>(
            delegate (Building building)
            {
                ProductionBuilding pB = (building as ProductionBuilding);

                // Check if building can take this item
                if (pB.GetStorageTotalWegiht() > pB.data.storageCapacity)
                    return false;

                s_Item producedItem = pB.data.producedItem;
                switch (producedItem)
                {
                    // Check if item is an ingredient 
                    case ICraftable craftable:
                        foreach (s_Item ingredient in craftable.GetRecipe())
                        {
                            if (ingredient == haulJob.item.data)
                                return true;
                        }
                        break;
                }

                return false;
            });
    }
}