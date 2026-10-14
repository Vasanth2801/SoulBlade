using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int contactDamage = 1;
    [SerializeField] private float contactCooldown = 1f;

    private Transform target;
    private float nextContactDamageTime;

    private void Start()
    {
        target = pointB != null ? pointB : pointA;
    }

    private void Update()
    {
        if (pointA == null || pointB == null)
        {
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        if(Vector2.Distance(transform.position,target.position) < 0.05f)
        {
            target = target == pointA ? pointB : pointA;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryDamagePlayer(collision.collider);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryDamagePlayer(other);
    }

    private void TryDamagePlayer(Collider2D other)
    {
        if(Time.time < nextContactDamageTime)
        {
            return;
        }

        PlayerHealth player = other.GetComponentInParent<PlayerHealth>();
        if (player == null)
        {
            return;
        }

        player.TakeDamage(contactDamage);
        nextContactDamageTime = Time.time + contactCooldown;
    }
}