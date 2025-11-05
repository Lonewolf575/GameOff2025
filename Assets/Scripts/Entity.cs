using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Speed at which the entity moves")]
    public float moveSpeed = 5f;

    private Transform target;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f; // Disable gravity for space movement
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        // Calculate direction to target
        Vector2 direction = ((Vector2)target.position - rb.position).normalized;

        // Move towards target
        rb.linearVelocity = direction * moveSpeed;

        // Optional: Rotate entity to face movement direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }
}