using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    public static BuildingGrid instance;

    public Vector2Int gridSize = new Vector2Int(10, 10); 
    public float cellSize = 1f; 
    public Color gridColor = Color.green; 

    private BuildingData[,] gridCells; 

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        gridCells = new BuildingData[gridSize.x, gridSize.y];

        
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                gridCells[i, j] = null;
            }
        }
        
    }

    public Vector2Int GetGridCoordinates(Vector3 position)
    {
        Vector3 gridOrigin = GetGridOrigin();
        int x = Mathf.FloorToInt((position.x - gridOrigin.x) / cellSize);
        int y = Mathf.FloorToInt((position.z - gridOrigin.z) / cellSize);
        return new Vector2Int(x, y);
    }

    public Vector3 GetWorldPosition(Vector2Int cellCoords)
    {
        Vector3 gridOrigin = GetGridOrigin();
        float x = gridOrigin.x + cellCoords.x * cellSize + cellSize / 2;
        float z = gridOrigin.z + cellCoords.y * cellSize + cellSize / 2;
        return new Vector3(x, 0, z);
    }

    public bool IsCellOccupied(Vector2Int cellCoords)
    {
        Debug.Log("Occupied");
        return gridCells[cellCoords.x, cellCoords.y] != null;
    }

    public void PlaceBuildingInCell(Vector2Int cellCoords, BuildingData buildingData)
    {
        gridCells[cellCoords.x, cellCoords.y] = buildingData;
    }

    private Vector3 GetGridOrigin()
    {
        float totalWidth = gridSize.x * cellSize;
        float totalHeight = gridSize.y * cellSize;
        return transform.position - new Vector3(totalWidth / 2, 0, totalHeight / 2);
    }
    private void OnDrawGizmos()
    {
        Vector3 gridOrigin = GetGridOrigin();
        Gizmos.color = gridColor;

        
        float totalWidth = gridSize.x * cellSize;
        float totalHeight = gridSize.y * cellSize;

        for (int x = 0; x <= gridSize.x; x++)
        {
            Vector3 startPoint = gridOrigin + new Vector3(x * cellSize, 0, 0);
            Vector3 endPoint = startPoint + new Vector3(0, 0, gridSize.y * cellSize);
            Gizmos.DrawLine(startPoint, endPoint);
        }

        for (int y = 0; y <= gridSize.y; y++)
        {
            Vector3 startPoint = gridOrigin + new Vector3(0, 0, y * cellSize);
            Vector3 endPoint = startPoint + new Vector3(gridSize.x * cellSize, 0, 0);
            Gizmos.DrawLine(startPoint, endPoint);
        }
    }
}
