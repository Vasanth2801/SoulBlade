using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Vector2 attackSize = new Vector2(1.1f, 0.8f);
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackCooldown = 0.35f;
    [SerializeField] private Animator animator;

    private float nextAttackTime;

    private void Update()
    {
        if(Keyboard.current == null)
        {
            return;
        }

        if(Keyboard.current.jKey.wasPressedThisFrame && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        if(attackPoint == null)
        {
            Debug.LogWarning("PlayerCombat: Assign Attack Point in the Inspector", this);
            return;
        }

        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPoint.position, attackSize, 0f, enemyLayers);
        foreach(Collider2D hit in hits)
        {
            EnemyHealth enemy = hit.GetComponentInParent<EnemyHealth>();

            if(enemy != null)
            {
                enemy.TakeDamage(attackDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(attackPoint.position, attackSize);
    }
}