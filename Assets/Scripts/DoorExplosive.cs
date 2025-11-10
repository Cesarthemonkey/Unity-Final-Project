using UnityEngine;

public class DoorExplosive : MonoBehaviour
{

    [SerializeField]
    private GameObject door;

    [SerializeField]
    private ParticleSystem particle;
    private bool moveDoor;
    public void BlowUp()
    {
        moveDoor = true;

        door.SetActive(false);
    }

    void Update()
    {
        if(moveDoor)
        {
            
        }
    }
}
