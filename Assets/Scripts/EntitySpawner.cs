using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EntitySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("Start angle of the spawn arc in degrees")]
    [Range(0, 360)] public float startAngle = 150f;
    [Tooltip("End angle of the spawn arc in degrees")]
    [Range(0, 360)] public float endAngle = 210f;
    [Tooltip("Distance from target where entities spawn")]
    public float spawnRadius = 10f;
    [Tooltip("Time between spawns in seconds")]
    public float spawnInterval = 2f;

    [Header("References")]
    [Tooltip("Target that entities will move towards")]
    public Transform target;
    [Tooltip("List of entity prefabs that can be spawned")]
    public List<GameObject> entityPrefabs;

    private void Start()
    {
        // Validate target exists
        if (target == null)
        {
            Debug.LogError("No target assigned to EntitySpawner!");
            return;
        }

        // Start spawning coroutine
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnEntity();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEntity()
    {
        if (entityPrefabs == null || entityPrefabs.Count == 0)
        {
            Debug.LogWarning("No entity prefabs assigned!");
            return;
        }

        // Generate random angle within arc
        float randomAngle = Random.Range(startAngle, endAngle);

        // Convert angle to radians
        float angleRad = randomAngle * Mathf.Deg2Rad;

        // Calculate spawn position
        Vector3 spawnPos = target.position + new Vector3(
            Mathf.Cos(angleRad) * spawnRadius,
            Mathf.Sin(angleRad) * spawnRadius,
            0
        );

        // Randomly select and spawn entity
        GameObject entityPrefab = entityPrefabs[Random.Range(0, entityPrefabs.Count)];
        GameObject spawnedEntity = Instantiate(entityPrefab, spawnPos, Quaternion.identity);

        // Configure entity to move towards target
        Entity entity = spawnedEntity.GetComponent<Entity>();
        if (entity != null)
        {
            entity.SetTarget(target);
        }
    }

    // Optional: Visualize the spawn arc in the editor
    private void OnDrawGizmosSelected()
    {
        if (target == null) return;

        Gizmos.color = Color.yellow;
        Vector3 startDir = Quaternion.Euler(0, 0, startAngle) * Vector3.right;
        Vector3 endDir = Quaternion.Euler(0, 0, endAngle) * Vector3.right;

        Vector3 startPos = target.position + startDir * spawnRadius;
        Vector3 endPos = target.position + endDir * spawnRadius;

        Gizmos.DrawLine(target.position, startPos);
        Gizmos.DrawLine(target.position, endPos);

        int segments = 20;
        float angleStep = (endAngle - startAngle) / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle1 = startAngle + angleStep * i;
            float angle2 = startAngle + angleStep * (i + 1);

            Vector3 pos1 = target.position + Quaternion.Euler(0, 0, angle1) * Vector3.right * spawnRadius;
            Vector3 pos2 = target.position + Quaternion.Euler(0, 0, angle2) * Vector3.right * spawnRadius;

            Gizmos.DrawLine(pos1, pos2);
        }
    }
}