using UnityEngine;

public class MoveWayPoint : MonoBehaviour
{
    public LevelManager parentLevelManager;
    void Start()
    {
        parentLevelManager = GetComponentInParent<LevelManager>();
    }

    public virtual MoveWayPoint GetNextMoveWayPoint()
    {
        return parentLevelManager.UpdateNextWayPoint();
    }
}
