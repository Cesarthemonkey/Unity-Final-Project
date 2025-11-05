using UnityEngine;
using UnityEngine.AI;

public class SkeletonTarget : Target
{

    [SerializeField] protected private PlayerController player;
    [SerializeField] private GameObject[] gameObjects;
    private Animator animator;

    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        animator = GetComponent<Animator>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshCollider = gameObject.GetComponent<MeshCollider>();
        targetAudio = gameObject.GetComponent<AudioSource>();
        ScoreText = transform.Find("ScoreText").gameObject;
    }

    void Update()
    {
        navMeshAgent.destination = player.transform.position;


        if (navMeshAgent.velocity.magnitude != 0f)
        {
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);

        }

    }

    override protected void ShowVFX()
    {
        targetAudio.PlayOneShot(destroySounds[Random.Range(0, destroySounds.Length - 1)], 1.0f);
        Instantiate(DestroyParticle, transform.position, DestroyParticle.transform.rotation);
    }
    public override void HideGameObjects()
    {
        navMeshAgent.velocity = Vector3.zero;
        foreach (var item in gameObjects)
        {
            item.SetActive(false);
        }
    }
}
