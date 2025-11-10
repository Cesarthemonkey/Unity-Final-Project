using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private LevelManager[] levels;
    [SerializeField]
    private GameObject gameOverScreen;


    [SerializeField] private AudioClip chime;
    [SerializeField] private AudioClip error;

    private AudioSource audioSource;
    private GameObject crossHair;
    public static GameManager Instance { get; private set; }

    public int score;

    public int streak;
    public int startTime;
    public bool gameActive = false;
    public bool isInCutScene = false;

    public int level = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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

    public void PlayStreakBreak()
    {
        audioSource.PlayOneShot(error);
    }

    public void updateStreak()
    {
        streak++;
        if (streak >= 2)
        {
            audioSource.PlayOneShot(chime);
        }
    }

    public void StartNextLevel()
    {
        levels[level].StopLevel();
        level++;
        if (level < levels.Length)
        {
            levels[level].InitializeLevel();
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        gameActive = false;
        GameInfoController.Instance.gameObject.SetActive(false);
        gameOverScreen.SetActive(true);
        crossHair.SetActive(false);
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
        yield return new WaitForSeconds(1f);
        gameActive = true;
        GameInfoController.Instance.centerCountDown.gameObject.SetActive(false);

        levels[level].InitializeLevel();
    }

}
