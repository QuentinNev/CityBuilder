using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StockpileManager : MonoBehaviour
{
    public static StockpileManager singleton;
    public List<Stockpile> stockpiles;
    public List<ProductionBuilding> harvestables;

    private void Awake()
    {
        singleton = this;
    }

    public Stockpile GetClosestStockpile(Vector3 from, Stockpile prod = null)
    {
        Stockpile closest = null;
        float closestDistance = float.MaxValue;

        foreach (Stockpile stockpile in stockpiles)
        {
            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(from, stockpile.entrance.position, NavMesh.AllAreas, path))
            {
                float distance = 0;
                Vector3 lastPos = from;

                foreach(Vector3 corner in path.corners)
                {
                    distance += (corner - lastPos).magnitude;
                    lastPos = corner;
                }

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = stockpile;
                }
            }
        }

        return closest;
    }    

    public void AddStockpile(Stockpile stockpile)
    {
        stockpiles.Add(stockpile);
    }
    public void RemoveStockpile(Stockpile stockpile)
    {
        stockpiles.Remove(stockpile);
    }

    public void AddHarvestable(ProductionBuilding productionBuilding)
    {
        harvestables.Add(productionBuilding);
    }
    public void RemoveHarvestable(ProductionBuilding productionBuilding)
    {
        harvestables.Remove(productionBuilding);
    }
}
