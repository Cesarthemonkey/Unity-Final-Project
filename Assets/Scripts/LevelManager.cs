using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public int duration;
    public string levelName;

    private int countDownTime;

    void Start()
    {
    }

    void Update()
    {

    }

    public void StartLevel()
    {
        countDownTime = duration;
        GameInfoController.Instance.levelText.text = levelName;
        StartCoroutine(CountdownToStart());
    }
    
    IEnumerator CountdownToStart()
    {
        while (countDownTime > 0)
        {
            GameInfoController.Instance.countDownText.text = countDownTime.ToString();
            yield return new WaitForSeconds(1f);

            countDownTime--;
        }

        GameInfoController.Instance.countDownText.text = "Wait";
    }

}
