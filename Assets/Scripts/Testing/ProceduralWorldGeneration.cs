using System.Collections.Generic;
using UnityEngine;

public class SpaceProceduralGenerator : MonoBehaviour
{
    [Header("Space Tile Settings")]
    public GameObject spaceTilePrefab;  // Prefab for space tiles
    public float tileWorldSize = 16f;   // Size of each tile in world units, adjust based on sprite PPU
    public int renderDistance = 4;      // How many chunks to generate around the player
    public int spaceLayer = 3;          // Layer for space tiles (e.g., background)

    [Header("Asteroid Settings")]
    public GameObject asteroidPrefab;   // Prefab for asteroids
    public int asteroidsPerChunk = 5;   // Max number of asteroids per chunk
    public int asteroidLayer = 1;       // Layer for asteroids (e.g., spaceships)
    public float noiseScale = 0.1f;     // Scale for Perlin noise (adjust for asteroid distribution)
    public float asteroidDensity = 0.5f; // Threshold for Perlin noise to determine asteroid placement

    private Transform player;
    private Vector2 playerPosition;
    private Dictionary<Vector2, GameObject> chunks = new Dictionary<Vector2, GameObject>();
    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>(); // Track occupied positions

    private void Start()
    {
        player = Camera.main.transform; // Get the player or camera transform
        GenerateInitialChunks();
    }

    private void Update()
    {
        UpdateChunksAroundPlayer();
        RemoveDistantChunks();
    }

    // Generate initial chunks around the player
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

    // Generate a single chunk
    void GenerateChunk(Vector2 chunkCoord)
    {
        if (chunks.ContainsKey(chunkCoord)) return;

        // Create new space tile chunk
        GameObject newChunk = Instantiate(spaceTilePrefab);
        newChunk.transform.position = new Vector3(chunkCoord.x * tileWorldSize, chunkCoord.y * tileWorldSize, 0); // Ensure tiles are placed next to each other
        newChunk.name = $"SpaceTile_{chunkCoord.x}_{chunkCoord.y}";
        newChunk.layer = spaceLayer; // Assign space tile to the specified layer

        // Spawn asteroids within the chunk using Perlin Noise
        for (int i = 0; i < asteroidsPerChunk; i++)
        {
            Vector3 asteroidPosition = GetAsteroidPosition(chunkCoord);

            if (asteroidPosition != Vector3.zero) // If position is valid, spawn asteroid
            {
                SpawnAsteroid(asteroidPosition);
            }
        }

        // Store the chunk
        chunks.Add(chunkCoord, newChunk);
    }

    // Use Perlin Noise to generate asteroid positions and avoid overlapping asteroids
    Vector3 GetAsteroidPosition(Vector2 chunkCoord)
    {
        for (int attempt = 0; attempt < 10; attempt++) // Try multiple positions to avoid overlap
        {
            float xOffset = Random.Range(-tileWorldSize / 2, tileWorldSize / 2);
            float yOffset = Random.Range(-tileWorldSize / 2, tileWorldSize / 2);

            Vector3 potentialPosition = new Vector3(
                chunkCoord.x * tileWorldSize + xOffset,
                chunkCoord.y * tileWorldSize + yOffset,
                0f
            );

            // Use Perlin Noise to determine if an asteroid should be placed
            float noiseValue = Mathf.PerlinNoise(potentialPosition.x * noiseScale, potentialPosition.y * noiseScale);

            if (noiseValue > asteroidDensity && !occupiedPositions.Contains(potentialPosition))
            {
                occupiedPositions.Add(potentialPosition);
                return potentialPosition;
            }
        }

        return Vector3.zero; // Return zero if no valid position is found
    }

    // Spawn an asteroid at a random position within the chunk
    void SpawnAsteroid(Vector3 position)
    {
        GameObject asteroid = Instantiate(asteroidPrefab, position, Quaternion.identity);
        asteroid.layer = asteroidLayer; // Assign asteroid to the specified layer

        // Set up the asteroid's random rotation and movement if needed
        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)); // Random movement direction
        }
    }

    // Update the chunks around the player based on their current position
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

    // Remove chunks that are too far from the player
    void RemoveDistantChunks()
    {
        List<Vector2> chunksToRemove = new List<Vector2>();

        foreach (var chunk in chunks)
        {
            // Calculate the distance from the current chunk to the player
            float distanceToPlayer = Vector2.Distance(playerPosition, chunk.Key);

            // If the chunk is beyond the render distance, mark it for removal
            if (distanceToPlayer > renderDistance + 1) // Slightly more than render distance to avoid flickering
            {
                chunksToRemove.Add(chunk.Key);
            }
        }

        // Remove the marked chunks
        foreach (var chunkCoord in chunksToRemove)
        {
            Destroy(chunks[chunkCoord]);
            chunks.Remove(chunkCoord);
        }
    }
}
