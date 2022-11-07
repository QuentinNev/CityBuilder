using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobManager : MonoBehaviour
{
    public static JobManager singleton;

    private void Awake()
    {
        singleton = this;
        jobs = new List<Job>();
    }

    [Sirenix.Serialization.OdinSerialize]
    public List<Job> jobs;

    /// <summary>
    /// Create a new job, gives it an id
    /// </summary>
    /// <param name="newJob"></param>
    public void AddJob(Job newJob)
    {
        newJob.jobID = (uint)jobs.Count;
        jobs.Add(newJob);
    }

    /// <summary>
    /// Remove a job cleanly
    /// </summary>
    /// <param name="jobToRemove"></param>
    public void RemoveJob(Job jobToRemove)
    {
        if (jobToRemove.assignedPawn)
        {
            jobToRemove.assignedPawn.jobs.Remove(jobToRemove);
        }

        jobs.Remove(jobToRemove);
        jobToRemove.sourceBuilding.pendingJobs.Remove(jobToRemove);
    }

    /// <summary>
    /// Remove a job cleanly
    /// </summary>
    /// <param name="jobToRemove"></param>
    public void RemoveJobs(List<Job> jobsToRemove)
    {
        foreach (Job job in jobsToRemove)
        {
            job.assignedPawn?.jobs.Remove(job);
            jobs.Remove(job);
            job.sourceBuilding.pendingJobs.Remove(job);
        }
    }

    /// <summary>
    /// Get the first available of the corresponding type
    /// </summary>
    /// <typeparam name="T">Type of the requested job</typeparam>
    /// <returns>First available job</returns>
    public static T GetAvailableJob<T>() where T : Job
    {
        foreach (Job job in singleton.jobs)
        {
            if (!job.assignedPawn && job is T)
            {
                return job as T;
            }
        }

        return null;
    }

    /// <summary>
    /// Get all available jobs of the corresponding type
    /// </summary>
    /// <typeparam name="T">Type of the requested jobs</typeparam>
    /// <returns></returns>
    public static List<T> GetAvailableJobs<T>() where T : Job
    {
        List<T> foundJobs = new List<T>();
        foreach (Job job in singleton.jobs)
        {
            if (!job.assignedPawn && job is T)
            {
                foundJobs.Add(job as T);
            }
        }

        return foundJobs;
    }
}
