using TMPro;
using UnityEngine;

public class GameInfoController : MonoBehaviour
{
    [SerializeField]
    public TMP_Text countDownText;

    [SerializeField]
    public TMP_Text levelText;

    [SerializeField]
    public TMP_Text centerCountDown;
    public static GameInfoController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
