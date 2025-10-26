using TMPro;
using UnityEngine;

public class ScoreUIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreTextUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreTextUI.text = GameManager.Instance.score.ToString();
    }
}
