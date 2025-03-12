using Assets.Scripts.Data;
using Assets.Scripts.Generator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Visualizer : MonoBehaviour
{
    [SerializeField]
    private EnvironmentScriptableObject environments;
    [SerializeField]
    private GameObject parentObject;

    private List<GameObject> instanceCells = new List<GameObject>();

    public void PaintRooms(RoomGraph rooms)
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            for (int j = 0; j < rooms[i].Count; j++)
            {
                PaintRoom(rooms[i][j]);
            }
        }
    }
    public void PaintRoom(Room room)
    {
        PaintTiles(room.Cells, room.Position);
    }
    public void PaintPaths(HashSet<Vector3Int> pathPositions)
    {
        foreach (var position in pathPositions)
        {
            PaintSingleTile(position * Info.CellSize, environments.Floors[Random.Range(0, environments.Floors.Length - 1)]);
        }
    }

    private void PaintTiles(short[,] cells, Vector3 center)
    {
        int cellSize = Info.CellSize;
        for (int i = 0; i < cells.GetLength(0); i++)
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                switch (cells[i, j])
                {
                    case 1:
                        PaintSingleTile(
                            new Vector3(j*cellSize, 0, i*cellSize) + center * cellSize, 
                            environments.Floors[Random.Range(0, environments.Floors.Length - 1)]);
                        break;
                }
            }
        }
    }

    private void PaintSingleTile(Vector3 position, GameObject tile)
    {
        var floor = Instantiate(tile, parentObject.transform);
        floor.transform.position = position;
        instanceCells.Add(floor);
    }
    public void Clean()
    {
        if (instanceCells is null)
            return;
        foreach (GameObject instanceCell in instanceCells)
        {
            Destroy(instanceCell.gameObject);
        }
        instanceCells.Clear();
    }
}
