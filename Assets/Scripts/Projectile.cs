using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float launchForce = 3000f;
    public float lifetime = 10f;
    public float gravityScale = 1f;

    public GameObject destroyParticle;
    public GameObject ParticleSpawn;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * launchForce);

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {

            if (!other.CompareTag("Target"))
            {
                Instantiate(destroyParticle, ParticleSpawn.transform.position, other.transform.rotation);
            }
            Destroy(gameObject);
        }
    }

}
