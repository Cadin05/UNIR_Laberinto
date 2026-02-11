using System.Threading;
using UnityEngine;

public class ReceiveDamage : MonoBehaviour
{
    [SerializeField] int initialLife = 3;
    [SerializeField] float secondsBetweenDamage = 3;

    private int actualLife;
    private MovementControls moveControls;

    void Awake()
    {
        actualLife = initialLife;
        moveControls = GetComponent<MovementControls>();
    }

    float lastTimeTrigger = 0;

    void OnTriggerEnter(Collider other)
    {
        if (Time.time - lastTimeTrigger > secondsBetweenDamage)
        if (other.CompareTag("Enemy"))
        {
            lastTimeTrigger = Time.time;
            initialLife--;
        }
    }
}
