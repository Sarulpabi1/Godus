using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{

    TerrainParent terrain;
    public void GenerateMesh(TerrainParent terrain)
    {
        this.terrain = terrain;
        Vector3[,] heightMap = new Vector3[terrain.gridSize, terrain.gridSize];

        List<Vector3> vertices = new List<Vector3>();

        for (int i = 0; i < terrain.gridSize; i++)
        {
            for (int j = 0; j < terrain.gridSize; j++)
            {
                float step = terrain.meshSize / (terrain.gridSize - 1);

                Vector3 vertex = new Vector3(i * step, GetHeight(i, j), j * step);
                heightMap[i, j] = vertex;
                vertices.Add(vertex);
            }
        }

        List<int> triangles = new List<int>();
        

        for (int i = 0; i < terrain.gridSize - 1; i++)
        {
            for (int j = 0; j < terrain.gridSize - 1; j++)
            {
                int cornerVertex = i + j * terrain.gridSize;

                triangles.Add(cornerVertex);
                triangles.Add(cornerVertex + 1);
                triangles.Add(cornerVertex + 1 + terrain.gridSize);

                triangles.Add(cornerVertex);
                triangles.Add(cornerVertex + 1 + terrain.gridSize);
                triangles.Add(cornerVertex + terrain.gridSize);
            }
        }

        List<int> waterTris = new List<int>();

        if (terrain.water)
        {
            vertices.Add(new Vector3(0, terrain.waterLevel, 0));
            vertices.Add(new Vector3(terrain.meshSize, terrain.waterLevel, 0));
            vertices.Add(new Vector3(0, terrain.waterLevel, terrain.meshSize));
            vertices.Add(new Vector3(terrain.meshSize, terrain.waterLevel, terrain.meshSize));

            waterTris.Add(vertices.Count - 2);
            waterTris.Add(vertices.Count - 3);
            waterTris.Add(vertices.Count - 4);

            waterTris.Add(vertices.Count - 3);
            waterTris.Add(vertices.Count - 2);
            waterTris.Add(vertices.Count - 1);
        }

        Mesh mesh = new Mesh();

        mesh.subMeshCount = 2;

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);

        if (terrain.water)
            mesh.SetTriangles(waterTris, 1);
        
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;

    }

    private float GetHeight(int x, int z)
    {
        float height = 0;

        float amplitude = 1;
        float frequence = terrain.perlinScale;

        float step = terrain.meshSize / (terrain.gridSize - 1);
        Vector3 worldPos = new Vector3(x * step + transform.position.x, 0, z * step + transform.position.z);

        for (int i = 0; i < terrain.octaveCount; i++)
        {
            height += Mathf.PerlinNoise(worldPos.x * frequence + terrain.seed, worldPos.z * frequence + terrain.seed) * amplitude;

            frequence /= terrain.lacunarity;
            amplitude *= terrain.persistence;
        }

        if (!terrain.islandify)
            return height * terrain.heightMultiplier;

        Vector2 center = new Vector2(terrain.meshSize / 2, terrain.meshSize / 2);

       
        Vector2 vertexPos = new Vector2(x * step, z * step);

        float distance = Vector2.Distance(center, vertexPos);
        float normalizedistance = Mathf.Clamp01(distance / (terrain.meshSize / 2));

        return height * (1 - normalizedistance) * terrain.heightMultiplier;

    }
    
}
