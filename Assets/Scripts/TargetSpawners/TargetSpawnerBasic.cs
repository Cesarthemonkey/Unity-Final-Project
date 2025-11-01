using UnityEngine;

public class TargetSpawnerBasic : TargetSpawner
{

    [SerializeField]
    private GameObject[] TargetSpawnLocations;
    
   [SerializeField]
    private bool RandomSpawns = false;


    void Start()
    {
        SetNumberOfTargetsPerSpawnWave(TargetSpawnLocations.Length);
        parentLevelManager = GetComponentInParent<LevelManager>();
    }

    public override void SpawnTargets()
    {
        if (spawnQueue.Count > 0)
        {
            for (int i = 0; i < TargetSpawnLocations.Length; i++)
            {
                Target nextTarget = spawnQueue.Dequeue();
                Instantiate(nextTarget, TargetSpawnLocations[i].transform.position, TargetSpawnLocations[i].transform.rotation, transform);
            }
        }
        else
        {
            NextSpawner();
        }
    }
}
