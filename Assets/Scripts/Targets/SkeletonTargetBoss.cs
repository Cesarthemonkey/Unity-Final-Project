using UnityEngine;

public class SkeletonTargetBoss : SkeletonTarget
{

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

        bool isMoving = navMeshAgent.velocity.magnitude > 0.05f;
        animator.SetBool("Walking", isMoving);
    }

}
