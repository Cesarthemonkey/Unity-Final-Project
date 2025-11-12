using UnityEngine;

public class VaseTarget : Target
{
    private BoxCollider boxCollider;


    [SerializeField]
    private GameObject[] vasePrefabs;

    private GameObject selectedVase;

    void Start()
    {
        transform.Find("Vase1").gameObject.SetActive(false);
        boxCollider = gameObject.GetComponent<BoxCollider>();
        targetAudio = gameObject.GetComponent<AudioSource>();
        ScoreText = transform.Find("ScoreText").gameObject;
        player = FindFirstObjectByType<PlayerController>();

        selectedVase = vasePrefabs[Random.Range(0, vasePrefabs.Length)];
        selectedVase.SetActive(true);
        meshRenderer = selectedVase.GetComponent<MeshRenderer>();

    }

    public override void HideGameObjects()
    {
        meshRenderer.enabled = false;
        boxCollider.enabled = false;
    }
}
