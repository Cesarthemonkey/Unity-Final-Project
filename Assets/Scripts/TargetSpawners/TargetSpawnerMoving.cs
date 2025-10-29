using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
public class TargetSpawnerMoving : TargetSpawner
{

    [SerializeField]
    private int numberOfTargetsPerSpawnWave = 0;
    [SerializeField] private GameObject spawnLocation;
    [SerializeField] private TargetWayPoint[] targetWayPoints;
    [SerializeField] private List<Target> currentTargets = new List<Target>();
    [SerializeField] private float spawnDelay;

    private Queue<Target> respawnQueue;

    void Start()
    {
        respawnQueue = new Queue<Target>();
        SetNumberOfTargetsPerSpawnWave(numberOfTargetsPerSpawnWave);
        parentLevelManager = GetComponentInParent<LevelManager>();
    }
    public override void SpawnTargets()
    {
        if (spawnQueue.Count > 0)
        {
            respawnQueue = new Queue<Target>();
            currentTargets = new List<Target>();
            StartCoroutine(SpawnTargetsEveryXSeconds());
        }
        else
        {
            NextSpawner();
        }
    }

    IEnumerator SpawnTargetsEveryXSeconds()
    {
        int targetsToSpawn = numberOfTargetsPerSpawnWave;

        while (targetsToSpawn > 0)
        {
            Target nextTarget = spawnQueue.Dequeue();
            Target spawnedTarget = Instantiate(nextTarget, spawnLocation.transform.position, spawnLocation.transform.rotation, transform);
            spawnedTarget.SetWayPoint(targetWayPoints[0]);
            currentTargets.Add(spawnedTarget);
            yield return new WaitForSeconds(spawnDelay);

            targetsToSpawn--;
        }
    }

    public TargetWayPoint GetNextWayPoint(Target target, TargetWayPoint targetWayPoint)
    {

        int index = Array.IndexOf(targetWayPoints, targetWayPoint);
        if (index == targetWayPoints.Length - 1)
        {
            target.gameObject.transform.position = spawnLocation.transform.position;
            respawnQueue.Enqueue(target);


            return null;
        }

        return targetWayPoints[index + 1];
    }

    private protected override void UpdateTargetSpawner()
    {

        if (respawnQueue.Count == transform.GetComponentsInChildren<Target>().Length)
        {
            RespawnAllTargets();
        } 
        
         base.UpdateTargetSpawner();
        
    }

    private void RespawnAllTargets()
    {
        StartCoroutine(RespawnTargetsEveryXSeconds());
    }

    IEnumerator RespawnTargetsEveryXSeconds()
    {
        Queue<Target> tempQueue = respawnQueue;
        respawnQueue = new Queue<Target>();
        int targetsToReSpawn = tempQueue.Count;

        while (targetsToReSpawn > 0)
        {
            Target target = tempQueue.Dequeue();
            target.SetWayPoint(targetWayPoints[0]);
            yield return new WaitForSeconds(spawnDelay);

            targetsToReSpawn--;
        }
    }
}
