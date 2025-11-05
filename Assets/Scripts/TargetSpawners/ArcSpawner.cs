using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArcSpawner : TargetSpawnerBasic
{
    private List<Target> spawnedObjects = new List<Target>();
    
    [SerializeField]    private bool upForce;

    override protected void SpawnStandardTargets()
    {
        spawnedObjects = new List<Target>();
        if (TargetSpawnLocations == null || TargetSpawnLocations.Length == 0)
        {
            Debug.LogWarning($"[TargetSpawnerBasic] No spawn locations assigned! Aborting standard spawn.");
            return;
        }

        Debug.Log($"[TargetSpawnerBasic] Spawning ARC {TargetSpawnLocations.Length} targets (standard layout).");

        for (int i = 0; i < TargetSpawnLocations.Length && spawnQueue.Count > 0; i++)
        {
            StartCoroutine(SpawnTargetsRandomly(i));
        }
    }

    private IEnumerator SpawnTargetsRandomly(int i)
    {
        float waitTime = Random.Range(0.5f, 3f);
        yield return new WaitForSeconds(waitTime);
        Target nextTarget = spawnQueue.Dequeue();
        Target x = Instantiate(
            nextTarget,
            TargetSpawnLocations[i].transform.position,
            TargetSpawnLocations[i].transform.rotation,
            transform
        );
        spawnedObjects.Add(x);


        Rigidbody targetRb = x.GetComponent<Rigidbody>();


        if (upForce)
        {
            Vector3 launchDirection = (transform.up * 1f).normalized;
            targetRb.AddForce(launchDirection * 7f, ForceMode.Impulse);


        }
        else
        {
            Vector3 launchDirection = (-transform.forward + transform.up * 1f).normalized;
            targetRb.AddForce(launchDirection * 7f, ForceMode.Impulse);
            targetRb.AddTorque(transform.forward * 6f, ForceMode.Force);
        }


        targetRb.useGravity = true;

        Debug.Log($"[TargetSpawnerBasic] Spawned '{nextTarget.name}' at slot {i} ({TargetSpawnLocations[i].name}) after waiting {waitTime:F2}s.");
    }

    protected override void SpawnNextRoundOfTargets()
    {
        if (spawnedObjects.Count == TargetSpawnLocations.Length)
        {
            float yThreshold = -1f;

            bool allLessThanThreshold = spawnedObjects.All(obj =>
                obj == null || obj.transform.position.y < yThreshold
            );

            Debug.Log($"All below {yThreshold}: {allLessThanThreshold}");

            if (allLessThanThreshold)
            {
                foreach (var obj in spawnedObjects.Where(o => o != null))
                {
                    Destroy(obj.gameObject);
                }

                SpawnStandardTargets();
                spawnedObjects = new List<Target>();
            }
        }
    }


}
