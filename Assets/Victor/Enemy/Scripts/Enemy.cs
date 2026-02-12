using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform patrolPointsParent;
    [SerializeField] float reachDistance = 2f;
    [SerializeField] float initialLife = 3f;
    [SerializeField] Canvas canvasEnemyDetection;
    [SerializeField] private FloatingHealthBar healthBar;
    

    private AudioSource[] audioSources;
    private Animator animator;
    private Health health;
    NavMeshAgent agent;
    Vision vision;
    int currenPatrolPoint = 0;
    float currentLife =10f;

    public Score score;

    [SerializeField] float waitingTime = 3f;
    private bool hasMadeDamage = false;
    enum State
    {
        Patrol,
        Following,
        Death,
        Waiting,
    }
    
    State currentState;


    void Awake()
    {
        agent= GetComponent<NavMeshAgent>();
        vision = GetComponent<Vision>();
        //healthBar = GetComponentInChildren<FloatingHealthBar>();
        health = GetComponent<Health>();
        animator = GetComponentInChildren<Animator>();
        currentLife = initialLife;
        audioSources = GetComponents<AudioSource>();
        score = FindAnyObjectByType(typeof(Score)).GetComponent<Score>();
        //healthBar.UpdateHealthBar(currentLife, initialLife);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        UpdateSense();

        UpdateDecissionMaking();

        UpdateState();

       if (health.hit == true)
        {
            health.hit = false;
            Hurt();
        }
        
    }

    void UpdateSense()
    {
         target=vision.GetPlayerInSight();
    }
    
    void UpdateDecissionMaking()
    {
        
        if (health.health <= 0f)
        {
            Debug.Log("Sin vida");
            SetState(State.Death);
        }
        else if (hasMadeDamage)
        {
            Debug.Log("Dañado");
            SetState(State.Waiting);
        }
        else if (target!=null)
        {
            Debug.Log("Siguiendo");
            SetState(State.Following);          
        }       
        else
        {
            Debug.Log("Patrullando");
            SetState(State.Patrol);            
        }
    }
    void UpdateState()
    {
        switch (currentState)
        {
            case State.Patrol:
                UpdatePatrol();
                break;
            case State.Following:
                UpdateFollowingState();
                break;
            case State.Death:
                UpdateDeath();
                break;
            case State.Waiting:
                UpdateWaiting();
                break;
        }
    }

    void SetState(State newstate)
    {
        //revisamos si el nuevo estado es igual al actual y asi no tener que hacer nada
        //si es diferente pues si que empezamos a revisar y actualizar el estado
        if (currentState == newstate) 
        {
            return;
        }
        switch(currentState)
        {
            case State.Patrol:
                ExitPatrolState();
                break;
            case State.Following:
                ExitFollowingState();
                break;
            case State.Death:
                ExitDeathState();
                break;
            case State.Waiting:
                ExitWaitingState();
                break;

        }
        currentState=newstate;
        switch(currentState)
        {
            case State.Patrol:
                EnterPatrolState();
                break;
            case State.Following:
                EnterFollowingState();
                break;
            case State.Death:
                EnterDeathState();
                break;
            case State.Waiting:
                EnterWaitingState();
                break;

        }

    }

    //GESTION ESTADO PATRULLA
    void EnterPatrolState()
    {
        //no necesario porque la estrucutra de nuestra FSM no lo necesita, porque recalculamos constantemente.
        animator.SetBool("Walking", true);
    }
    void UpdatePatrol()
    {
        Vector3 nextPosition = patrolPointsParent.GetChild(currenPatrolPoint).position;
            agent.SetDestination(nextPosition);
            if (Vector3.Distance(nextPosition,transform.position) < reachDistance)
            {
                currenPatrolPoint++;
                if (currenPatrolPoint >= patrolPointsParent.childCount)
                {
                    currenPatrolPoint =0;
                }
            }

        animator.SetBool("Walking", true);
    }

    void ExitPatrolState()
    {
        agent.SetDestination(transform.position);
    }

    //GESTION ESTADO SEGUIMIENTO
    void EnterFollowingState()
    {
        canvasEnemyDetection.enabled = true;
        
        AudioSource detectedAudio = audioSources[0];
        if (detectedAudio != null)
        {
            detectedAudio.Play();
        }

        animator.SetBool("Walking", true);
    }
    void UpdateFollowingState()
    {
        agent.SetDestination(target.position);


        animator.SetBool("Walking", true);
    }
    void ExitFollowingState()
    {
        canvasEnemyDetection.enabled = false;
        agent.SetDestination(transform.position);
        AudioSource undetectedAudio = audioSources[1];
        if (undetectedAudio != null)
        {
            undetectedAudio.Play();
        }

    }
    //GESTION ESTADO MUERTE
    void EnterDeathState()
    {
        animator.SetBool("Dead", true);
        //desactivar collider
        AudioSource deathAudio = audioSources[3];
        if (deathAudio != null)
        {
            deathAudio.Play();
        }

        Debug.Log("Muerto");
        score.UpdateScore(100);
    }
    void UpdateDeath()
    {
        Destroy(gameObject, 2f); 
    }
    
    void ExitDeathState()
    {
        // no aplica
        //creamos el método para tenerlo por si hicera falta.
    }

    //GESTION ESTADO ESPERANDO
    float lastTimeDamage = 0;

    void EnterWaitingState()
    {
        animator.SetBool("Walking", false);
        agent.SetDestination(transform.position);
    }

    void UpdateWaiting()
    {
        if (Time.time - lastTimeDamage > waitingTime)
        {
            hasMadeDamage = false;
        }

    }
    void ExitWaitingState()
    {
        // no aplica
        //creamos el método para tenerlo por si hicera falta.
    }

    public void Hurt()
    {
        AudioSource hurtAudio = audioSources[2];
        if (hurtAudio != null)
        {
            hurtAudio.Play();
        }
        animator.SetTrigger("Hurt");
        //healthBar.UpdateHealthBar(currentLife, initialLife);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lastTimeDamage = Time.time;
            hasMadeDamage = true;

            Debug.Log("Player Damage");
        }
    }
}
