using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private Vector3 cameraOffset = new Vector3(0, .5f, 0);
    public bool shake = false;
    public AnimationCurve curve;
    public float duration = 1f;
    public static CameraManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Update(){}

    public void ShakeCamera()
    {
        shake = true;
        StartCoroutine(Shaking());
    }

    private IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            
            Vector3 shakeOffset = Random.insideUnitSphere * strength;
            transform.position = player.transform.position + cameraOffset + shakeOffset;
            transform.rotation = player.transform.rotation;
            yield return null;
        }
        shake = false;
        transform.position = startPosition;
    }

    void LateUpdate()
    {

        if (!shake)
        {
            transform.position = player.transform.position + cameraOffset;
            transform.rotation = player.transform.rotation;
        }

    }

}
