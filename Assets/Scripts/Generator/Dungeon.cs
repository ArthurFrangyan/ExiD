using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts.Data;
using Generator.PathFinders;
using UnityEngine;

namespace Generator
{
    public class Dungeon : IEnumerable<Block>
    {
        private Block[,,] Blocks { get; set; }
        public Vector3Int Size { get; }
        public Vector3Int Position { get; set; }
        public static int CellSize { get; set; } = 4;
        public List<Stairs> Staircases { get; set; }

        public Dungeon(int xSize, int ySize, int zSize, Vector3Int position)
        {
            Blocks = new Block[xSize+1, ySize, zSize+1];
            Size = new Vector3Int(xSize, ySize, zSize);
            Staircases = new List<Stairs>();
            Position = position;
        }
        public Dungeon(Vector3Int size, Vector3Int position) : this(size.x, size.y, size.z, position) { }
        
        public ref Block this[int x, int y, int z] => ref Blocks[x, y, z];

        public ref Block this[Vector3Int pos] => ref this[pos.x, pos.y, pos.z];

        public bool InBounds(int x, int y, int z)
        {
            return x >= 0 && x < Size.x &&
                   y >= 0 && y < Size.y &&
                   z >= 0 && z < Size.z;
        }

        public bool InBounds(Vector3Int pos) => InBounds(pos.x, pos.y, pos.z);

        public static Dungeon GenerateDungeonMatrix(List<Room> rooms)
        {
            var voxelGridProps = GetMatrixDimension(rooms);
            Dungeon dungeon = new Dungeon(voxelGridProps.Size, voxelGridProps.Position);
            AddRoomsToDungeon(dungeon, rooms);
            return dungeon;
        }

        private static void AddRoomsToDungeon(Dungeon dungeon, List<Room> rooms)
        {
            foreach (var room in rooms)
            {
                for (int i = 0; i < room.Blocks.GetLength(0); i++)
                for (int j = 0; j < room.Blocks.GetLength(1); j++)
                {
                    dungeon[room.Position - dungeon.Position + new Vector3Int(j,0,i)] = room.Blocks[i, j];
                }
            }
        }
        
        private static VoxelGridProps GetMatrixDimension(List<Room> rooms)
        {
            if (rooms == null || rooms.Count == 0)
                throw new ArgumentException("Rooms collection cannot be null or empty");
            
            var maxX = rooms.Max(r => r.Position.x + r.Diameter);
            var maxY = rooms.Max(r => r.Position.y) + 1; // TODO: Height
            var maxZ = rooms.Max(r => r.Position.z + r.Diameter);
            
            var minX = rooms.Min(r => r.Position.x);
            var minY = rooms.Min(r => r.Position.y);
            var minZ = rooms.Min(r => r.Position.z);
            
            var max = new Vector3Int(maxX, maxY, maxZ);
            var min = new Vector3Int(minX, minY, minZ);
            return new VoxelGridProps(min,max - min, CellSize);
        }

        private struct VoxelGridProps
        {
            public Vector3Int Position { get; }
            public Vector3Int Size { get; }
            public int CellSize { get; }
            public VoxelGridProps(Vector3Int position, Vector3Int size, int cellSize)
            {
                Position = position;
                Size = size;
                CellSize = cellSize;
            }
        }

        public IEnumerator<Block> GetEnumerator()
        {
            return Blocks.Cast<Block>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static Vector3 WorldPosition(Vector3Int position)
        {
            return position * CellSize;
        }
    }
}