using UnityEngine;

public class TargetWayPoint : MonoBehaviour
{
    public TargetSpawnerMoving targetSpawner;
    void Start()
    {
        targetSpawner = GetComponentInParent<TargetSpawnerMoving>();
    }

    public virtual TargetWayPoint GetNextMoveWayPoint(Target target)
    {
        return targetSpawner.GetNextWayPoint(target, this);
    }
}
