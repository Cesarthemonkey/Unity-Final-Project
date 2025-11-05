using System.Collections;
using UnityEngine;

public class MovingLevelManager : LevelManager
{

    private LevelWayPoint currentWayPointSpawn;

    override public void StartLevel()
    {
        Debug.Log($"[LevelManager - Moving] === STARTING LEVEL: '{levelName}' ===");

        countDownTime = duration;
        player.speed = playerSpeed;
        StartCoroutine(CountdownToStart());
    }

    override protected IEnumerator CountdownToStart()
    {
        while (countDownTime > 0)
        {
            GameInfoController.Instance.countDownText.text = countDownTime.ToString();

            if (countDownTime == duration)
            {
                yield return null;
         
                player.currentWayPoint = levelWayPoints[levelAreaIndex];
            }



            while (pauseTimer)
                yield return null;

            yield return new WaitForSeconds(1f);
            countDownTime--;
        }

        Debug.Log($"[LevelManager] Level '{levelName}' timer finished. Transitioning...");
        GameInfoController.Instance.countDownText.text = "Wait";
        GameManager.Instance.StartNextLevel();
    }

    private void StartSpawnerForArea(LevelWayPoint currentWayPoint)
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            if (spawners[i].parentLevelWayPoint == currentWayPoint)
            {
                spawners[i].StartSpawner();
            }
            else
            {
                spawners[i].KillSpawner();
            }
        }
    }

    override public LevelWayPoint GetNextLevelWayPoint()
    {

        levelAreaIndex++;
        
        if(levelAreaIndex == levelWayPoints.Length)
        {
            return null;
        }


        LevelWayPoint nextWaypoint = levelWayPoints[levelAreaIndex];
        if (nextWaypoint.isTargetSpawnArea)
        {
            currentWayPointSpawn = nextWaypoint;
            StartSpawnerForArea(currentWayPointSpawn);
            Debug.Log($"[LevelManager] Waypoint is a spawn area — Starting Target Spawners!");
        }

        Debug.Log($"[LevelManager] Advancing to Level WayPoint #{levelAreaIndex}: {nextWaypoint.name}");
        return nextWaypoint;
    }

    public override void StartNextSpawner()
    {
        StartSpawnerForArea(currentWayPointSpawn);
    }

    public override void StopLevel()
    {
        Debug.LogWarning($"[LevelManager] === STOPPING LEVEL '{levelName}' ===");

        if (spawners != null && spawners.Length > 0)
        {
            Debug.Log($"[LevelManager] Killing current spawner: Index {spawnerIndex}");
            for (int i = 0; i < spawners.Length; i++)
            {
                spawners[i].KillSpawner();
            }
        }
        else
        {
            Debug.LogWarning($"[LevelManager] No active spawner to stop.");
        }
    }


}
