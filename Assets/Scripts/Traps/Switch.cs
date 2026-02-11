using Unity.VisualScripting;
using UnityEngine;

public class Switch : MonoBehaviour, IInteractable
{
    Animator animator;
    public GameObject switchActivatedObject;
    public Score score;

    private void Awake()
    {
        score = FindAnyObjectByType(typeof(Score)).GetComponent<Score>();
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        animator.SetTrigger("TurnOn");
        switchActivatedObject.GetComponent<IRemotelyActivatable>().OnRemoteActivation();
        gameObject.layer = LayerMask.NameToLayer("Default");
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = LayerMask.NameToLayer("Default");
        }

        score.UpdateScore(10);
    }
}
