using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform patrolPointsParent;
    [SerializeField] float reachDistance = 2f;

    NavMeshAgent agent;
    Sight sight;
    int currenPatrolPoint = 0;

    enum State
    {
        Patrol,
        Following,
        Death,
    }
    

    void Awake()
    {
        agent= GetComponent<NavMeshAgent>();
        sight = GetComponent<Sight>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        target=sight.GetPlayerInSight();
        if (target != null)
        {
            agent.SetDestination(target.position);    
        }
        else
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
        
    }



}
