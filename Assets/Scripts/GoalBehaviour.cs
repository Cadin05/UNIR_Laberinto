using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalBehaviour : MonoBehaviour
{
    public Transform playerTransform;

    Animator animator;
    bool touchedGoal = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (touchedGoal)
        {
            playerTransform.position = Vector3.Lerp(playerTransform.position, transform.position + Vector3.up * 0.75f, 0.1f);
        }
    }

    public void TouchedByPlayer()
    {
        animator.SetTrigger("Touched");
        playerTransform.gameObject.GetComponent<MovementControls>().enabled = false;
        touchedGoal = true;        
    }

    public void LoadWinScene()
    {
        SceneManager.LoadScene("Win");
    }
}
