using UnityEditor.SettingsManagement;
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
    NavMeshAgent agent;
    Vision vision;
    int currenPatrolPoint = 0;
    float currentLife =10f;


    enum State
    {
        Patrol,
        Following,
        Death,
    }
    
    State currentState;


    void Awake()
    {
        agent= GetComponent<NavMeshAgent>();
        vision = GetComponent<Vision>();
        //healthBar = GetComponentInChildren<FloatingHealthBar>();
        currentLife = initialLife;
        audioSources = GetComponents<AudioSource>();
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

       
        
    }

    void UpdateSense()
    {
         target=vision.GetPlayerInSight();
    }
    
    void UpdateDecissionMaking()
    {
        if (currentLife<=0f)
        {
            SetState(State.Death);
        }
        else if (target!=null) 
        {
            SetState(State.Following);          
        }
        else
        {
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
                //EnterDeathState();
                break;           

        }

    }

    //GESTION ESTADO PATRULLA
    void EnterPatrolState()
    {
        //no necesario porque la estrucutra de nuestra FSM no lo necesita, porque recalculamos constantemente.
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
    }
    void UpdateFollowingState()
    {
        agent.SetDestination(target.position);   
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
        //poner animacion de muerte
        //desactivar collider
        AudioSource deathAudio = audioSources[3];
        if (deathAudio != null)
        {
            deathAudio.Play();
        }
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
    public void Hurt()
    {
        currentLife--;
        AudioSource hurtAudio = audioSources[2];
        if (hurtAudio != null)
        {
            hurtAudio.Play();
        }
        
        //healthBar.UpdateHealthBar(currentLife, initialLife);
    }

    
    
    
}
