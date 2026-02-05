using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    [SerializeField] Transform attackSource;
    [SerializeField] float attackRange = 2f;
    [SerializeField] float attackCD = 1f;
    [SerializeField] float lastAttackTime;
    [SerializeField] int strength = 1;

    Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Attack()
    {
        if (Time.time - lastAttackTime >= attackCD)
        {
            lastAttackTime = Time.time;

            animator.SetTrigger("Attack");

            if (Physics.Raycast(attackSource.position, attackSource.forward, out RaycastHit hit, attackRange))
            {
                IDamageable damageable = hit.collider.GetComponent<IDamageable>();

                if (damageable != null)
                {
                    damageable.TakeDamage(strength);
                }
            }
        }
    }
}
