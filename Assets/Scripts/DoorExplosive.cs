using UnityEngine;

public class DoorExplosive : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField]
    private GameObject door;


    [SerializeField]
    private AudioClip doorOpenSound;


    private float speed = 2f;
    [SerializeField]
    private ParticleSystem particle;
    private bool moveDoor;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void BlowUp()
    {
        moveDoor = true;
        audioSource.PlayOneShot(doorOpenSound);
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        if (moveDoor)
        {
            door.transform.position = Vector3.MoveTowards(
             door.transform.position,
             new Vector3(door.transform.position.x - 2f, door.transform.position.y, door.transform.position.z),
             speed * Time.deltaTime
         );
        }
    }
}
