using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Camera playerCamera;
    
    [SerializeField] private float maxShootDistance = 100f;
    public float fireRate = 2f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.gameActive)
        {
            Shoot();
        }
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

        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.LookRotation(direction));
    }
}
