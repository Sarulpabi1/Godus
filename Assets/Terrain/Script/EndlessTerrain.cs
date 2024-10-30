using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class EndlessTerrain : MonoBehaviour
{
    [Header("Terrain Parent Settings")]
    public TerrainParent terrainParent;

    public GameObject chunkPrefab;

    public Transform player;
    public int renderDistance = 5;

    private Dictionary<Vector2Int, GameObject> generatedChunks = new Dictionary<Vector2Int, GameObject>();


    private void Update()
    {
        if (Application.isPlaying)
        {
            GenerateChunksAroundPlayer();
        }
    }
    private void GenerateChunksAroundPlayer()
    {
        Vector2Int playerChunkCoord = GetChunkCoordFromPosition(player.position);

        for (int x = -renderDistance; x <= renderDistance; x++)
        {
            for (int z = -renderDistance; z <= renderDistance; z++)
            {
                Vector2Int chunkCoord = new Vector2Int(playerChunkCoord.x + x, playerChunkCoord.y + z);

                if (!generatedChunks.ContainsKey(chunkCoord))
                {
                    GenerateChunk(chunkCoord);
                }
                else
                {
                    generatedChunks[chunkCoord].SetActive(true);
                }
            }
        }

        List<Vector2Int> chunksToDeactivate = new List<Vector2Int>();
        foreach (var chunk in generatedChunks)
        {
            if (Mathf.Abs(chunk.Key.x - playerChunkCoord.x) > renderDistance ||
                Mathf.Abs(chunk.Key.y - playerChunkCoord.y) > renderDistance)
            {
                chunksToDeactivate.Add(chunk.Key);
            }
        }

        foreach (var chunkCoord in chunksToDeactivate)
        {
            generatedChunks[chunkCoord].SetActive(false);
        }
    }

    private Vector2Int GetChunkCoordFromPosition(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / terrainParent.meshSize);
        int z = Mathf.FloorToInt(position.z / terrainParent.meshSize);
        return new Vector2Int(x, z);
    }

    private void GenerateChunk(Vector2Int chunkCoord)
    {
        GameObject chunkObject = Instantiate(chunkPrefab, new Vector3(chunkCoord.x * terrainParent.meshSize, 0, chunkCoord.y * terrainParent.meshSize), Quaternion.identity);
        chunkObject.transform.parent = transform;

        TerrainGenerator chunk = chunkObject.GetComponent<TerrainGenerator>();
        chunk.GenerateMesh(terrainParent);

        generatedChunks.Add(chunkCoord, chunkObject);
    }
}
