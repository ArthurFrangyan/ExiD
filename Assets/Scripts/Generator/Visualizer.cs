using System.Collections.Generic;
using Data.ScriptableObjects;
using UnityEngine;
using static Generator.Dungeon;
using Random = UnityEngine.Random;

namespace Generator
{
    public class Visualizer : MonoBehaviour
    {
        [SerializeField]
        private EnvironmentScriptableObject environments;
        [SerializeField]
        private GameObject parentObject;

        private readonly List<GameObject> _instanceTiles = new();

        public void PaintDungeon(Dungeon dungeon)
        {
            for (int x = 0; x < dungeon.Size.x; x++)
            for (int y = 0; y < dungeon.Size.y; y++)
            for (int z = 0; z < dungeon.Size.z; z++)
            {
                PaintBlock(dungeon[x, y, z],
                    new Vector3(x, y, z) * CellSize + dungeon.Position);
            }
            foreach (var stairs in dungeon.Staircases)
            {
                PaintStairs(stairs);
            }
            
            for (int y = 0; y <= dungeon.Size.y; y++)
            for (int x = 0; x <= dungeon.Size.x; x++)
            for (int z = 0; z <= dungeon.Size.z; z++)
            {
                if (dungeon[x, y, z].HasBottomLeftColumn)
                    PaintColumn(new Vector3(x, y, z) * CellSize + dungeon.Position);
                if (dungeon[x, y, z].HasFloorLeftStoneCorner)
                    PaintFloorLeftStoneCorner(new Vector3(x, y, z) * CellSize + dungeon.Position);
                if (dungeon[x, y, z].HasFloorBottomStoneCorner)
                    PaintFloorBottomStoneCorner(new Vector3(x, y, z) * CellSize + dungeon.Position);
            }
        }

        private void PaintStairs(Stairs stairs)
        {
            PaintSingleTile(stairs.Position, environments.Stairs[stairs.GetTypeIndex()], stairs.RotationY); 
        }

        private void PaintColumn(Vector3 position)
        {
            PaintSingleTile(position - new Vector3(1,0,1)*CellSize/2f, environments.Columns[Random.Range(0, environments.Columns.Length - 1)]);
        }

        private void PaintFloorLeftStoneCorner(Vector3 position)
        {
            PaintSingleTile(position - new Vector3(0.5f,0,0.5f)*CellSize, environments.StoneCorners[Random.Range(0, environments.StoneCorners.Length - 1)]);
        }
        private void PaintFloorBottomStoneCorner(Vector3 position)
        {
            PaintSingleTile(position - new Vector3(0.5f,0,0.5f)*CellSize, environments.StoneCorners[Random.Range(0, environments.StoneCorners.Length - 1)], 90);
        }

        private void PaintBlock(Block block, Vector3 position)
        {
            if (block.HasTopWall)
            {
                PaintSingleTile(
                    position + new Vector3(0, 0, CellSize/2f),
                    environments.Walls[Random.Range(0, environments.Walls.Length)]);
            }
            if (block.HasBottomWall)
            {
                PaintSingleTile(
                    position + new Vector3(0, 0, -CellSize/2f),
                    environments.Walls[Random.Range(0, environments.Walls.Length)],
                    180);
            }
            if (block.HasRightWall)
            {
                PaintSingleTile(
                    position + new Vector3(CellSize/2f, 0, 0),
                    environments.Walls[Random.Range(0, environments.Walls.Length)],
                    90);
            }
            if (block.HasLeftWall)
            {
                PaintSingleTile(
                    position + new Vector3(-CellSize/2f, 0, 0),
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
                    position + new Vector3(0, CellSize, 0),
                    environments.Ceils[Random.Range(0, environments.Ceils.Length)]);
            }
        }

        private void PaintSingleTile(Vector3 position, GameObject tile, float yAngle = 0)
        {
            var floor = Instantiate(tile, parentObject.transform);
            floor.transform.position = position;
            floor.transform.eulerAngles = new Vector3(0, yAngle, 0);
            _instanceTiles.Add(floor);
        }

        public void Clean()
        {
            if (_instanceTiles is null)
                return;
            foreach (var instanceCell in _instanceTiles)
            {
                Destroy(instanceCell.gameObject);
            }
            _instanceTiles.Clear();
        }

        public void PaintRooms2D(IList<List<Room>> rooms)
        {
            foreach (var roomLiene in rooms)
            foreach (var room in roomLiene)
            {
                PaintRoom(room);
            }
        }
        public void PaintRoom(Room room)
        {
            for (int x = 0; x < room.Size.x; x++)
            for (int z = 0; z < room.Size.z; z++)
            {
                PaintBlock(room[x,z], (new Vector3(x, 0, z) + room.Position) * CellSize);
            }
        }

        public void PaintPaths(HashSet<Vector3Int> pathPositions)
        {
            foreach (var position in pathPositions)
            {
                PaintSingleTile(WorldPosition(position), environments.Floors[Random.Range(0, environments.Floors.Length - 1)]);
            }
        }

        private void PaintBlocksArea(Block[,] cells, Vector3 center)
        {
            for (int y = 0; y < cells.GetLength(0); y++)
            for (int x = 0; x < cells.GetLength(1); x++)
                PaintBlock(cells[y,x], (new Vector3(x, 0, y) + center) * CellSize);
        }
    }
}
