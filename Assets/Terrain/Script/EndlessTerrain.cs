using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class EndlessTerrain : MonoBehaviour
{
    [Header("Terrain Parent Settings")]
    public TerrainParent terrainParent;

    [Header("Player Settings")]
    public Transform player; 
    public int renderDistance = 5; 

    private Dictionary<Vector2Int, GameObject> terrainChunks = new Dictionary<Vector2Int, GameObject>();
    public List<Material> materials = new List<Material>();

    private Vector2Int previousPlayerChunkCoord = new Vector2Int(int.MaxValue, int.MaxValue);

    private void Start()
    {
        if (terrainParent == null)
        {
            Debug.LogError("TerrainParent n'est pas assigné dans le script InfiniteTerrainGenerator.");
        }
        if (player == null)
        {
            Debug.LogError("Player n'est pas assigné dans le script InfiniteTerrainGenerator.");
        }
        

    }

    private void Update()
    {
        if (terrainParent == null || player == null) return;

        Vector2Int playerChunkCoord = GetPlayerChunkCoord();

        if (playerChunkCoord != previousPlayerChunkCoord)
        {
            previousPlayerChunkCoord = playerChunkCoord;
            ManageChunksAroundPlayer(playerChunkCoord);
        }
    }

    private Vector2Int GetPlayerChunkCoord()
    {
        return new Vector2Int(
            Mathf.FloorToInt(player.position.x / TerrainParent.ChunkSize),
            Mathf.FloorToInt(player.position.z / TerrainParent.ChunkSize)
        );
    }

    private void ManageChunksAroundPlayer(Vector2Int playerChunkCoord)
    {
        for (int x = -renderDistance; x <= renderDistance; x++)
        {
            for (int z = -renderDistance; z <= renderDistance; z++)
            {
                Vector2Int chunkCoord = new Vector2Int(playerChunkCoord.x + x, playerChunkCoord.y + z);

                if (!terrainChunks.ContainsKey(chunkCoord))
                {
                    CreateChunk(chunkCoord);
                }
                else
                {
                    if (!terrainChunks[chunkCoord].activeSelf)
                    {
                        terrainChunks[chunkCoord].SetActive(true);
                    }
                }
            }
        }

        foreach (var chunk in terrainChunks)
        {
            if (Vector2Int.Distance(chunk.Key, playerChunkCoord) > renderDistance)
            {
                if (chunk.Value.activeSelf)
                {
                    chunk.Value.SetActive(false);
                }
            }
        }
    }

    private void CreateChunk(Vector2Int chunkCoord)
    {
        GameObject newChunk = new GameObject($"Chunk_{chunkCoord.x}_{chunkCoord.y}");
        newChunk.transform.position = new Vector3(chunkCoord.x * TerrainParent.ChunkSize, 0, chunkCoord.y * TerrainParent.ChunkSize);
        newChunk.transform.parent = transform;

        TerrainGenerator terrainGenerator = newChunk.AddComponent<TerrainGenerator>();
        MeshRenderer meshRenderer = newChunk.AddComponent<MeshRenderer>();
        BuildingGrid buildingGrid = newChunk.AddComponent<BuildingGrid>();

        meshRenderer.SetMaterials(materials);
        terrainGenerator.GenerateMesh(terrainParent);

        terrainChunks.Add(chunkCoord, newChunk);

        meshRenderer = terrainGenerator.GetMeshRenderer();
        

    }
}
