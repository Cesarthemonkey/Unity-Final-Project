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

    public int level = 0;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        level = 0;
        score = 0;
        streak = 0;
        StartCoroutine(InitializeGame());
    }

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

    public void StartNextLevel()
    {
        level++;
        levels[level].InitializeLevel();
    }

    IEnumerator InitializeGame()
    {
        GameInfoController.Instance.centerCountDown.gameObject.SetActive(true);
        GameInfoController.Instance.centerCountDown.text = startTime.ToString();

        while (startTime > 0)
        {
            GameInfoController.Instance.centerCountDown.text = startTime.ToString();
            yield return new WaitForSeconds(1f);

            startTime--;
        }
        GameInfoController.Instance.centerCountDown.text = "GO!";
        new WaitForSeconds(1f);
        gameActive = true;
        GameInfoController.Instance.centerCountDown.gameObject.SetActive(false);

        levels[level].InitializeLevel();
    }

}
