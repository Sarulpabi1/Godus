using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteAlways]
public class TerrainParent : MonoBehaviour
{
    [Header("   Mesh Options")]
    public Mesh mesh;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    [Range(1, 256)] public int gridSize;
    [Range(0f, 32f)] public float meshSize;
    public float heightMultiplier;
    public const int ChunkSize = 32;


    [Header("   Perlin Options")]
    [Range(0, 1)] public float perlinScale;
    [Range(0, 1)] public float lacunarity;
    [Range(0, 1)] public float persistence;
    [Range(0, 1)] public float terraceStep;
    [Range(1, 8)] public int octaveCount;

    [Header("   Terrain Options")]
    public bool islandify;
    public bool water;
    public float waterLevel = 0;
    public int seed = 0;

    public int gridResolution;
    public static TerrainParent instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out BuildingGrid buildingGrid))
            {
                if (buildingGrid != null && TerrainGenerator.instance != null)
                {
                    int gridResolution = buildingGrid.gridResolution;
                    float cellSize = buildingGrid.cellSize;
                    buildingGrid.InitializeGrid(gridResolution, cellSize, TerrainGenerator.instance);
                }
            }
        }
    }

    void Update()
    {

        foreach (Transform child in transform)
        {
            if (Application.isPlaying)
                return;

            if (child.TryGetComponent(out TerrainGenerator chunk))
            {
                chunk.GenerateMesh(this);
            }
        }
    }

}