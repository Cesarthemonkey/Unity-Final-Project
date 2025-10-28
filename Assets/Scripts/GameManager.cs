using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private LevelManager[] levels;

    public static GameManager Instance { get; private set; }

    public int score;

    public int streak;
    public int startTime;
    public bool gameActive = false;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
        streak = 0;
        StartCoroutine(InitializeGame());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updateScore(int points)
    {
        if (streak > 0)
        {
            score += points * streak;

        }
        else
        {
            score += points;

        }
    }

    public void resetStreak()
    {
        streak = 0;
    }

    public void updateStreak()
    {
        streak++;
    }

    IEnumerator InitializeGame()
    {
        GameInfoController.Instance.centerCountDown.gameObject.SetActive(true);
        GameInfoController.Instance.centerCountDown.text = startTime.ToString();

        while (startTime > 0)
        {
            Debug.Log(startTime);
            GameInfoController.Instance.centerCountDown.text = startTime.ToString();
            yield return new WaitForSeconds(1f);

            startTime--;
        }
        GameInfoController.Instance.centerCountDown.text = "GO!";
        new WaitForSeconds(1f);
        gameActive = true;
        GameInfoController.Instance.centerCountDown.gameObject.SetActive(false);

        levels[0].StartLevel();
    }

}
