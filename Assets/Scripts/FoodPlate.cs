using UnityEngine;

public class FoodPlate : MonoBehaviour
{
    [SerializeField] private GameObject[] food;
    void Start()
    {
        food[Random.Range(0, food.Length)].SetActive(true); 
    }

}
