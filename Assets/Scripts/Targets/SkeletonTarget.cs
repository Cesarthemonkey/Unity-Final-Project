using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonTarget : Target
{
    [SerializeField] protected PlayerController player;
    [SerializeField] protected GameObject[] gameObjects;
    [SerializeField] protected GameObject spawnParticlePrefab;
    [SerializeField] protected GameObject spawnParticleTransform;
    [SerializeField] protected GameObject[] weapons;

    protected BoxCollider hitbox;

    [SerializeField] protected float enemySpeed;
    [SerializeField] protected float spawnTime;
    protected Animator animator;
    protected NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        hitbox = GetComponent<BoxCollider>();
        hitbox.enabled = false;

        navMeshAgent.speed = 0;
        StartCoroutine(SpawnSkeleton());
        HideGameObjects();

        if (weapons.Length > 0)
            weapons[Random.Range(0, weapons.Length)].SetActive(true);

        player = FindFirstObjectByType<PlayerController>();

        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        targetAudio = GetComponent<AudioSource>();
        ScoreText = transform.Find("ScoreText").gameObject;
    }

    private void Update()
    {
        if (player == null) return;

        navMeshAgent.destination = player.transform.position;

        bool isMoving = navMeshAgent.velocity.magnitude > 0.1f;
        animator.SetBool("Running", isMoving);
    }

    protected override void ShowVFX()
    {
        targetAudio.PlayOneShot(destroySounds[Random.Range(0, destroySounds.Length)], 1.0f);
        Instantiate(DestroyParticle, transform.position, DestroyParticle.transform.rotation);
    }

    public override void HideGameObjects()
    {
        foreach (var item in gameObjects)
        {
            if (item != null)
                item.SetActive(false);
        }
    }

    protected IEnumerator SpawnSkeleton()
    {
        Vector3 spawnPos = spawnParticleTransform.transform.position;
        spawnPos.y -= 1f;

        GameObject particleInstance = Instantiate(
            spawnParticlePrefab,
            spawnPos,
            spawnParticlePrefab.transform.rotation
        );
        Destroy(particleInstance, spawnTime);

        yield return new WaitForSeconds(spawnTime);

        foreach (var item in gameObjects)
        {
            if (item != null)
                item.SetActive(true);
        }


        navMeshAgent.speed = enemySpeed;
        hitbox.enabled = true;

    }

    override public void HitTarget()
    {
        totalHits++;
        if (totalHits < hitsToDestroy)
        {
            GameManager.Instance.updateScore(points);
            UpdateStreak();
            DisplayPoints();
        }
        else
        {
            navMeshAgent.speed = 0;
            hit = true;
            UpdateStreak();
            ShowVFX();
            GameManager.Instance.updateScore(points);
            DisplayPoints();
            HideGameObjects();
            DestroyTarget();
            hitbox.enabled = false;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            HitTarget();
        }

        if (other.CompareTag("Player"))
        {
            StartCoroutine(SkeletonAttacksPlayer());
        }
    }

    protected IEnumerator SkeletonAttacksPlayer()
    {
        animator.SetBool("Attack", true);
        navMeshAgent.speed = 0;
        GameManager.Instance.resetStreak();
        GameManager.Instance.updateScore(points * -1);
        hitbox.enabled = false;

        yield return new WaitForSeconds(1f);
        DeSpawnTarget();
    }


    private void DeSpawnTarget()
    {
        Vector3 spawnPos = spawnParticleTransform.transform.position;
        spawnPos.y -= 1f;

        Instantiate(
            DeSpawnParticleTransform,
            spawnPos,
            DeSpawnParticleTransform.transform.rotation
        );
        HideGameObjects();

        DestroyTarget();
    }
}
