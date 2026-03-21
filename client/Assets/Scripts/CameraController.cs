using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void Start()
    {
        if (Camera.main.aspect <= 0.5f)
        {
            Camera.main.orthographicSize = Screen.height / 2f;
        }
    }
}
