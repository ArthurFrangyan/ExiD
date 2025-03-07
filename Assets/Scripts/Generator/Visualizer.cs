using Assets.Scripts.Data;
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

    private List<GameObject> floors = new List<GameObject>();
    public void PaintFloorTiles(Room room)
    {
        PaintTiles(room.Cells, floorTile);
    }

    private void PaintTiles(short[,] cells, EnvironmentScriptableObject tile)
    {
        for (int i = 0; i < cells.GetLength(0); i++)
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                PaintSingleTile(cells[i,j], new Vector3(j,0,i), tile);
            }
        }
    }

    private void PaintSingleTile(short cell ,Vector3 position, EnvironmentScriptableObject tile)
    {
        switch (cell)
        {
            case 1:
                var floor = Instantiate(tile.Floors[0]);
                floor.transform.position = Info.CellSize * position;
                floors.Add(floor);
                break;
        }
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
