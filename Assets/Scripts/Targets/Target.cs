using UnityEngine;
using TMPro;
using NUnit.Framework.Interfaces;

public class Target : MonoBehaviour
{
    [SerializeField]
    private GameObject DestroyParticle;

    [SerializeField]
    private AudioClip[] destroySounds;

    [SerializeField]
    private int points;

    protected MeshRenderer meshRenderer;
    protected MeshCollider meshCollider;
    protected AudioSource targetAudio;
    protected GameObject ScoreText;

    void Start()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshCollider = gameObject.GetComponent<MeshCollider>();
        targetAudio = gameObject.GetComponent<AudioSource>();
        ScoreText = transform.Find("ScoreText").gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        ShowVFX();
        GameManager.Instance.updateScore(points);
        DisplayPoints();
        HideGameObjects();
        DestroyTarget();
    }         

    private void DestroyTarget()
    {
        Destroy(gameObject, 1f);
    }

    private void DisplayPoints()
    {
        if (points < 0)
        {
            ScoreText.GetComponent<TMP_Text>().text = points.ToString();
        }
        else
        {
            ScoreText.GetComponent<TMP_Text>().text = '+' + points.ToString();
        }
        ScoreText.SetActive(true);
    }

    public virtual void HideGameObjects()
    {
        meshRenderer.enabled = false;
        meshCollider.enabled = false;
    }
    
    private void ShowVFX()
    {
        targetAudio.PlayOneShot(destroySounds[Random.Range(0, destroySounds.Length)], 1.0f);
        Instantiate(DestroyParticle, transform.position, transform.rotation);
    }

}
