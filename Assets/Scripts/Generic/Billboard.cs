using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        transform.rotation = cam.transform.rotation;
    }
}
