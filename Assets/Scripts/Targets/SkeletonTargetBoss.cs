using System.Collections;
using UnityEngine;
public class SkeletonTargetBoss : SkeletonTarget
{
    public LevelManager parentLevelManager;
    [SerializeField] AudioClip spawnSound;
    [SerializeField] AudioClip scream;

    private void Start()
    {
        parentLevelManager = GetComponentInParent<LevelManager>();
        player = FindFirstObjectByType<PlayerController>();

        audioSource = GetComponent<AudioSource>();
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

        bool isMoving = navMeshAgent.velocity.magnitude > 0.05f;
        animator.SetBool("Walking", isMoving);
    }

    override protected IEnumerator SpawnSkeleton()
    {
        parentLevelManager.pauseTimer = true;
        Vector3 spawnPos = spawnParticleTransform.transform.position;
        spawnPos.y -= 1f;
        audioSource.PlayOneShot(spawnSound);

        GameObject particleInstance = Instantiate(
            spawnParticlePrefab,
            spawnPos,
            spawnParticlePrefab.transform.rotation
        );
        CameraManager.Instance.ShakeCamera();
        Destroy(particleInstance, spawnTime);

        yield return new WaitForSeconds(1f);

        foreach (var item in gameObjects)
        {
            if (item != null)
                item.SetActive(true);
        }
        animator.SetBool("Scream", true);
        yield return new WaitForSeconds(.65f);

        audioSource.PlayOneShot(scream);
        CameraManager.Instance.ShakeCamera();
        yield return new WaitForSeconds(2f);
        animator.SetBool("Scream", false);
        animator.SetBool("Walking", true);
        yield return new WaitForSeconds(1f);
        navMeshAgent.speed = enemySpeed;
        hitbox.enabled = true;
        parentLevelManager.pauseTimer = false;
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
            parentLevelManager.StopLevel();
        }
    }

}
