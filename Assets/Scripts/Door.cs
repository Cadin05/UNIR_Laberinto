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
        gameObject.layer = LayerMask.NameToLayer("Default");
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}
