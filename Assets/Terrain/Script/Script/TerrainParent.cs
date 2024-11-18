using System;
using System.Collections;
using System.Collections.Generic;
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

    void Update()
    {
        foreach (Transform child in transform)
        {
            if (Application.isPlaying)
                return;

            if (child.TryGetComponent(out TerrainGenerator chunk))
            {
                chunk.GenerateTerrain(this);
            }
        }
    }

}