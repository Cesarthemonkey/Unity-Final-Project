using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }

    public void TitleScreen()
    {
        SceneManager.LoadScene(0);
    }
}
