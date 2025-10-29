using UnityEngine;

public class ExplosionMoveWayPoint : MoveWayPoint
{
    [SerializeField]
    private DoorExplosive doorExplosive;
    private PlayerController playerController;
    void Start()
    {
        parentLevelManager = GetComponentInParent<LevelManager>();
        playerController = FindFirstObjectByType<PlayerController>();
    }
    public override MoveWayPoint GetNextMoveWayPoint()
    {
        playerController.Freeze(3f);
        doorExplosive.BlowUp();
        return base.GetNextMoveWayPoint();
    }
}
