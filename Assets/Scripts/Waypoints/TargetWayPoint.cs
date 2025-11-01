public class TargetWayPoint : MoveWayPoint
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
