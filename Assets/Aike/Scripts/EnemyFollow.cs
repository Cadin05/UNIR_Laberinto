using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float initialLife = 3f;
    [SerializeField] Canvas canvasEnemyDetection;
    [SerializeField] private FloatingHealthBar healthBar;
    
    private AudioSource[] audioSources;
    
    private float currentLife;
    private NavMeshAgent agent;
    private SightFollow sight;

    private Vector3 initialPosition;
    
    enum State
    {
        Waiting, // TODO: Cambiar a Patrol
        Following,
        Death
    }

    State currentState;

    private void Awake()
    {
        sight = GetComponent<SightFollow>();
        agent = GetComponent<NavMeshAgent>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        currentLife = initialLife;
        initialPosition = transform.position;
        audioSources = GetComponents<AudioSource>();
        healthBar.UpdateHealthBar(currentLife, initialLife);
    }

    private void Update()
    {
        UpdateSenses();
        UpdateDecisionMaking();
        UpdateState();
    }

    private void UpdateSenses()
    {
        target = sight.GetPlayerInSight();
    }

    private void UpdateDecisionMaking()
    {
        if (currentLife <= 0f)
        {
              SetState(State.Death);
        }
        else if (target != null)
        {
             SetState(State.Following);
        }
        else
        {
            SetState(State.Waiting);
        }
    }

    private void UpdateState()
    {
        switch (currentState)
        {
            case State.Waiting:
                 UpdateWaiting();
                break;
            case State.Following:
                 UpdateFollowing();
                break;
            case State.Death:
                 UpdateDeath();
                break;
        }
    }
    
    #region State Update

    private void SetState(State newState)
    {
        if (newState != currentState)
        {
            switch (currentState)
            {
                case State.Waiting:
                    ExitWaitingState();
                    break;
                case State.Following:
                    ExitFollowingState();
                    break;
                case State.Death:
                    // ExitDeathState();
                    break;
            }

            currentState = newState;

            switch (currentState)
            {
                case State.Waiting:
                    EnterWaitingState();
                    break;
                case State.Following:
                    EnterFollowingState();
                    break;
                case State.Death:
                    EnterDeathState();
                    break;
            }
        }
    }
    
    #region Waiting State
    private void EnterWaitingState()
    { 
        // Nada de momento
    }
    private void UpdateWaiting()
    {
        agent.SetDestination(initialPosition);
    }

    private void ExitWaitingState()
    {
        // que se pare
        agent.SetDestination(transform.position);
    }
    #endregion
    
    #region Following State
    private void EnterFollowingState()
    {
        canvasEnemyDetection.enabled = true;
        
        AudioSource detectedAudio = audioSources[0];
        if (detectedAudio != null)
        {
            detectedAudio.Play();
        }
        
    }
    private void UpdateFollowing()
    {
        agent.SetDestination(target.position);
    }

    private void ExitFollowingState()
    {
        canvasEnemyDetection.enabled = false;
        agent.SetDestination(transform.position);
        AudioSource undetectedAudio = audioSources[1];
        if (undetectedAudio != null)
        {
            undetectedAudio.Play();
        }
    }
    #endregion
    
    #region Death State
    private void EnterDeathState()
    {
        // que se pare
        agent.SetDestination(transform.position);
        
        AudioSource deathAudio = audioSources[3];
        if (deathAudio != null)
        {
            deathAudio.Play();
        }
    }
    private void UpdateDeath()
    {
        Destroy(gameObject, 2f); 
    }

    
    #endregion

    #endregion
    
    public void Hurt()
    {
        currentLife--;
        AudioSource hurtAudio = audioSources[2];
        if (hurtAudio != null)
        {
            hurtAudio.Play();
        }
        
        healthBar.UpdateHealthBar(currentLife, initialLife);
    }
}