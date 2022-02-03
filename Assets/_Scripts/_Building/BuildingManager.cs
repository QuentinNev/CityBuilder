using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager singleton;

    private void Awake()
    {
        singleton = this;
    }

    public List<Building> buildings;

    /// <summary>
    /// Return the first building of the request type and that pass the filter if there's one
    /// </summary>
    /// <param name="filter">Optional filter</param>
    /// <typeparam name="T">Type of the building</typeparam>
    /// <returns></returns>
    public Building GetBuilding<T>(Func<Building, bool> filter = null)
    {
        foreach (Building building in buildings)
        {
            // Apply the filter if there's one
            if (building is T && (filter == null || filter(building)))
            {
                return building;
            }
        }

        return null;
    }
}
