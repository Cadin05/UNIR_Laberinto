using UnityEngine;

public class Door : MonoBehaviour, IRemotelyActivatable
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnRemoteActivation()
    {
        animator.SetTrigger("Open");
        gameObject.layer = LayerMask.NameToLayer("Default");
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}
