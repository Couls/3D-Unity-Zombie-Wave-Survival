using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] public GameObject Bat;
    private PlayerMovement staminacontroller;
    public bool canAttack = true;
    public float attackCooldown = 1.0f;
    public bool Active = false;
    [SerializeField] private AudioClip BatSwingNoise;
    [SerializeField] public AudioClip BatSwingHit;
    // Start is called before the first frame update
    void Start()
    {
        staminacontroller = GetComponentInParent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canAttack && staminacontroller.stamina >= 5 && !staminacontroller.grabbed)
        {
            BatSwing();
        }
    }
    void BatSwing()
    {
        Active = true;
        canAttack = false;
        Animator animator = Bat.GetComponent<Animator>();
        animator.SetTrigger("Attack");
        AudioSource audiosource = GetComponent<AudioSource>();
        audiosource.PlayOneShot(BatSwingNoise);
        StartCoroutine(ResetAttackCooldown());
        staminacontroller.stamina -= 5;

    }
    IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        Active = false;
    }
}
