using UnityEngine;
using System.Collections;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float maxShootDistance = 100f;

    public MoveWayPoint currentWayPoint;
    public float speed = 5f;
    public float rotationSpeed = 40f;

    private bool isFrozen = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.gameActive)
        {
            Shoot();
        }

        MoveTowardsWayPoint();
    }

    private void Shoot()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, maxShootDistance))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(maxShootDistance);
        }

        Vector3 direction = (targetPoint - shootPoint.position).normalized;

       Instantiate(projectilePrefab, shootPoint.position, Quaternion.LookRotation(direction));
    }

    public void MoveTowardsWayPoint()
    {
        if (currentWayPoint == null || isFrozen) return;

        Vector3 direction = (currentWayPoint.transform.position - transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        float angle = Quaternion.Angle(transform.rotation, targetRotation);

        if (angle <= 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                currentWayPoint.transform.position,
                speed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, currentWayPoint.transform.position) < 0.1f)
            {
                currentWayPoint = currentWayPoint.GetNextMoveWayPoint();
            }
        }
    }

    public void Freeze(float time)
    {
        StartCoroutine(FreezePlayer(time));
    }

    IEnumerator FreezePlayer(float time)
    {
        isFrozen = true;
        float freezeTime = time;
        while (freezeTime > 0)
        {
            yield return new WaitForSeconds(1f);

            freezeTime--;
        }
        isFrozen = false;
    }
}
