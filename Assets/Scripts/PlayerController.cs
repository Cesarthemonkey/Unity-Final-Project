using UnityEngine;
using System.Collections;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float maxShootDistance = 100f;

    [SerializeField] private AudioClip[] bowSounds;

    private AudioSource audioSource;
    public MoveWayPoint currentWayPoint;
    public float speed = 3f;
    public float rotationSpeed = 40f;

    private bool isFrozen = false;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    } 
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

        audioSource.PlayOneShot(bowSounds[Random.Range(0, bowSounds.Length)]);  
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

        // Keep player's Y position fixed
        // if (currentWayPoint.playerSpeed > 0)
        // {
        //     speed = currentWayPoint.playerSpeed;
        // }


        Vector3 targetPosition = currentWayPoint.transform.position;
        targetPosition.y = transform.position.y;

        // Direction ignoring Y
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        if (!currentWayPoint.disableRotation)
        {
            // Rotate towards the target
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

        }

        float angle = Quaternion.Angle(transform.rotation, targetRotation);

        if (angle <= 0 || currentWayPoint.disableRotation)
        {
            if (!currentWayPoint.disableRotation)
            {
                transform.rotation = targetRotation;
            }
            transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

            // Check distance using the flattened position
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
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
