using UnityEngine;

public class Switch : MonoBehaviour, IInteractable
{
    Animator animator;
    public GameObject switchActivatedObject;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        animator.SetTrigger("TurnOn");
        switchActivatedObject.GetComponent<ISwitchActivatable>().OnSwitchActivation();
        gameObject.layer = LayerMask.NameToLayer("Default");
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}
