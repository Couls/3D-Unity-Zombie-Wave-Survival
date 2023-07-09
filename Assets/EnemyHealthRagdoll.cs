using UnityEngine;
using UnityEngine.AI;

public class EnemyHealthRagdoll : MonoBehaviour
{
    [SerializeField] private int hp = 100;
    [SerializeField] public bool isDead = false;
    [SerializeField] Ragdoller ragdoll = null;

    private Animator animator = null;
    private NavMeshAgent agent = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(int damageAmount)
    {
        hp -= damageAmount;

        if (hp <= 0)
        {
            hp = 0;

            isDead = true;
            GameObject wm = GameObject.Find("GameManager");
            wm.GetComponent<WaveManager>().ZombieDeath();
            if (agent != null)
            {
                agent.enabled = false;
            }

            if (ragdoll != null)
            {
                ragdoll.Ragdoll(transform);
                GameObject PlayerCamera = GameObject.Find("Main Camera");
                Camera cam = PlayerCamera.GetComponent<Camera>();
                ragdoll.ApplyForce(cam.ScreenPointToRay(Input.mousePosition).direction, 10000f);
            }
        }
    }
    public void Decapitate()
    {
        GameObject Head = GameObject.Find("Head");
        Head.SetActive(false);
    }
}