using UnityEngine;

public class CrossHair : MonoBehaviour
{
   private RectTransform crosshair;

    void Start()
    {
        crosshair = GetComponent<RectTransform>();
        Cursor.visible = false;
    }

    void Update()
    {
        crosshair.position = Input.mousePosition;
    }
}
