using System.Collections.Generic;
using UnityEngine;

public abstract class TargetSpawner : MonoBehaviour
{
    [SerializeField] private int spawnRounds;
    [SerializeField] private Target normalTarget;
    [SerializeField] private Target badTarget;
    [SerializeField] private Target bonusTarget;
    [SerializeField] private int numberOfBadTargets;
    [SerializeField] private int numberOfBonusTargets;

    protected private int targetsPerSpawnWave = 0;
    private bool spawnerActive = false;

    protected Queue<Target> spawnQueue;
    protected LevelManager parentLevelManager;

    void Start()
    {
        parentLevelManager = GetComponentInParent<LevelManager>();
        Debug.Log($"[TargetSpawner] Initialized on GameObject '{name}'");
    }

    void Update()
    {
        UpdateTargetSpawner();
    }

    public void ResetSpawner()
    {
        spawnerActive = false;

        if (spawnQueue != null)
            spawnQueue.Clear();
        else
            spawnQueue = new Queue<Target>();

        Debug.Log($"[TargetSpawner] Spawner reset to default values. (Active={spawnerActive})");
    }

    public void StartSpawner()
    {
        Debug.Log($"[TargetSpawner] === Starting Spawner ===");
        ResetSpawner();
        LogSpawnerState();

        spawnQueue = new Queue<Target>(BuildSpawnQueue());
        spawnerActive = true;

        Debug.Log($"[TargetSpawner] Spawn queue built with {spawnQueue.Count} entries.");
        SpawnTargets();

        LogSpawnerState();
        Debug.Log($"[TargetSpawner] === Spawner Started ===");
    }

    public void KillSpawner()
    {
        Debug.LogWarning($"[TargetSpawner] !!! KILLING SPAWNER !!!");
        ResetSpawner();
        StopAllCoroutines();

        Target[] allTargets = transform.GetComponentsInChildren<Target>();
        foreach (Target target in allTargets)
            Destroy(target.gameObject);

        Debug.Log($"[TargetSpawner] All active targets destroyed.");
        LogSpawnerState();
    }

    private List<Target> BuildSpawnQueue()
    {
        Debug.Log($"[TargetSpawner] Building spawn queue...");

        List<Target> prefabsToSpawn = new List<Target>();

        for (int i = 0; i < numberOfBadTargets; i++) prefabsToSpawn.Add(badTarget);
        for (int i = 0; i < numberOfBonusTargets; i++) prefabsToSpawn.Add(bonusTarget);
        for (int i = 0; i < (targetsPerSpawnWave * spawnRounds) - numberOfBadTargets - numberOfBonusTargets; i++) 
            prefabsToSpawn.Add(normalTarget);

        Shuffle(prefabsToSpawn);

        Debug.Log($"[TargetSpawner] Spawn queue built: {prefabsToSpawn.Count} total (Bad={numberOfBadTargets}, Bonus={numberOfBonusTargets}, Normal={prefabsToSpawn.Count - numberOfBadTargets - numberOfBonusTargets})");

        return prefabsToSpawn;
    }

    protected private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }

    public void NextSpawner()
    {
        Debug.Log($"[TargetSpawner] Completed spawning — notifying LevelManager to start next spawner.");
        ResetSpawner();
        parentLevelManager.StartNextSpawner();
    }

    private void SpawnNextRoundOfTargets()
    {
        if (spawnerActive && IsOnlyAllGoodTargetsDestroyed())
        {
            Debug.Log($"[TargetSpawner] All good targets destroyed — spawning next round.");
            BadTarget[] badTargets = transform.GetComponentsInChildren<BadTarget>();

            foreach (BadTarget badTarget in badTargets)
                Destroy(badTarget.gameObject);

            SpawnTargets();
        }
    }

    private bool IsOnlyAllGoodTargetsDestroyed()
    {
        Target[] allTargets = transform.GetComponentsInChildren<Target>();
        BadTarget[] badTargets = transform.GetComponentsInChildren<BadTarget>();
        return allTargets.Length == 0 || badTargets.Length == allTargets.Length;
    }

    abstract public void SpawnTargets();

    protected private void SetNumberOfTargetsPerSpawnWave(int numberOfTargets)
    {
        targetsPerSpawnWave = numberOfTargets;
        Debug.Log($"[TargetSpawner] Set targets per spawn wave to {targetsPerSpawnWave}");
    }

    protected private virtual void UpdateTargetSpawner()
    {
        SpawnNextRoundOfTargets();
    }

    public void LogSpawnerState()
    {
        Debug.Log($"\n[TargetSpawner] --- STATE SNAPSHOT ---" +
                  $"\n  Spawn Rounds: {spawnRounds}" +
                  $"\n  Bad Targets: {numberOfBadTargets}" +
                  $"\n  Bonus Targets: {numberOfBonusTargets}" +
                  $"\n  Targets per Wave: {targetsPerSpawnWave}" +
                  $"\n  Spawner Active: {spawnerActive}" +
                  $"\n  Queue Count: {(spawnQueue != null ? spawnQueue.Count.ToString() : "null")}" +
                  $"\n  Parent Level Manager: {(parentLevelManager != null ? parentLevelManager.name : "null")}" +
                  $"\n-----------------------------");
    }
}
