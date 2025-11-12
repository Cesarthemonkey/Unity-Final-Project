using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonTarget : Target
{
    protected AudioSource audioSource;
    [SerializeField] protected AudioClip[] hitSounds;
    [SerializeField] protected AudioClip[] swordSounds;
    [SerializeField] protected AudioClip[] swordSwishSounds;
    [SerializeField] protected PlayerController player;
    [SerializeField] protected GameObject[] gameObjects;
    [SerializeField] protected GameObject spawnParticlePrefab;
    [SerializeField] protected GameObject spawnParticleTransform;
    [SerializeField] protected GameObject[] weapons;
    [SerializeField] protected GameObject NegativeScoreText;
    protected BoxCollider hitbox;

    [SerializeField] protected float enemySpeed;
    [SerializeField] protected float spawnTime;
    protected Animator animator;
    protected NavMeshAgent navMeshAgent;

    private bool moving = false;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = FindFirstObjectByType<PlayerController>();

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
        moving = true;
    }

    private void Update()
    {
        if (player == null || !moving) return;

        navMeshAgent.destination = player.transform.position;

        bool isMoving = navMeshAgent.velocity.magnitude > 0.1f;
        animator.SetBool("Running", isMoving);
    }

    protected override void ShowVFX()
    {
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

    virtual protected IEnumerator SpawnSkeleton()
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
        audioSource.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Length)]);
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
            moving = false;
            StartCoroutine(SkeletonAttacksPlayer());
        }
    }

    protected IEnumerator SkeletonAttacksPlayer()
    {
        navMeshAgent.isStopped = true;
        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(0.75f);

        audioSource.PlayOneShot(swordSwishSounds[Random.Range(0, swordSwishSounds.Length)]);

        yield return new WaitForSeconds(0.35f);
        audioSource.PlayOneShot(swordSounds[Random.Range(0, swordSounds.Length)]);
        GameManager.Instance.resetStreak();
        GameManager.Instance.updateScore(points * -1);
        hitbox.enabled = false;
        NegativeScoreText.GetComponent<TMP_Text>().text = (points * -1).ToString();
        NegativeScoreText.gameObject.SetActive(true);
        CameraManager.Instance.ShakeCamera();

        yield return new WaitForSeconds(0.5f);

        animator.SetBool("Attack", false);
        navMeshAgent.isStopped = false;

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
