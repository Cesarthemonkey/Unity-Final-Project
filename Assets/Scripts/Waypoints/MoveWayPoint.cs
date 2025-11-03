using UnityEngine;

public class MoveWayPoint : MonoBehaviour
{
    public LevelManager parentLevelManager;

    public bool disableRotation = false;

    public float playerSpeed = 5;

    void Start()
    {
        parentLevelManager = GetComponentInParent<LevelManager>();
    }

    public virtual MoveWayPoint GetNextMoveWayPoint()
    {
        return parentLevelManager.UpdateNextWayPoint();
    }
}
