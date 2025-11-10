using UnityEngine;

public class BadTarget : Target
{
    [SerializeField]
    private GameObject image;

    public override void HideGameObjects()
    {
        base.HideGameObjects();

        if (image)
        {
            image.SetActive(false);
        }
    }
    
    public override void UpdateStreak()
    {
        GameManager.Instance.PlayStreakBreak();
        CameraManager.Instance.ShakeCamera();
        GameManager.Instance.resetStreak();
    }
}
