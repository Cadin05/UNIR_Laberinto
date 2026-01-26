using UnityEngine;

public class LimitFPS : MonoBehaviour
{
    public int targetFPS = 60;

    private void Start()
    {
        Application.targetFrameRate = targetFPS;
    }
}
