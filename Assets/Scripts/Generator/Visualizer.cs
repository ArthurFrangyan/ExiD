using System.Collections.Generic;
using Assets.Scripts.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Generator
{
    public class Visualizer : MonoBehaviour
    {
        [SerializeField]
        private EnvironmentScriptableObject environments;
        [SerializeField]
        private GameObject parentObject;

        private readonly List<GameObject> _instanceCells = new List<GameObject>();

        public void PaintRooms3D(RoomGraph3D rooms)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                for (int j = 0; j < rooms[i].Count; j++)
                {
                    for (int k = 0; k < rooms[i][j].Count; k++)
                    {
                        PaintRoom(rooms[i][j][k]);
                    }
                }
            }
        }
        public void PaintRooms2D(IList<List<Room>> rooms)
        {
            foreach (var roomLiene in rooms)
            {
                foreach (var room in roomLiene)
                {
                    PaintRoom(room);
                }
            }
        }
        public void PaintRoom(Room room)
        {
            PaintTiles(room.Blocks, room.Position);
        }
        public void PaintPaths(HashSet<Vector3Int> pathPositions)
        {
            foreach (var position in pathPositions)
            {
                PaintSingleTile(position * Info.CellSize, environments.Floors[Random.Range(0, environments.Floors.Length - 1)]);
            }
        }

        private void PaintTiles(Block[,] cells, Vector3 center)
        {
            int cellSize = Info.CellSize;
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    if (cells[i, j].HasFloor)
                    {
                        PaintSingleTile(
                            new Vector3(j * cellSize, 0, i * cellSize) + center * cellSize,
                            environments.Floors[Random.Range(0, environments.Floors.Length - 1)]);
                    }
                }
            }
        }

        private void PaintSingleTile(Vector3 position, GameObject tile)
        {
            var floor = Instantiate(tile, parentObject.transform);
            floor.transform.position = position;
            _instanceCells.Add(floor);
        }
        public void Clean()
        {
            if (_instanceCells is null)
                return;
            foreach (GameObject instanceCell in _instanceCells)
            {
                Destroy(instanceCell.gameObject);
            }
            _instanceCells.Clear();
        }
    }
}
