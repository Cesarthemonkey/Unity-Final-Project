using UnityEngine;

public class Target : MonoBehaviour
{


    [SerializeField]
    private GameObject DestroyParticle;

    [SerializeField]
    private GameObject ScoreText;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    [SerializeField]
    private AudioClip[] destroySounds;

    private AudioSource targetAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshCollider = gameObject.GetComponent<MeshCollider>();
        targetAudio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            ScoreText.SetActive(true);
            meshRenderer.enabled = false;
            meshCollider.enabled = false;
            targetAudio.PlayOneShot(destroySounds[Random.Range(0, destroySounds.Length)], 1.0f);
            Instantiate(DestroyParticle, transform.position, transform.rotation);
            Destroy(gameObject, 1f);
        }
    }

}
