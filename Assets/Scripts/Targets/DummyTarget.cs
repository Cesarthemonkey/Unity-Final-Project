using UnityEngine;

public class DummyTarget : Target
{
    private BoxCollider boxCollider;

    void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();
        targetAudio = gameObject.GetComponent<AudioSource>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        ScoreText = transform.Find("ScoreText").gameObject;
    }

    public override void HideGameObjects()
    {
        meshRenderer.enabled = false;
        boxCollider.enabled = false;
    }
}
