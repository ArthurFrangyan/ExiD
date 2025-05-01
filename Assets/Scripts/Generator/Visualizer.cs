using System.Collections.Generic;
using Assets.Scripts.Data;
using Generator.GraphAlgorithm;
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
            PaintBlocksArea(room.Blocks, room.Position);
        }

        public void PaintPaths(HashSet<Vector3Int> pathPositions)
        {
            foreach (var position in pathPositions)
            {
                PaintSingleTile(position * Info.CellSize, environments.Floors[Random.Range(0, environments.Floors.Length - 1)]);
            }
        }

        public void PaintDungeon(Dungeon dungeon)
        {
            int cellSize = Info.CellSize;
            for (int x = 0; x < dungeon.Blocks.GetLength(0); x++)
            for (int y = 0; y < dungeon.Blocks.GetLength(1); y++)
            for (int z = 0; z < dungeon.Blocks.GetLength(2); z++)
            {
                PaintBlock(dungeon.Blocks[x, y, z],
                    new Vector3(x * cellSize, y * cellSize, z * cellSize) + dungeon.Position * cellSize);
            }
        }

        private void PaintBlocksArea(Block[,] cells, Vector3 center)
        {
            int cellSize = Info.CellSize;
            for (int y = 0; y < cells.GetLength(0); y++)
            {
                for (int x = 0; x < cells.GetLength(1); x++)
                {
                    PaintBlock(cells[y,x], 
                        new Vector3(x * cellSize, 0, y * cellSize) + center * cellSize);
                }
            }
        }

        private void PaintBlock(Block block, Vector3 position)
        {
            var cellSize= Info.CellSize;
            if (block.HasTopWall)
            {
                PaintSingleTile(
                    position + new Vector3(0, 0, cellSize/2f),
                    environments.Walls[Random.Range(0, environments.Walls.Length - 1)]);
            }
            if (block.HasBottomWall)
            {
                PaintSingleTile(
                    position + new Vector3(0, 0, -cellSize/2f),
                    environments.Walls[Random.Range(0, environments.Walls.Length - 1)],
                    180);
            }
            if (block.HasRightWall)
            {
                PaintSingleTile(
                    position + new Vector3(cellSize/2f, 0, 0),
                    environments.Walls[Random.Range(0, environments.Walls.Length - 1)],
                    90);
            }
            if (block.HasLeftWall)
            {
                PaintSingleTile(
                    position + new Vector3(-cellSize/2f, 0, 0),
                    environments.Walls[Random.Range(0, environments.Walls.Length - 1)],
                    -90);
            }
            if (block.HasFloor)
            {
                PaintSingleTile(
                    position,
                    environments.Floors[Random.Range(0, environments.Floors.Length - 1)]);
            }
            if (block.HasCeil)
            {
                PaintSingleTile(
                    position + new Vector3(0, cellSize, 0),
                    environments.Ceils[Random.Range(0, environments.Ceils.Length - 1)]);
            }
        }

        private void PaintSingleTile(Vector3 position, GameObject tile, float yAngle = 0)
        {
            var floor = Instantiate(tile, parentObject.transform);
            floor.transform.position = position;
            floor.transform.eulerAngles = new Vector3(0, yAngle, 0);
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
