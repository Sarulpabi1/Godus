using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class EndlessTerrain : MonoBehaviour
{
    public const float maxViewDistance = 500;
    public Transform viewer;

    public static Vector2 viewerPos;
    int chunkSize;
    int chunksVisibleInViewDistance;

    Dictionary<Vector2, Chunk> chunkDictionary = new Dictionary<Vector2, Chunk>();
    List<Chunk> chunkVisibleLastUpdate = new List<Chunk>();

    private void Start()
    {
        chunkSize = TerrainParent.ChunkSize - 1;
        chunksVisibleInViewDistance = Mathf.RoundToInt(maxViewDistance / chunkSize);
    }

    private void Update()
    {
        viewerPos = new Vector2(viewer.position.x, viewer.position.z);
        UpdateVisibleChunks();
    }
    private void UpdateVisibleChunks()
    {
        for (int i = 0; i < chunkVisibleLastUpdate.Count; i++)
            chunkVisibleLastUpdate[i].SetVisible(false);
        chunkVisibleLastUpdate.Clear();


        int currentChunkCoordX = Mathf.RoundToInt(viewerPos.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPos.y / chunkSize);

        for (int yOffset = -chunksVisibleInViewDistance; yOffset <= chunksVisibleInViewDistance; yOffset++)
        {
            for (int xOffset = -chunksVisibleInViewDistance; xOffset <= chunksVisibleInViewDistance; xOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                if (chunkDictionary.ContainsKey(viewedChunkCoord))
                {
                    chunkDictionary[viewedChunkCoord].UpdateChunk();
                    if (chunkDictionary[viewedChunkCoord].IsVisible())
                    {
                        chunkVisibleLastUpdate.Add(chunkDictionary[viewedChunkCoord]);
                    }
                }
                else 
                {
                    chunkDictionary.Add(viewedChunkCoord, new Chunk(viewedChunkCoord, chunkSize, transform));
                }
            }
        }
    }

    public class Chunk
    {
        GameObject meshObject;
        Vector2 position;
        Bounds bounds;
        public Chunk (Vector2 coord, int size, Transform parent)
        {
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            Vector3 positionV3 = new Vector3(position.x, 0, position.y);

            meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            meshObject.transform.position = positionV3;
            meshObject.transform.localScale = Vector3.one * size / 10f;
            meshObject.transform.parent = parent;
            SetVisible(false);
        }

        public void UpdateChunk()
        {
            float viewerDistanceFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPos));
            bool visible = viewerDistanceFromNearestEdge <= maxViewDistance;
            SetVisible(visible);
        }

        public void SetVisible(bool visible)
        {
            meshObject.SetActive(visible);
        }

        public bool IsVisible() { return meshObject.activeSelf; }
    }
}
