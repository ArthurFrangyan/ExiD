using Generator.Library;
using UnityEngine;

namespace Generator.PathFinder.AStarAlgorithm
{
    public class VoxelGridVertex : IVoxelGridRef<Vertex>
    {
        private readonly Vertex[,,] _data;
        public Vector3Int Size { get; }

        public VoxelGridVertex(int x, int y, int z)
        {
            _data = new Vertex[x, y, z];
            Size = new Vector3Int(x, y, z);
        }

        public VoxelGridVertex(Vector3Int size) : this(size.x, size.y, size.z) { }

        public Vertex this[int x, int y, int z]
        {
            get => InBounds(x, y, z) ? _data[x, y, z] : null;
            set
            {
                if (InBounds(x, y, z))
                    _data[x, y, z] = value;
            }
        }

        public Vertex this[Vector3Int pos]
        {
            get => this[pos.x, pos.y, pos.z];
            set => this[pos.x, pos.y, pos.z] = value;
        }
        public bool InBounds(int x, int y, int z)
        {
            return x >= 0 && x < Size.x &&
                   y >= 0 && y < Size.y &&
                   z >= 0 && z < Size.z;
        }

        public bool InBounds(Vector3Int pos) => InBounds(pos.x, pos.y, pos.z);

        public Vertex[,,] Raw => _data;
        
        
        public static VoxelGridVertex GenerateVertexList(Dungeon dungeon)
        {
            VoxelGridVertex vertices = new VoxelGridVertex(dungeon.Size);
            
            for (int x = 0; x < dungeon.Size.x; x++)
            for (int y = 0; y < dungeon.Size.y; y++)
            for (int z = 0; z < dungeon.Size.z; z++)
            {
                vertices[x,y,z] = new Vertex(dungeon[x,y,z].IsLocked, new Vector3Int(x,y,z));
            }
            return vertices;
        }

        public void IncludeEdgeLock(Dungeon dungeon, Edge<Room> edge)
        {
            IncludeRoomLock(dungeon, edge.A);
            IncludeRoomLock(dungeon, edge.B);
        }

        public void ExcludeEdgeLock(Dungeon dungeon, Edge<Room> edge)
        {
            ExcludeRoomLock(dungeon, edge.A);
            ExcludeRoomLock(dungeon, edge.B);
        }

        public void IncludeRoomLock(Dungeon dungeon, Room room)
        {
            for (int i = 0; i < room.Blocks.GetLength(0); i++)
            for (int j = 0; j < room.Blocks.GetLength(1); j++)
            {
                if (room.Blocks[i, j].IsLocked)
                {
                    var pos = room.Position - dungeon.Position + new Vector3Int(j,0,i);
                    this[pos].IsLocked = true;
                }
            }
        }

        public void ExcludeRoomLock(Dungeon dungeon, Room room)
        {
            for (int i = 0; i < room.Blocks.GetLength(0); i++)
            for (int j = 0; j < room.Blocks.GetLength(1); j++)
            {
                if (room.Blocks[i, j].IsLocked)
                {
                    var pos = room.Position - dungeon.Position + new Vector3Int(j,0,i);
                    this[pos].IsLocked = false;
                }
            }
        }

        public void Reset()
        {
            for (var i0 = 0; i0 < _data.GetLength(0); i0++)
            for (var i1 = 0; i1 < _data.GetLength(1); i1++)
            for (var i2 = 0; i2 < _data.GetLength(2); i2++)
            {
                _data[i0, i1, i2].Predecessor = null;
                _data[i0, i1, i2].MinDistance = 0;
            }
        }
    }
}