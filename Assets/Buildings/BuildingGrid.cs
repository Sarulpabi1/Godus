using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    public int gridResolution;        
    public float cellSize = 0.186f;            
    public TerrainGenerator terrain;  
    public TerrainParent terrainP;  
    private bool[,] gridOccupied;

    public void InitializeGrid(int resolution, float cellSize, TerrainGenerator terrain)
    {
        resolution = TerrainParent.instance.gridSize;

        this.terrain = terrain;
        this.cellSize = cellSize;
        this.gridResolution = resolution;

        gridOccupied = new bool[gridResolution, gridResolution];
    }
    public bool CanPlaceBuilding(Vector3 position, BuildingData buildingData)
    {
        int gridX = Mathf.FloorToInt(position.x / cellSize);
        int gridZ = Mathf.FloorToInt(position.z / cellSize);

        if (gridX < 0 || gridZ < 0 || gridX >= gridResolution || gridZ >= gridResolution || gridOccupied[gridX, gridZ])
        {
            Debug.Log("Case non valide ou déjà occupée.");
            return false;
        }

        if (!IsAreaFlat(position))
        {
            Debug.Log("Le terrain n'est pas assez plat pour ce bâtiment.");
            return false;
        }

        gridOccupied[gridX, gridZ] = true;
        return true;
    }

    private bool IsAreaFlat(Vector3 position, float threshold = 0.1f)
    {
        int gridX = Mathf.FloorToInt(position.x / cellSize);
        int gridZ = Mathf.FloorToInt(position.z / cellSize);

        float height1 = terrain.GetHeightAtGridPoint(gridX, gridZ);
        float height2 = terrain.GetHeightAtGridPoint(gridX + 1, gridZ);
        float height3 = terrain.GetHeightAtGridPoint(gridX, gridZ + 1);
        float height4 = terrain.GetHeightAtGridPoint(gridX + 1, gridZ + 1);

        return Mathf.Abs(height1 - height2) < threshold &&
               Mathf.Abs(height1 - height3) < threshold &&
               Mathf.Abs(height1 - height4) < threshold;
    }

    private void OnDrawGizmos()
    {
        if (terrain == null || gridResolution <= 0) return;

        Gizmos.color = Color.red;

        for (int i = 0; i < gridResolution; i++)
        {
            for (int j = 0; j < gridResolution; j++)
            {

                Vector3 cellCenter = new Vector3(i * cellSize + cellSize / 2, 0, j * cellSize + cellSize / 2);

                cellCenter.y = terrain.GetHeightAtGridPoint(i, j);

                Gizmos.DrawWireCube(cellCenter, new Vector3(cellSize, 0.1f, cellSize));
            }
        }
    }
}
