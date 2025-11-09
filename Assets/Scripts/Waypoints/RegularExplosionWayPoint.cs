using UnityEngine;
using System.Collections;

public class RegularExplosionWayPoint : MoveWayPoint
{
    [SerializeField] private float explosionRadius = 5.0f;
    [SerializeField] private float explosionForce = 300f;
    [SerializeField] private float upwardsModifier = 2.0f;
    [SerializeField] private float timeDelay = 3f;

    [SerializeField] private GameObject Explosion;
    [SerializeField] private GameObject ExplosionPoint;
    [SerializeField] private GameObject[] Destructurables;
    [SerializeField] private GameObject[] Debris;
    [SerializeField] private GameObject SmokeSource;

    [SerializeField] private AudioClip boomClip;
    protected PlayerController playerController;

    protected AudioSource audioSource;
    void Start()
    {
        parentLevelManager = GetComponentInParent<LevelManager>();
        playerController = FindFirstObjectByType<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    public override MoveWayPoint GetNextMoveWayPoint()
    {
        playerController.Freeze(3f + timeDelay);
        StartCoroutine(DelayedExplosion());
        return base.GetNextMoveWayPoint();
    }

    private IEnumerator DelayedExplosion()
    {
        GameObject smokeSource = Instantiate(SmokeSource, ExplosionPoint.transform.position, SmokeSource.transform.rotation);

        yield return new WaitForSeconds(timeDelay);

        if (Explosion != null && ExplosionPoint != null)
            Instantiate(Explosion, ExplosionPoint.transform.position, ExplosionPoint.transform.rotation);

        audioSource.PlayOneShot(boomClip);


        foreach (var item in Debris)
        {
            Instantiate(item, ExplosionPoint.transform.position, item.transform.rotation);

        }
        Vector3 explosionPosition = ExplosionPoint.transform.position;

        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);
        foreach (Collider hit in colliders)
        {
            if (!hit.CompareTag("Player"))
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, upwardsModifier);
            }
        }

        foreach (var item in Destructurables)
        {
            if (item != null)
                Destroy(item);
        }

        if (ExplosionPoint != null)
            Destroy(ExplosionPoint);

        Destroy(smokeSource);
    }
}
