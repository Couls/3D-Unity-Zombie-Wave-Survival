using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBrain : MonoBehaviour
{
    private PlayerMovement targetMovement;
    private PlayerHealth targetHealth;
    private Animator anim;
    public float AttackCooldown = 1f;
    public bool canAttack = true;
    public bool Grabbing = false;
    private bool Active;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Grabbing && targetHealth != null)
        {
            targetHealth.TakeDamage(1 * Time.deltaTime); // drain a bit of health as long as they're grabbed 
        }
    }
    public void Attack()
    {
        if (!canAttack || Grabbing) return;
        canAttack = false;
        anim.SetTrigger("Attack");
        Active = true;
    }

    IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(AttackCooldown);
        canAttack = true;
    }

    void OnTriggerStay(Collider col)
    {
        if (!Active || Grabbing) return;
        if (col.gameObject.layer == 3 && col.gameObject.GetComponent<PlayerMovement>() != null)
        {
            Active = false;
            Debug.Log("Collided with da: " + col);
            if (targetHealth == null)
            {
                targetHealth = col.gameObject.GetComponent<PlayerHealth>();
            }
            targetMovement = col.gameObject.GetComponent<PlayerMovement>();
            if (targetMovement.grabbed == true) // someone else grabbed them, we'll just attack
            {
                Debug.Log("Dealing Stratch Damage to Player, At least We should be >:(");
                targetHealth.TakeDamage(5);
                AttackCooldown = 4f; // We made a hit, so let's give the player time to react
            }
            else if (targetMovement.grabbed == false) // they're not grabbed, so we'll grab them
            {
                //Debug.Log("Grabbing" + col);
                anim.SetBool("Grab", true);
                targetMovement.Grabbed(this.gameObject);
                Grabbing = true; // we technically shouldn't have to tell zombiemovement that we're grabbing and can't move since we'll be attached to the player
            }
            StartCoroutine(ResetAttackCooldown());
        }
    }
}
