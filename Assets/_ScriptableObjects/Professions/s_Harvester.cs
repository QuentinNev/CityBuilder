using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Profession/Transporter", order = 1)]
public class s_Harvester : s_Profession
{
    public override void DoJob(Pawn pawn)
    {
        if (pawn.currentJob == null)
        {
            Produce productionJob = JobManager.GetAvailableJob<Produce>();

            if (productionJob != null)
            {
                if (pawn.CheckIfDestinationIsReachable(productionJob.sourceBuilding.entrance.position))
                {
                    productionJob.assignedPawn = pawn;
                    pawn.jobs.Add(productionJob);
                }
            }

            pawn.Status = "Looking for a job";
        }
        else
        {
            ProductionBuilding building = (pawn.currentJob as Produce).sourceBuilding as ProductionBuilding;
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
                    building.Produce(pawn);
                    pawn.Status = "Producing " + building.data.producedItem.name;
                    break;
            }
        }
    }
}
