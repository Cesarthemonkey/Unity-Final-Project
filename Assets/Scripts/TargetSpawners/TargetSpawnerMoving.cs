using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;

public class TargetSpawnerMoving : TargetSpawner
{
    [Header("Moving Spawner Settings")]
    [SerializeField] private int numberOfTargetsPerSpawnWave = 0;
    [SerializeField] private GameObject spawnLocation;
    [SerializeField] private TargetWayPoint[] targetWayPoints;
    [SerializeField] private float spawnDelay = 0.5f;

    private Queue<Target> respawnQueue;

    void Start()
    {
        respawnQueue = new Queue<Target>();
        SetNumberOfTargetsPerSpawnWave(numberOfTargetsPerSpawnWave);
        parentLevelManager = GetComponentInParent<LevelManager>();

        Debug.Log($"[TargetSpawnerMoving] Initialized on '{name}' | " +
                  $"Targets/Wave: {numberOfTargetsPerSpawnWave}, Spawn Delay: {spawnDelay}s");
    }

    // ------------------------------------------------------------------
    // SPAWN LOGIC
    // ------------------------------------------------------------------
    public override void SpawnTargets()
    {
        Debug.Log($"[TargetSpawnerMoving] === SpawnTargets() Triggered ===");

        StopAllCoroutines();

        if (spawnQueue != null && spawnQueue.Count > 0)
        {
            Debug.Log($"[TargetSpawnerMoving] Starting coroutine to spawn {numberOfTargetsPerSpawnWave} targets...");
            respawnQueue = new Queue<Target>();
            StartCoroutine(SpawnTargetsEveryXSeconds());
        }
        else
        {
            Debug.Log($"[TargetSpawnerMoving] Spawn queue empty — moving to next spawner.");
            NextSpawner();
        }
    }

    private IEnumerator SpawnTargetsEveryXSeconds()
    {
        int targetsToSpawn = numberOfTargetsPerSpawnWave;
        Debug.Log($"[TargetSpawnerMoving] Begin spawning sequence ({targetsToSpawn} targets, {spawnDelay:F2}s delay).");

        while (targetsToSpawn > 0 && spawnQueue.Count > 0)
        {
            Target nextTarget = spawnQueue.Dequeue();
            Target spawnedTarget = Instantiate(nextTarget, spawnLocation.transform.position, spawnLocation.transform.rotation, transform);
            spawnedTarget.SetWayPoint(targetWayPoints[0]);

            Debug.Log($"[TargetSpawnerMoving] Spawned '{spawnedTarget.name}' at {spawnLocation.name} → heading to waypoint '{targetWayPoints[0].name}'");

            yield return new WaitForSeconds(spawnDelay);
            targetsToSpawn--;
        }

        Debug.Log($"[TargetSpawnerMoving] Finished initial spawn cycle. Remaining in queue: {spawnQueue.Count}");
    }

    // ------------------------------------------------------------------
    // WAYPOINT & RESPAWN HANDLING
    // ------------------------------------------------------------------
    public TargetWayPoint GetNextWayPoint(Target target, TargetWayPoint currentWayPoint)
    {
        int index = Array.IndexOf(targetWayPoints, currentWayPoint);

        if (index == -1)
        {
            Debug.LogWarning($"[TargetSpawnerMoving] Target '{target.name}' provided invalid waypoint reference.");
            return null;
        }

        if (index == targetWayPoints.Length - 1)
        {
            Debug.Log($"[TargetSpawnerMoving] '{target.name}' reached final waypoint. Respawning soon...");
            target.transform.position = spawnLocation.transform.position;
            respawnQueue.Enqueue(target);
            return null;
        }

        TargetWayPoint next = targetWayPoints[index + 1];
        Debug.Log($"[TargetSpawnerMoving] '{target.name}' moving from '{currentWayPoint.name}' → '{next.name}'");
        return next;
    }

    // ------------------------------------------------------------------
    // UPDATE LOOP
    // ------------------------------------------------------------------
    private protected override void UpdateTargetSpawner()
    {
        if (respawnQueue.Count == transform.GetComponentsInChildren<Target>().Length && respawnQueue.Count > 0)
        {
            Debug.Log($"[TargetSpawnerMoving] All active targets completed their paths. Respawning...");
            RespawnAllTargets();
        }

        base.UpdateTargetSpawner();
    }

    // ------------------------------------------------------------------
    // RESPAWN LOGIC
    // ------------------------------------------------------------------
    private void RespawnAllTargets()
    {
        Debug.Log($"[TargetSpawnerMoving] Initiating respawn sequence for {respawnQueue.Count} targets.");
        StartCoroutine(RespawnTargetsEveryXSeconds());
    }

    private IEnumerator RespawnTargetsEveryXSeconds()
    {
        Queue<Target> tempQueue = respawnQueue;
        respawnQueue = new Queue<Target>();

        int targetsToRespawn = tempQueue.Count;
        Debug.Log($"[TargetSpawnerMoving] Respawning {targetsToRespawn} targets ({spawnDelay:F2}s delay).");

        while (targetsToRespawn > 0 && tempQueue.Count > 0)
        {
            Target target = tempQueue.Dequeue();
            target.SetWayPoint(targetWayPoints[0]);
            Debug.Log($"[TargetSpawnerMoving] Respawned '{target.name}' → starting waypoint '{targetWayPoints[0].name}'");

            yield return new WaitForSeconds(spawnDelay);
            targetsToRespawn--;
        }

        Debug.Log($"[TargetSpawnerMoving] Respawn cycle complete. Active targets: {transform.GetComponentsInChildren<Target>().Length}");
    }

    // ------------------------------------------------------------------
    // DEBUGGING
    // ------------------------------------------------------------------
    public void LogSpawnerMovingState()
    {
        Debug.Log($"\n[TargetSpawnerMoving] --- STATE SNAPSHOT ---" +
                  $"\n  Spawn Location: {spawnLocation?.name ?? "null"}" +
                  $"\n  Spawn Delay: {spawnDelay:F2}s" +
                  $"\n  Targets/Wave: {numberOfTargetsPerSpawnWave}" +
                  $"\n  Total Waypoints: {targetWayPoints.Length}" +
                  $"\n  Active Children: {transform.GetComponentsInChildren<Target>().Length}" +
                  $"\n  Respawn Queue: {respawnQueue.Count}" +
                  $"\n  Spawn Queue: {(spawnQueue != null ? spawnQueue.Count : 0)}" +
                  $"\n-----------------------------");
    }
}
