using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : Clickable
{
    public List<Job> pendingJobs;
    public Transform entrance;
    public float radius = 1f;

    protected virtual void Awake()
    {
        pendingJobs = new List<Job>();
    }

    void Start()
    {
        BuildingManager.singleton.buildings.Add(this);
    }

    private void OnDrawGizmosSelected()
    {
        if (entrance)
        {
            Color color = Color.red;

            color.a = 0.5f;

            Gizmos.color = color;
            Gizmos.DrawSphere(entrance.position, radius);
        }
    }
}
