using UnityEngine;

public class Weapon : MonoBehaviour
{
    private WeaponController wc;
    [SerializeField] private Camera cam;
    private EnemyHealthRagdoll enemyHealth;
    void Awake()
    {
        wc = GetComponentInParent<WeaponController>();
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 6 && wc.Active)// its an enemy
        {
            wc.Active = true;
            enemyHealth = col.gameObject.GetComponent<EnemyHealthRagdoll>();
            if (enemyHealth == null) return;
            enemyHealth.TakeDamage(100);
            AudioSource audiosource = wc.GetComponent<AudioSource>();
            audiosource.PlayOneShot(wc.BatSwingHit);
            if (col.gameObject.tag == "Critical")
            {
                enemyHealth.TakeDamage(100);
                enemyHealth.Decapitate();
            }
        }
    }
}
