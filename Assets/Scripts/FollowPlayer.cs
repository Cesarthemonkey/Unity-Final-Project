using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    [SerializeField]
    private GameObject player;

    private Vector3 cameraOffset = new Vector3(0, 1, 0);
    void LateUpdate()
    {
        transform.position = player.transform.position + cameraOffset;
        transform.rotation = player.transform.rotation;
    }
}
