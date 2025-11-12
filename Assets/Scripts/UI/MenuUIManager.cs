using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIManager : MonoBehaviour
{

    [SerializeField] private GameObject HighScore;
    void Start()
    {
        MainManager.Instance.LoadHighScore();
        HighScore.GetComponent<TMP_Text>().text = MainManager.Instance.HighScore.ToString("N0");
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        MainManager.Instance.SaveHighScore();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
            Application.Quit(); // original code to quit Unity player
#endif
    }
}
