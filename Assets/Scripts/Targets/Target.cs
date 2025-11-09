using UnityEngine;
using TMPro;

public class Target : MonoBehaviour
{
    [SerializeField]
    protected GameObject DestroyParticle;

    [SerializeField]
    protected GameObject DeSpawnParticleTransform;

    [SerializeField]
    protected AudioClip[] destroySounds;

    [SerializeField]
    protected int points;


    [SerializeField]
    protected int hitsToDestroy = 1;

    [SerializeField]
    protected int totalHits = 0;
    protected MeshRenderer meshRenderer;
    protected MeshCollider meshCollider;
    protected AudioSource targetAudio;
    protected GameObject ScoreText;

    private TargetWayPoint currentWayPoint;

    public float speed = 5;

    protected bool hit = false;

    void Start()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshCollider = gameObject.GetComponent<MeshCollider>();
        targetAudio = gameObject.GetComponent<AudioSource>();
        ScoreText = transform.Find("ScoreText").gameObject;
    }

    void Update()
    {
        MoveTowardsWayPoint();
    }
     void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            HitTarget();
        }
    }

    public virtual void HitTarget()
    {
        hit = true;
        UpdateStreak();
        ShowVFX();
        GameManager.Instance.updateScore(points);
        DisplayPoints();
        HideGameObjects();
        DestroyTarget();
    }

    protected void DestroyTarget()
    {
        Destroy(gameObject, 1f);
    }

    protected void DisplayPoints()
    {
        if (points < 0)
        {
            ScoreText.GetComponent<TMP_Text>().text = points.ToString();
        }
        else
        {

            if (GameManager.Instance.streak > 1)
            {
                ScoreText.GetComponent<TMP_Text>().text = '+' + points.ToString() + " x" + GameManager.Instance.streak.ToString();
            }
            else
            {
                ScoreText.GetComponent<TMP_Text>().text = '+' + points.ToString();
            }

        }
        ScoreText.SetActive(true);
    }

    public virtual void HideGameObjects()
    {
        if (meshCollider)
        {
            meshCollider.enabled = false;

        }

        if (meshRenderer)
        {
            meshRenderer.enabled = false;

        }
    }

    virtual protected void ShowVFX()
    {
        targetAudio.PlayOneShot(destroySounds[Random.Range(0, destroySounds.Length - 1)], 1.0f);
        Instantiate(DestroyParticle, transform.position, transform.rotation);
    }

    public void SetWayPoint(TargetWayPoint waypoint)
    {
        currentWayPoint = waypoint;
    }

    public void MoveTowardsWayPoint()
    {
        if (currentWayPoint == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            currentWayPoint.transform.position,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, currentWayPoint.transform.position) < 0.1f && !hit)
        {
            currentWayPoint = currentWayPoint.GetNextMoveWayPoint(this);
        }
    }

    public virtual void UpdateStreak() { }

}
