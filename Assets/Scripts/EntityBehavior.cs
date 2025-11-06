using System;
using UnityEngine;

public class EntityBehavior : MonoBehaviour
{
    public GameObject entityPrefab; // Prefab of the entity to spawn
    public Transform target;
    public float moveSpeed = 5f;
    public Collider2D boundaryCollider;
    private SpriteSpawner spawner; // Reference to the spawner

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find target by tag
        target = GameObject.FindGameObjectWithTag("Player").transform;
        // Find the spawner in the scene
        spawner = FindFirstObjectByType<SpriteSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if( target != null)
        {
            // Get direction to the target
            Vector3 direction = (target.position - transform.position).normalized;

            // Move towards the target
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Rotate to face the target
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        }
    }

    // Called when this collider hits another collider
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Spawn a new entity
            if (spawner != null)
            {
                spawner.SpawnMultipleSprites(1);
            }

            // Destroy this entity
            Destroy(gameObject);
            Console.WriteLine("Entity collided with Player and spawned a new entity.");
        }
    }
}
