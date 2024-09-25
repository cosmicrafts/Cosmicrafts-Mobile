using System.Collections.Generic;
using UnityEngine;

public class SpaceProceduralGenerator : MonoBehaviour
{
    [Header("Space Tile Settings")]
    public GameObject spaceTilePrefab;
    public float tileWorldSize = 16f;
    public int renderDistance = 4;
    public int spaceLayer = 3;

    [Header("Asteroid Settings")]
    public GameObject asteroidPrefab;
    public int asteroidsPerChunk = 5;
    public int asteroidLayer = 1;
    public float noiseScale = 0.1f;
    public float asteroidDensity = 0.5f;

    [Header("AI Enemy Settings")]
    public GameObject enemyPrefab;        // Prefab for enemies
    public int enemiesPerChunk = 2;       // Number of enemies per chunk
    public float enemySpawnDistance = 10; // Distance from player before spawning enemies

    private Transform player;
    private Vector2 playerPosition;
    private Dictionary<Vector2, GameObject> chunks = new Dictionary<Vector2, GameObject>();
    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();

    private void Start()
    {
        player = Camera.main.transform;
        GenerateInitialChunks();
    }

    private void Update()
    {
        UpdateChunksAroundPlayer();
        RemoveDistantChunks();
    }

    void GenerateInitialChunks()
    {
        playerPosition = new Vector2(Mathf.FloorToInt(player.position.x / tileWorldSize), Mathf.FloorToInt(player.position.y / tileWorldSize));

        for (int x = -renderDistance; x <= renderDistance; x++)
        {
            for (int y = -renderDistance; y <= renderDistance; y++)
            {
                GenerateChunk(new Vector2(playerPosition.x + x, playerPosition.y + y));
            }
        }
    }

    void GenerateChunk(Vector2 chunkCoord)
    {
        if (chunks.ContainsKey(chunkCoord)) return;

        GameObject newChunk = Instantiate(spaceTilePrefab);
        newChunk.transform.position = new Vector3(chunkCoord.x * tileWorldSize, chunkCoord.y * tileWorldSize, 0);
        newChunk.layer = spaceLayer;
        chunks.Add(chunkCoord, newChunk);

        for (int i = 0; i < asteroidsPerChunk; i++)
        {
            Vector3 asteroidPosition = GetAsteroidPosition(chunkCoord);
            if (asteroidPosition != Vector3.zero)
            {
                SpawnAsteroid(asteroidPosition);
            }
        }

        // Spawn AI Enemies
        for (int i = 0; i < enemiesPerChunk; i++)
        {
            Vector3 enemyPosition = GetRandomPositionAroundPlayer(chunkCoord);
            if (enemyPosition != Vector3.zero)
            {
                SpawnEnemy(enemyPosition);
            }
        }
    }

    Vector3 GetAsteroidPosition(Vector2 chunkCoord)
    {
        for (int attempt = 0; attempt < 10; attempt++)
        {
            float xOffset = Random.Range(-tileWorldSize / 2, tileWorldSize / 2);
            float yOffset = Random.Range(-tileWorldSize / 2, tileWorldSize / 2);

            Vector3 potentialPosition = new Vector3(
                chunkCoord.x * tileWorldSize + xOffset,
                chunkCoord.y * tileWorldSize + yOffset,
                0f
            );

            float noiseValue = Mathf.PerlinNoise(potentialPosition.x * noiseScale, potentialPosition.y * noiseScale);

            if (noiseValue > asteroidDensity && !occupiedPositions.Contains(potentialPosition))
            {
                occupiedPositions.Add(potentialPosition);
                return potentialPosition;
            }
        }

        return Vector3.zero;
    }

    void SpawnAsteroid(Vector3 position)
    {
        GameObject asteroid = Instantiate(asteroidPrefab, position, Quaternion.identity);
        asteroid.layer = asteroidLayer;

        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
        }
    }

    // Random position for AI enemies around the player
    Vector3 GetRandomPositionAroundPlayer(Vector2 chunkCoord)
    {
        float xOffset = Random.Range(-tileWorldSize / 2, tileWorldSize / 2);
        float yOffset = Random.Range(-tileWorldSize / 2, tileWorldSize / 2);

        Vector3 enemyPosition = new Vector3(
            chunkCoord.x * tileWorldSize + xOffset,
            chunkCoord.y * tileWorldSize + yOffset,
            0f
        );

        if (Vector3.Distance(enemyPosition, player.position) > enemySpawnDistance)
        {
            return enemyPosition;
        }
        
        return Vector3.zero; // Return invalid position if too close
    }

    void SpawnEnemy(Vector3 position)
    {
        GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // Add any random or initial velocity here if needed
        }
    }

    void UpdateChunksAroundPlayer()
    {
        Vector2 newPlayerPos = new Vector2(Mathf.FloorToInt(player.position.x / tileWorldSize), Mathf.FloorToInt(player.position.y / tileWorldSize));

        if (newPlayerPos != playerPosition)
        {
            playerPosition = newPlayerPos;

            for (int x = -renderDistance; x <= renderDistance; x++)
            {
                for (int y = -renderDistance; y <= renderDistance; y++)
                {
                    Vector2 chunkCoord = new Vector2(playerPosition.x + x, playerPosition.y + y);
                    if (!chunks.ContainsKey(chunkCoord))
                    {
                        GenerateChunk(chunkCoord);
                    }
                }
            }
        }
    }

    void RemoveDistantChunks()
    {
        List<Vector2> chunksToRemove = new List<Vector2>();

        foreach (var chunk in chunks)
        {
            float distanceToPlayer = Vector2.Distance(playerPosition, chunk.Key);
            if (distanceToPlayer > renderDistance + 1)
            {
                chunksToRemove.Add(chunk.Key);
            }
        }

        foreach (var chunkCoord in chunksToRemove)
        {
            Destroy(chunks[chunkCoord]);
            chunks.Remove(chunkCoord);
        }
    }
}
