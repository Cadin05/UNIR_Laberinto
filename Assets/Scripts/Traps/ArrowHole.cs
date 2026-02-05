using UnityEngine;

public class ArrowHole : MonoBehaviour
{
    float arrowShootInterval;
    [SerializeField] float arrowShootIntervalMax = 5f;
    [SerializeField] float arrowShootStartOffset = 0f;

    public GameObject arrowObject;

    private void Start()
    {
        arrowShootInterval = arrowShootStartOffset;
    }

    private void Update()
    {
        if (arrowShootInterval > 0f)
        {
            arrowShootInterval -= Time.deltaTime;
        }
        else
        {
            Instantiate(arrowObject, transform);
            arrowShootInterval = arrowShootIntervalMax;
        }
    }
}
