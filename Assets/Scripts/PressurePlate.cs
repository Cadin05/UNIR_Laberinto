using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    Animator animator;
    public GameObject plateActivatedObject;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animator.SetBool("Pressed", true);
            plateActivatedObject.GetComponent<IRemotelyActivatable>().OnRemoteActivation();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animator.SetBool("Pressed", false);
        }
    }
}
