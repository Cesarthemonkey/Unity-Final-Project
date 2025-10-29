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

    [SerializeField]
    private TargetSpawner[] spawners;

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
        while (countDownTime > 0)
        {
            GameInfoController.Instance.countDownText.text = countDownTime.ToString();
            yield return new WaitForSeconds(1f);

            countDownTime--;
        }

        GameManager.Instance.StartNextLevel();
        GameInfoController.Instance.countDownText.text = "Wait";
    }

    private void MovePlayer()
    {
        if (waypoints.Length > 0)
        {
            player.currentWayPoint = waypoints[currentWayPoint];
        } else
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

        if(spawners.Length > 0)
        {
            spawners[levelNumber].KillSpawner();
        }
    }

}
