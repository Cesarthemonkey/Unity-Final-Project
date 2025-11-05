using UnityEngine;

public class DoorExplosive : MonoBehaviour
{

    [SerializeField]
    private GameObject door;
    
    [SerializeField]
    private ParticleSystem particle;
   public void BlowUp()
    {

        Rigidbody rigidbody = GetComponent<Rigidbody>();
       Instantiate(particle, transform.position, transform.rotation);
        door.SetActive(false);
    }
}
