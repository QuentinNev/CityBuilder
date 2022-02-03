using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

/// <summary>
/// This class represent a pawn by it's animator, stats and navMeshAgent
/// </summary>
public class Pawn : Clickable
{
    #region S T A T S

    // H E A L T H
    public float baseHealth
    {
        get
        {
            return 100 + (constitution * 10);
        }
    }
    float currentHealth = 100;
    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }

        set
        {
            currentHealth = value;
            OnHealthUpdated(EventArgs.Empty, currentHealth);
        }
    }

    // S T R
    public int strength
    {
        get
        {
            return baseStrength + profession.bonusStrength;
        }
    }
    public int baseStrength;

    // C O N
    public int constitution
    {
        get
        {
            return baseConstitution + profession.bonusConstitution;
        }
    }
    public int baseConstitution;

    // A G I
    public int agility
    {
        get
        {
            return baseAgility + profession.bonusAgility;
        }
    }
    public int baseAgility;

    // I N T
    public int intelligence
    {
        get
        {
            return baseIntelligence + profession.bonusIntelligence;
        }
    }
    public int baseIntelligence;

    // C H A
    public int charisma
    {
        get
        {
            return baseCharisma + profession.bonusCharisma;
        }
    }
    public int baseCharisma;
    #endregion

    #region Leveling
    public const int MAX_LEVEL = 10;
    public int level;
    public int experience;
    #endregion

    #region Profession
    public s_Profession profession;
    public List<Job> jobs;
    public Job currentJob
    {
        get
        {
            if (jobs.Count > 0)
                return jobs[0];

            return null;
        }
    }

    public int jobStep = 0;
    int waitForJob = 0;
    public void DelayJob() { waitForJob = 10; }

    string status = "Doing nothing";
    public string Status
    {
        get
        {
            return status;
        }

        set
        {
            if (status != value)
            {
                status = value;
                OnPawnStatusUpdated(EventArgs.Empty, status);
            }
        }
    }

    /// <summary>
    /// Set a new profession to a pawn.
    /// </summary>
    /// <param name="newProfession"></param>
    public void SetNewProfession(s_Profession newProfession)
    {
        profession = newProfession;
    }

    public void RefreshUI()
    {
        OnPawnStatusUpdated(EventArgs.Empty, status);
        OnHealthUpdated(EventArgs.Empty, currentHealth);
        OnPawnInventoryUpdated(EventArgs.Empty);
    }

    #endregion

    #region Animation
    private Animator _animator;
    public bool performingAction;
    public float actionTimeRemaining;
    #endregion

    #region Inventory
    public List<Item> inventory;
    public float invTotalWeight = 0;
    public float maxWeight
    {
        get
        {
            return 10 + (strength * 2);
        }
    }

    /// <summary>
    /// Check if pawn can carry this item
    /// </summary>
    /// <param name="item">Item to fit in inventory</param>
    /// <returns></returns>
    public bool CanPickUpItem(Item item)
    {
        return (invTotalWeight + item.data.weight < maxWeight);
    }

    /// <summary>
    /// Try to pick up item in inventory
    /// </summary>
    /// <param name="item">Item to pick up</param>
    /// <param name="building">Building in which the item is stored in</param>
    /// <returns>Succeed</returns>
    public bool TryPickingUpItem(Item item, Stockpile building)
    {
        if (invTotalWeight + item.data.weight < maxWeight)
        {
            invTotalWeight += item.data.weight;
            inventory.Add(item);

            switch (building)
            {
                case ProductionBuilding productionBuilding:
                    productionBuilding.RemoveItemFromStorage(item);
                    break;

                case Stockpile stockpile:
                    stockpile.RemoveItemFromStorage(item);
                    break;

                default:
                    break;
            }

            OnPawnInventoryUpdated(EventArgs.Empty);
            return true;
        }

        Debug.Log("My inventory is too full to pick up : " + item.data.name);

        return false;
    }

    /// <summary>
    /// Try to drop an item in a stockpile
    /// </summary>
    /// <param name="item">Item to drop</param>
    /// <param name="targetStorage">Stockpile to store the item in</param>
    public void DropItem(Item item, Stockpile targetStorage)
    {
        if (!targetStorage)
        {
            targetStorage = StockpileManager.singleton.GetClosestStockpile(transform.position);
        }

        targetStorage.AddItemToStorage(item);
        invTotalWeight -= item.data.weight;
        inventory.Remove(item);
        OnPawnInventoryUpdated(EventArgs.Empty);
    }
    #endregion

    #region Movement
    [HideInInspector]
    public float currentMoveSpeed;
    public const float MAX_MOVE_SPEED = 7.5f;
    private float _animationSpeed;
    private NavMeshAgent _navMeshAgent;
    private Vector3 _lastPosition;

    public void SetDestination(Vector3 desination)
    {
        _animator.SetTrigger("StopAction");
        _navMeshAgent.SetDestination(desination);
    }

    public bool HasPath()
    {
        return _navMeshAgent.hasPath;
    }

    public void StopMoving()
    {
        _navMeshAgent.ResetPath();
    }

    public bool CheckIfDestinationIsReachable(Vector3 destination)
    {
        NavMeshPath path = new NavMeshPath();
        return _navMeshAgent.CalculatePath(destination, path);
    }
    #endregion

    #region Events

    public event EventHandler HealthUpdate;
    protected virtual void OnHealthUpdated(EventArgs e, float currentHealth)
    {
        HealthUpdate?.Invoke(this, e);
    }

    public event EventHandler StatusUpdate;
    protected virtual void OnPawnStatusUpdated(EventArgs e, string status)
    {
        this.status = status;
        StatusUpdate?.Invoke(this, e);
    }


    public event EventHandler InventoryUpdate;
    protected virtual void OnPawnInventoryUpdated(EventArgs e)
    {
        InventoryUpdate?.Invoke(this, e);
    }
    #endregion

    void Awake()
    {
        inventory = new List<Item>();
        jobs = new List<Job>();
    }

    void Start()
    {
        PawnManager.singleton.pawns.Add(this);

        if (!_animator)
        {
            _animator = GetComponent<Animator>();
        }

        if (!_navMeshAgent)
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.speed = MAX_MOVE_SPEED;
        }

        if (!profession)
            profession = PawnManager.singleton.defaultProfession;

        currentHealth = baseHealth;
    }

    public void UpdateBehavior()
    {
        // Delay job for a few frame to avoid spamming
        if (waitForJob > 0)
            waitForJob--;
        else
            profession.DoJob(this);

        currentMoveSpeed = (_lastPosition - transform.position).magnitude / Time.deltaTime;

        _animationSpeed = Mathf.Lerp(_animationSpeed, Mathf.InverseLerp(0, MAX_MOVE_SPEED, currentMoveSpeed), Time.deltaTime * 10f);

        _animator.SetFloat("MoveSpeed", _animationSpeed);

        _lastPosition = transform.position;

        if (actionTimeRemaining > 0)
        {
            if (!performingAction)
            {
                _animator.ResetTrigger("StopAction"); // Why is this even needed ?
                _animator.SetTrigger("Gather");
                performingAction = true;
            }

            actionTimeRemaining -= Time.deltaTime;
        }
        else if (performingAction)
        {
            performingAction = false;
            _animator.SetTrigger("StopAction");
        }
    }

    private void OnDestroy()
    {
        foreach (Job job in jobs)
            job.assignedPawn = null;

        PawnManager.singleton.pawns.Remove(this);
    }
}