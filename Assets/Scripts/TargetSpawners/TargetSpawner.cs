using System.Collections.Generic;
using UnityEngine;

public abstract class TargetSpawner : MonoBehaviour
{

    [SerializeField]

    private int spawnRounds;

    [SerializeField]
    private Target normalTarget;

    [SerializeField]
    private Target badTarget;
    [SerializeField]

    private Target bonusTarget;

    [SerializeField]

    private int numberOfBadTargets;

    [SerializeField]

    private int numberOfBonusTargets;
    private int targetsPerSpawnWave = 0;

    private bool spawnerActive = false;

    protected Queue<Target> spawnQueue;

    protected LevelManager parentLevelManager;
    void Start()
    {
        parentLevelManager = GetComponentInParent<LevelManager>();
    }

    void Update()
    {
        UpdateTargetSpawner();
    }

    abstract public void SpawnTargets();

    private List<Target> BuildSpawnQueue()
    {
        List<Target> prefabsToSpawn = new List<Target>();

        for (int i = 0; i < numberOfBadTargets; i++) prefabsToSpawn.Add(badTarget);
        for (int i = 0; i < numberOfBonusTargets; i++) prefabsToSpawn.Add(bonusTarget);
        for (int i = 0; i < (targetsPerSpawnWave * spawnRounds) - numberOfBadTargets - numberOfBonusTargets; i++) prefabsToSpawn.Add(normalTarget);

        Shuffle(prefabsToSpawn);

        return prefabsToSpawn;
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }

    private void SpawnNextRoundOfTargets()
    {
        Target[] allTargets = transform.GetComponentsInChildren<Target>();
        BadTarget[] badTargets = transform.GetComponentsInChildren<BadTarget>();

        if (spawnerActive && (allTargets.Length == 0 || badTargets.Length == allTargets.Length))
        {
            foreach (BadTarget badTarget in badTargets)
            {
                Destroy(badTarget.gameObject);
            }
            SpawnTargets();
        }
    }

    public void StartSpawner()
    {
        spawnQueue = new Queue<Target>(BuildSpawnQueue());
        spawnerActive = true;
        SpawnTargets();
    }

    public void NextSpawner()
    {
        parentLevelManager.StartNextSpawner();
        spawnerActive = false;
    }

    protected private void SetNumberOfTargetsPerSpawnWave(int numberOfTargets)
    {
        targetsPerSpawnWave = numberOfTargets;
    }

    protected private virtual void UpdateTargetSpawner()
    {
        SpawnNextRoundOfTargets();
    }
    public void KillSpawner()
    {
        spawnerActive = false;
        Target[] allTargets = transform.GetComponentsInChildren<Target>();
        StopAllCoroutines();
        foreach (Target target in allTargets)
        {
            Destroy(target.gameObject);
        }
    }

}
