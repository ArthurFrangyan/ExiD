using Assets.Scripts.Generator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Visualizer : MonoBehaviour
{
    [SerializeField]
    private EnvironmentScriptableObject floorTile;
    [SerializeField]
    private GameObject parentObject;

    private List<GameObject> floors;
    public void PaintFloorTiles(Room room)
    {
        PaintTiles(room.Cells, floorTile);
    }

    private void PaintTiles(HashSet<Vector2Int> positions, EnvironmentScriptableObject tile)
    {
        foreach (Vector2Int position in positions)
        {
            floors = new List<GameObject>();
            PaintSingleTile(position, tile);
        }
    }

    private void PaintSingleTile(Vector2 position, EnvironmentScriptableObject tile)
    {
        var floor = Instantiate(tile.Floors[0]);
        floor.transform.position = 4* GetVector3(position);
        floors.Add(floor);
    }
    private Vector3 GetVector3(Vector2 position)
    {
        return new Vector3(position.x, 0, position.y);
    }
    public void Clean()
    {
        if (floors is null)
            return;
        foreach (GameObject floor in floors)
        {
            Destroy(floor.gameObject);
        }
        floors.Clear();
    }
}
