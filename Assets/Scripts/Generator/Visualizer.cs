using System.Collections.Generic;
using Assets.Scripts.Data;
using Generator.GraphAlgorithm;
using Generator.GraphAlgorithm.RoomGraph3D;
using Generator.PathFinders;
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

        public void PaintDungeon(Dungeon dungeon)
        {
            for (int x = 0; x < dungeon.Size.x; x++)
            for (int y = 0; y < dungeon.Size.y; y++)
            for (int z = 0; z < dungeon.Size.z; z++)
            {
                PaintBlock(dungeon[x, y, z],
                    new Vector3(x, y, z) * Dungeon.CellSize + dungeon.Position);
            }
            foreach (var stairs in dungeon.Staircases)
            {
                PaintStairs(stairs);
            }
            
            for (int y = 0; y < dungeon.Size.y; y++)
            for (int x = 0; x <= dungeon.Size.x; x++)
            for (int z = 0; z <= dungeon.Size.z; z++)
            {
                if (dungeon[x, y, z].HasBottomLeftColumn)
                    PaintColumn(new Vector3(x, y, z) * Dungeon.CellSize + dungeon.Position - new Vector3(1,0,1)*Dungeon.CellSize/2f);;
            }
        }

        private void PaintStairs(Stairs stairs)
        {
            PaintSingleTile(stairs.Position, environments.Stairs[stairs.GetTypeIndex()], stairs.RotationY); 
        }

        private void PaintColumn(Vector3 position)
        {
            PaintSingleTile(position, environments.Columns[Random.Range(0, environments.Columns.Length - 1)]);
        }

        private void PaintBlock(Block block, Vector3 position)
        {
            var cellSize= Dungeon.CellSize;
            if (block.HasTopWall)
            {
                PaintSingleTile(
                    position + new Vector3(0, 0, cellSize/2f),
                    environments.Walls[Random.Range(0, environments.Walls.Length)]);
            }
            if (block.HasBottomWall)
            {
                PaintSingleTile(
                    position + new Vector3(0, 0, -cellSize/2f),
                    environments.Walls[Random.Range(0, environments.Walls.Length)],
                    180);
            }
            if (block.HasRightWall)
            {
                PaintSingleTile(
                    position + new Vector3(cellSize/2f, 0, 0),
                    environments.Walls[Random.Range(0, environments.Walls.Length)],
                    90);
            }
            if (block.HasLeftWall)
            {
                PaintSingleTile(
                    position + new Vector3(-cellSize/2f, 0, 0),
                    environments.Walls[Random.Range(0,environments.Walls.Length)],
                    -90);
            }
            if (block.HasFloor)
            {
                PaintSingleTile(
                    position,
                    environments.Floors[Random.Range(0, environments.Floors.Length)]);
            }
            if (block.HasRoof)
            {
                PaintSingleTile(
                    position + new Vector3(0, cellSize, 0),
                    environments.Ceils[Random.Range(0, environments.Ceils.Length)]);
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
            int cellSize = Dungeon.CellSize;
            for (int x = 0; x < room.Size.x; x++)
            for (int z = 0; z < room.Size.z; z++)
            {
                PaintBlock(room[x,z], 
                    new Vector3(x * cellSize, 0, z * cellSize) + room.Position * cellSize);
            }
        }

        public void PaintPaths(HashSet<Vector3Int> pathPositions)
        {
            foreach (var position in pathPositions)
            {
                PaintSingleTile(Dungeon.WorldPosition(position), environments.Floors[Random.Range(0, environments.Floors.Length - 1)]);
            }
        }

        private void PaintBlocksArea(Block[,] cells, Vector3 center)
        {
            int cellSize = Dungeon.CellSize;
            for (int y = 0; y < cells.GetLength(0); y++)
            {
                for (int x = 0; x < cells.GetLength(1); x++)
                {
                    PaintBlock(cells[y,x], 
                        new Vector3(x * cellSize, 0, y * cellSize) + center * cellSize);
                }
            }
        }
    }
}
