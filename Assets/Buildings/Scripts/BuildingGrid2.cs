using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGrid2 : MonoBehaviour
{
    public static BuildingGrid2 instance;

    public int gridSize = 10;
    public float cellSize = 1f;
    public TerrainParent terrain; 
    public TerrainGenerator terrainGenerator; 
    private bool[,] gridData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            gridData = new bool[gridSize, gridSize];
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Vector2Int GetGridCoordinates(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / cellSize);
        int z = Mathf.FloorToInt(position.z / cellSize);
        return new Vector2Int(x, z);
    }

    public Vector3 GetWorldPosition(Vector2Int gridCoords)
    {
        float x = gridCoords.x * cellSize;
        float z = gridCoords.y * cellSize;

        float y = GetHeightAtPosition(new Vector3(x, 0, z));

        return new Vector3(x, y, z);
    }

    public bool IsCellOccupied(Vector2Int gridCoords)
    {
        if (gridCoords.x < 0 || gridCoords.x >= gridSize || gridCoords.y < 0 || gridCoords.y >= gridSize)
            return true;

        return gridData[gridCoords.x, gridCoords.y];
    }

    public void PlaceBuildingInCell(Vector2Int gridCoords, BuildingData buildingData)
    {
        if (gridCoords.x >= 0 && gridCoords.x < gridSize && gridCoords.y >= 0 && gridCoords.y < gridSize)
        {
            gridData[gridCoords.x, gridCoords.y] = buildingData;
        }
    }

    private float GetHeightAtPosition(Vector3 position)
    {
        Mesh mesh = terrainGenerator.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        Vector3 localPos = terrain.transform.InverseTransformPoint(position);

        float closestHeight = float.MinValue;
        float minDistance = float.MaxValue;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 v0 = vertices[triangles[i]];
            Vector3 v1 = vertices[triangles[i + 1]];
            Vector3 v2 = vertices[triangles[i + 2]];

            if (IsPointInTriangle(localPos, v0, v1, v2))
            {
                closestHeight = InterpolateHeight(localPos, v0, v1, v2);
                break;
            }
            else
            {
                float distance = Vector3.Distance(localPos, (v0 + v1 + v2) / 3);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestHeight = InterpolateHeight(localPos, v0, v1, v2);
                }
            }
        }

        return terrain.transform.TransformPoint(new Vector3(0, closestHeight, 0)).y;
    }

    private bool IsPointInTriangle(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 v0 = b - a;
        Vector3 v1 = c - a;
        Vector3 v2 = p - a;

        float dot00 = Vector3.Dot(v0, v0);
        float dot01 = Vector3.Dot(v0, v1);
        float dot02 = Vector3.Dot(v0, v2);
        float dot11 = Vector3.Dot(v1, v1);
        float dot12 = Vector3.Dot(v1, v2);

        float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
        float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
        float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

        return (u >= 0) && (v >= 0) && (u + v < 1);
    }

    private float InterpolateHeight(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
    {
        float areaTotal = Vector3.Cross(b - a, c - a).magnitude;
        float areaPBC = Vector3.Cross(b - p, c - p).magnitude;
        float areaPCA = Vector3.Cross(c - p, a - p).magnitude;
        float areaPAB = Vector3.Cross(a - p, b - p).magnitude;

        return (areaPBC * a.y + areaPCA * b.y + areaPAB * c.y) / areaTotal;
    }

   private void OnDrawGizmos()
   {
       if (gridData == null || terrain == null) return;
   
       for (int x = 0; x < gridSize; x++)
       {
           for (int z = 0; z < gridSize; z++)
           {
               Vector2Int cellCoords = new Vector2Int(x, z);
   
               Vector3 cellWorldPosition = GetWorldPosition(cellCoords);

               Gizmos.color = IsCellOccupied(cellCoords) ? Color.red : Color.green;
   
               Gizmos.DrawWireCube(
                   cellWorldPosition + new Vector3(cellSize / 2, 0, cellSize / 2),
                   new Vector3(cellSize, 0.1f, cellSize)
               );
           }
       }
   }
}
