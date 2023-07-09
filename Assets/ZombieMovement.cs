using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class ZombieMovement : MonoBehaviour
{
    [SerializeField] private Transform playertransform; // we don't care about waypoints, we can sense brains and we want em
    [SerializeField] private float closeEnoughDistance = 1f;
    private EnemyHealthRagdoll health;
    private NavMeshAgent agent;
    private Animator animator;
    private ZombieBrain brain;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        brain = GetComponent<ZombieBrain>();
        health = GetComponent<EnemyHealthRagdoll>();
    }

    private void Start()
    {
        if (agent != null)
        {
            agent.SetDestination(playertransform.position);
        }
    }

    private void Update()
    {
        if (health.isDead == true) return;
        float distanceToWaypoint = Vector3.Distance(agent.transform.position, playertransform.position);
        if (distanceToWaypoint <= closeEnoughDistance)
        {
            agent.isStopped = true;
            Vector3 lookAtPosition = new Vector3(playertransform.position.x,
                                        this.transform.position.y,
                                        playertransform.position.z);
            transform.LookAt(lookAtPosition);
            brain.Attack();
            animator.SetFloat("Forward", 0f);
            return;
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(playertransform.position);
        }

        animator.SetFloat("Forward", agent.speed);
    }
}