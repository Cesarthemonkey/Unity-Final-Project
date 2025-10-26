using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    [SerializeField]
    private int points;

    [SerializeField]
    private TMP_Text pointsText;

    [SerializeField]
    private GameObject image;

    void Start()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshCollider = gameObject.GetComponent<MeshCollider>();
        targetAudio = gameObject.GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            DestroyTarget();
        }
    }

    private void DestroyTarget()
    {
        GameManager.Instance.updateScore(points);

        if (points < 0)
        {
            pointsText.text = points.ToString();
        }
        else
        {
            pointsText.text = '+' + points.ToString();

        }
        ScoreText.SetActive(true);
        meshRenderer.enabled = false;
        meshCollider.enabled = false;

        if (image)
        {
            image.SetActive(false);
        }
        targetAudio.PlayOneShot(destroySounds[Random.Range(0, destroySounds.Length)], 1.0f);
        Instantiate(DestroyParticle, transform.position, transform.rotation);
        Destroy(gameObject, 1f);
    }

}
