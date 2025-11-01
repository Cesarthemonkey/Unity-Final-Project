using UnityEngine;

public class LevelWayPoint : MoveWayPoint
{
    private PlayerController playerController;


    public bool isTargetSpawnArea = false;

    void Start()
    {
        parentLevelManager = GetComponentInParent<LevelManager>();
        playerController = FindFirstObjectByType<PlayerController>();
    }
    public override MoveWayPoint GetNextMoveWayPoint()
    {
        return parentLevelManager.GetNextLevelWayPoint();
    }
}
