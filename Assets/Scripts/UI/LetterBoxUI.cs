using UnityEngine;

public class LetterBoxUI : MonoBehaviour
{

    [SerializeField] GameObject[] letterBox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.isInCutScene)
        {
            foreach (var letterBox in letterBox)
            {
                letterBox.SetActive(true);
            }
        } else
        {
              foreach (var letterBox in letterBox)
            {
                letterBox.SetActive(false);
            }
        }
    }
}
