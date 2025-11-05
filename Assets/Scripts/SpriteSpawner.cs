using UnityEngine;

public class SpriteSpawner : MonoBehaviour
{
    public GameObject spritePrefab; // Prefab of the sprite to spawn
    public float spawnHeight = 10f;    // Height above camera view
    public float lineWidth = 10f;      // Width of the spawn line

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the main camera's position
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            // Calculate spawn position above camera view
            float camHeight = mainCamera.orthographicSize;
            float yPosition = mainCamera.transform.position.y + camHeight + spawnHeight;

            // Spawn first sprite
            Vector3 spawnPosition = GetRandomPositionOnLine(yPosition, lineWidth);
            Instantiate(spritePrefab, spawnPosition, Quaternion.identity);
        }
    }

    public void SpawnMultipleSprites(int count)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            float camHeight = mainCamera.orthographicSize;
            float yPosition = mainCamera.transform.position.y + camHeight + spawnHeight;

            for (int i = 0; i < count; i++)
            {
                // Random position along horizontal line
                Vector3 spawnPosition = GetRandomPositionOnLine(yPosition, lineWidth);
                Instantiate(spritePrefab, spawnPosition, Quaternion.identity);
            }
        }
    }

    private Vector3 GetRandomPositionOnLine(float yPosition, float lineWidth)
    {
        float xPosition = Random.Range(-lineWidth / 2, lineWidth / 2);
        return new Vector3(xPosition, yPosition, 0);
    }

    private Vector3 GetRandomPositionOnSemicircle(float arcLength, Vector3 center)
    {
        // Calculate radius from arc length (arc length = π * radius for semicircle)
        float radius = arcLength / Mathf.PI;

        // Generate random angle in radians (0 to PI for semicircle)
        float randomAngle = Random.Range(0f, Mathf.PI);

        // Calculate point on semicircle
        float x = Mathf.Cos(randomAngle) * radius;
        float y = Mathf.Sin(randomAngle) * radius;

        // Offset from center point
        return new Vector3(
            center.x + x,
            center.y + y,
            center.z
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
