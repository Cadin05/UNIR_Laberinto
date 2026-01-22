using UnityEngine;

public class Door : MonoBehaviour, ISwitchActivatable
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnSwitchActivation()
    {
        animator.SetTrigger("Open");
    }
}
