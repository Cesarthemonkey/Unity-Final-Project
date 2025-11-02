using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    public int duration;
    public string levelName;

    private int countDownTime;
    private bool pauseTimer = false;

    [Header("Waypoints & Player")]
    [SerializeField] private MoveWayPoint[] waypoints;
    [SerializeField] private PlayerController player;
    private int currentWayPoint = 0;

    [Header("Spawners & Areas")]
    [SerializeField] private TargetSpawner[] spawners;
    [SerializeField] private LevelWayPoint[] levelWayPoints;
    [SerializeField] private int timePerLevelWayPoint;
    private int spawnerIndex = 0;
    private int levelAreaIndex = 0;

    // ------------------------------------------------------------------
    // LEVEL FLOW
    // ------------------------------------------------------------------
    public void InitializeLevel()
    {
        Debug.Log($"[LevelManager] === INITIALIZING LEVEL: '{levelName}' ===");

        MovePlayer();
        GameInfoController.Instance.levelText.text = levelName;

        Debug.Log($"[LevelManager] Player positioned at waypoint {currentWayPoint} / {waypoints.Length}");
        Debug.Log($"[LevelManager] Level duration: {duration}s | Spawners: {spawners.Length}");
    }

    public void StartLevel()
    {
        Debug.Log($"[LevelManager] === STARTING LEVEL: '{levelName}' ===");

        countDownTime = duration;
        StartCoroutine(CountdownToStart());
    }

    private IEnumerator CountdownToStart()
    {
        if (spawners != null && spawners.Length > 0 &&
            spawnerIndex >= 0 && spawnerIndex < spawners.Length)
        {
            Debug.Log($"[LevelManager] Starting first spawner (Index {spawnerIndex})...");
            spawners[spawnerIndex].StartSpawner();
        }
        else
        {
            Debug.LogWarning($"[LevelManager] No valid spawners configured! Level will still count down.");
        }

        while (countDownTime > 0)
        {
            GameInfoController.Instance.countDownText.text = countDownTime.ToString();

            if (timePerLevelWayPoint > 0 &&
                countDownTime != duration &&
                countDownTime % timePerLevelWayPoint == 0)
            {
                Debug.Log($"[LevelManager] Time checkpoint reached at {countDownTime}s. Moving to next level waypoint...");
                pauseTimer = true;

                player.currentWayPoint = levelWayPoints[levelAreaIndex];
                Debug.Log($"[LevelManager] Player moved to Level WayPoint #{levelAreaIndex} ({levelWayPoints[levelAreaIndex].name})");

                if (spawners != null && spawners.Length > 0)
                {
                    spawners[spawnerIndex].KillSpawner();
                    spawnerIndex++;
                }
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

    // ------------------------------------------------------------------
    // PLAYER / WAYPOINT CONTROL
    // ------------------------------------------------------------------
    private void MovePlayer()
    {
        if (waypoints != null && waypoints.Length > 0)
        {
            player.currentWayPoint = waypoints[currentWayPoint];
            Debug.Log($"[LevelManager] Player set to initial waypoint: {waypoints[currentWayPoint].name}");
        }
        else
        {
            Debug.LogWarning($"[LevelManager] No waypoints found! Starting level immediately.");
            StartLevel();
        }
    }

    public MoveWayPoint UpdateNextWayPoint()
    {
        currentWayPoint++;

        if (currentWayPoint >= waypoints.Length)
        {
            Debug.Log($"[LevelManager] Final waypoint reached. Starting level phase.");
            StartLevel();
            return null;
        }

        Debug.Log($"[LevelManager] Advancing to next waypoint: {waypoints[currentWayPoint].name} ({currentWayPoint}/{waypoints.Length})");
        return waypoints[currentWayPoint];
    }

    // ------------------------------------------------------------------
    // SPAWNER MANAGEMENT
    // ------------------------------------------------------------------
    public void StartNextSpawner()
    {
        Debug.Log($"[LevelManager] === Starting Next Spawner ===");

        if (spawners == null || spawners.Length == 0)
        {
            Debug.LogWarning($"[LevelManager] No spawners available to start!");
            return;
        }

        spawnerIndex = (spawnerIndex + 1) % spawners.Length;

        Debug.Log($"[LevelManager] Activating Spawner #{spawnerIndex} ({spawners[spawnerIndex].name})");
        spawners[spawnerIndex].StartSpawner();
    }

    public void StopLevel()
    {
        Debug.LogWarning($"[LevelManager] === STOPPING LEVEL '{levelName}' ===");

        if (spawners != null && spawners.Length > 0)
        {
            Debug.Log($"[LevelManager] Killing current spawner: Index {spawnerIndex}");
            spawners[spawnerIndex].KillSpawner();
        }
        else
        {
            Debug.LogWarning($"[LevelManager] No active spawner to stop.");
        }
    }

    // ------------------------------------------------------------------
    // LEVEL WAYPOINT PROGRESSION
    // ------------------------------------------------------------------
    public LevelWayPoint GetNextLevelWayPoint()
    {
        if (levelWayPoints == null || levelWayPoints.Length == 0)
        {
            Debug.LogWarning($"[LevelManager] No level waypoints configured!");
            pauseTimer = false;
            return null;
        }

        if (levelAreaIndex >= levelWayPoints.Length - 1)
        {
            Debug.Log($"[LevelManager] Final level waypoint reached. Resuming countdown.");
            pauseTimer = false;
            return null;
        }

        LevelWayPoint currentWayPoint = levelWayPoints[levelAreaIndex];

        if (currentWayPoint.isTargetSpawnArea)
        {
            levelAreaIndex++;
            spawners[spawnerIndex].StartSpawner();
            Debug.Log($"[LevelManager] Waypoint is a spawn area — resuming timer and skipping direct movement.");
            pauseTimer = false;
            return null;
        }

        levelAreaIndex++;
        LevelWayPoint nextWaypoint = levelWayPoints[levelAreaIndex];
        Debug.Log($"[LevelManager] Advancing to Level WayPoint #{levelAreaIndex}: {nextWaypoint.name}");
        return nextWaypoint;
    }

    // ------------------------------------------------------------------
    // DEBUG UTILITIES
    // ------------------------------------------------------------------
    public void LogLevelState()
    {
        Debug.Log($"\n[LevelManager] --- STATE SNAPSHOT ---" +
                  $"\n  Level Name: {levelName}" +
                  $"\n  Duration: {duration}s" +
                  $"\n  Countdown: {countDownTime}s" +
                  $"\n  Current Waypoint: {currentWayPoint}/{waypoints.Length}" +
                  $"\n  Current Spawner Index: {spawnerIndex}" +
                  $"\n  Pause Timer: {pauseTimer}" +
                  $"\n  Level Area Index: {levelAreaIndex}" +
                  $"\n  Total Spawners: {(spawners != null ? spawners.Length : 0)}" +
                  $"\n  Total Level WayPoints: {(levelWayPoints != null ? levelWayPoints.Length : 0)}" +
                  $"\n-----------------------------");
    }
}
