using System.Collections.Generic;
using UnityEngine;

public class TargetSpawnerBasic : TargetSpawner
{
    [SerializeField] protected GameObject[] TargetSpawnLocations;
    [SerializeField] private bool RandomSpawns = false;
    [SerializeField] private int numberOfTargetsPerWave = 0;

    void Start()
    {
        if (!RandomSpawns)
        {
            SetNumberOfTargetsPerSpawnWave(TargetSpawnLocations.Length);
            Debug.Log($"[TargetSpawnerBasic] Initialized with {TargetSpawnLocations.Length} fixed spawn locations.");
        }
        else
        {
            SetNumberOfTargetsPerSpawnWave(numberOfTargetsPerWave);
            Debug.Log($"[TargetSpawnerBasic] Initialized for random spawning of {numberOfTargetsPerWave} targets per wave.");
        }

        parentLevelManager = GetComponentInParent<LevelManager>();
        Debug.Log($"[TargetSpawnerBasic] Parent Level Manager: {(parentLevelManager != null ? parentLevelManager.name : "null")}");
    }

    public override void SpawnTargets()
    {
        Debug.Log($"[TargetSpawnerBasic] === SpawnTargets() Triggered ===");

        if (spawnQueue == null || spawnQueue.Count == 0)
        {
            Debug.Log($"[TargetSpawnerBasic] Spawn queue empty — proceeding to NextSpawner().");
            NextSpawner();
            return;
        }

        if (RandomSpawns)
        {

            Debug.Log($"[TargetSpawnerBasic] Using RANDOM spawn pattern for this wave.");
            SpawnRandomTargets();
        }
        else
        {
            Debug.Log($"[TargetSpawnerBasic] Using STANDARD spawn pattern for this wave.");
            SpawnStandardTargets();
        }

        Debug.Log($"[TargetSpawnerBasic] === Wave Spawn Complete ===");
    }

    protected virtual void SpawnStandardTargets()
    {
                parentLevelManager.PlaySpawnSound();

        if (TargetSpawnLocations == null || TargetSpawnLocations.Length == 0)
        {
            Debug.LogWarning($"[TargetSpawnerBasic] No spawn locations assigned! Aborting standard spawn.");
            return;
        }

        Debug.Log($"[TargetSpawnerBasic] Spawning {TargetSpawnLocations.Length} targets (standard layout).");

        for (int i = 0; i < TargetSpawnLocations.Length && spawnQueue.Count > 0; i++)
        {
            Target nextTarget = spawnQueue.Dequeue();
            Instantiate(nextTarget, 
                        TargetSpawnLocations[i].transform.position, 
                        TargetSpawnLocations[i].transform.rotation, 
                        transform);

            Debug.Log($"[TargetSpawnerBasic] Spawned '{nextTarget.name}' at slot {i} ({TargetSpawnLocations[i].name}).");
        }
    }

    private void SpawnRandomTargets()
    {
        if (TargetSpawnLocations == null || TargetSpawnLocations.Length == 0)
        {
            Debug.LogWarning($"[TargetSpawnerBasic] No spawn locations assigned! Aborting random spawn.");
            return;
        }

        List<GameObject> shuffledSpawns = new List<GameObject>(TargetSpawnLocations);
        Shuffle(shuffledSpawns);

        int targetsToSpawn = Mathf.Min(targetsPerSpawnWave, spawnQueue.Count);
        Debug.Log($"[TargetSpawnerBasic] Random spawning {targetsToSpawn} targets (Queue: {spawnQueue.Count}, Locations: {TargetSpawnLocations.Length})");

        for (int i = 0; i < targetsToSpawn; i++)
        {
            Target nextTarget = spawnQueue.Dequeue();
            GameObject location = shuffledSpawns[i % shuffledSpawns.Count];

            Instantiate(nextTarget, location.transform.position, location.transform.rotation, transform);

            Debug.Log($"[TargetSpawnerBasic] [Random Spawn] -> Spawned '{nextTarget.name}' at location '{location.name}' ({i + 1}/{targetsToSpawn}).");
        }

        Debug.Log($"[TargetSpawnerBasic] Random wave spawn complete. Remaining in queue: {spawnQueue.Count}");
    }
}
