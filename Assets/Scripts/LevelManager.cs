using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public int duration;
    public string levelName;

    private int countDownTime;

    [SerializeField]
    private MoveWayPoint[] waypoints;

    [SerializeField]
    private PlayerController player;

    private int currentWayPoint = 0;

    private int levelNumber = 0;

    private bool pauseTimer = false;

    [SerializeField]
    private TargetSpawner[] spawners;

    [SerializeField]
    private LevelWayPoint[] levelWayPoints;

    [SerializeField]
    private int timePerLevelWayPoint;


    private int levelWayPointIndex = 0;

    public void InitializeLevel()
    {
        MovePlayer();
        GameInfoController.Instance.levelText.text = levelName;
    }

    public void StartLevel()
    {
        if (spawners.Length > 0)
        {
            spawners[levelNumber].StartSpawner();
        }
        countDownTime = duration;
        StartCoroutine(CountdownToStart());
    }

    

IEnumerator CountdownToStart()
{
    while (countDownTime >= 0)
    {
        // Display the countdown
        GameInfoController.Instance.countDownText.text = countDownTime.ToString();

        // Check if we hit a waypoint interval (and not at start or end)
        if (timePerLevelWayPoint > 0 &&
            countDownTime != duration &&
            countDownTime % timePerLevelWayPoint == 0 &&
            countDownTime != 0)
        {
            pauseTimer = true;
            player.currentWayPoint = levelWayPoints[levelWayPointIndex];
        }

        // Wait until pauseTimer is false before continuing
        while (pauseTimer)
        {
            yield return null; // waits one frame, keeps checking
        }

        // Wait for 1 second, then decrement
        yield return new WaitForSeconds(1f);
        countDownTime--;
    }

    // When countdown ends
    GameManager.Instance.StartNextLevel();
    GameInfoController.Instance.countDownText.text = "Wait";
}


    private void MovePlayer()
    {
        if (waypoints.Length > 0)
        {
            player.currentWayPoint = waypoints[currentWayPoint];
        }
        else
        {
            StartLevel();
        }
    }

    public MoveWayPoint UpdateNextWayPoint()
    {

        currentWayPoint++;

        if (currentWayPoint == waypoints.Length)
        {
            StartLevel();
            return null;
        }

        return waypoints[currentWayPoint];
    }

    public void StartNextSpawner()
    {
        if (levelNumber == spawners.Length - 1)
        {
            levelNumber = 0;
        }
        else
        {
            levelNumber++;
        }

        spawners[levelNumber].StartSpawner();
    }

    public void StopLevel()
    {

        if (spawners.Length > 0)
        {
            spawners[levelNumber].KillSpawner();
        }
    }

    public LevelWayPoint GetNextLevelWayPoint()
    {
        Debug.Log("NEXT");
        if(levelWayPointIndex == levelWayPoints.Length - 1)
        {
            pauseTimer = false;
            return null;
        }

        if (levelWayPoints[levelWayPointIndex].isTargetSpawnArea)
        {
            levelWayPointIndex++;
            pauseTimer = false;
            return null;
        }

        levelWayPointIndex++;
        LevelWayPoint nextWaypoint = levelWayPoints[levelWayPointIndex];
        return nextWaypoint;


    }

}
