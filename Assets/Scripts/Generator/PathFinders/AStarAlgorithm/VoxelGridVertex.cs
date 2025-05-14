using Generator.Library;
using UnityEngine;

namespace Generator.PathFinders.AStarAlgorithm
{
    public class VoxelGridVertex : IVoxelGridRef<Vertex>
    {
        private readonly Vertex[,,] _data;
        public Vector3Int Size { get; }

        public VoxelGridVertex(Dungeon dungeon) : this(dungeon.Size)
        {
            for (int x = 0; x < dungeon.Size.x; x++)
            for (int y = 0; y < dungeon.Size.y; y++)
            for (int z = 0; z < dungeon.Size.z; z++)
            {
                _data[x,y,z] = new Vertex(dungeon[x,y,z].IsLocked, new Vector3Int(x,y,z));
            }
        }

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


        public void MarkStartAndUnlockRoom(Dungeon dungeon, Room room)
        {
            for (int x = 0; x < room.Size.x; x++)
            for (int z = 0; z < room.Size.z; z++)
            {
                if (room[z, x].IsLocked)
                {
                    var pos = room.Position - dungeon.Position + new Vector3Int(x,0,z);
                    this[pos].IsStart = true;
                    this[pos].IsLocked = false;
                }
            }
        }

        public void RevertStartRoomToLocked(Dungeon dungeon, Room room)
        {
            for (int x = 0; x < room.Size.x; x++)
            for (int z = 0; z < room.Size.z; z++)
            {
                if (room[x, z].IsLocked)
                {
                    var pos = room.Position - dungeon.Position + new Vector3Int(x,0,z);
                    this[pos].IsStart = false;
                    this[pos].IsLocked = true;
                }
            }
        }

        public void MarkGoalAndUnlockRoom(Dungeon dungeon, Room room)
        {
            for (int x = 0; x < room.Size.x; x++)
            for (int z = 0; z < room.Size.z; z++)
            {
                if (room[x, z].IsLocked)
                {
                    var pos = room.Position - dungeon.Position + new Vector3Int(x,0,z);
                    this[pos].IsGoal = true;
                    this[pos].IsLocked = false;
                }
            }
        }

        public void RevertGoalRoomToLocked(Dungeon dungeon, Room room)
        {
            for (int x = 0; x < room.Size.x; x++)
            for (int z = 0; z < room.Size.z; z++)
            {
                if (room[x, z].IsLocked)
                {
                    var pos = room.Position - dungeon.Position + new Vector3Int(x,0,z);
                    this[pos].IsGoal = false;
                    this[pos].IsLocked = true;
                }
            }
        }

        public void ResetPathAnalysisData()
        {
            for (var x = 0; x < _data.GetLength(0); x++)
            for (var y = 0; y < _data.GetLength(1); y++)
            for (var z = 0; z < _data.GetLength(2); z++)
            {
                _data[x, y, z].Predecessor = null;
                _data[x, y, z].MinDistance = 0;
            }
        }

        public void IncludeRoomLock(Dungeon dungeon, Room room)
        {
            for (int x = 0; x < room.Size.x; x++)
            for (int z = 0; z < room.Size.z; z++)
            {
                if (room[x, z].IsLocked)
                {
                    var pos = room.Position - dungeon.Position + new Vector3Int(x,0,z);
                    this[pos].IsLocked = true;
                }
            }
        }

        public void ExcludeRoomLock(Dungeon dungeon, Room room)
        {
            for (int x = 0; x < room.Size.x; x++)
            for (int z = 0; z < room.Size.z; z++)
            {
                if (room[x, z].IsLocked)
                {
                    var pos = room.Position - dungeon.Position + new Vector3Int(x,0,z);
                    this[pos].IsLocked = false;
                }
            }
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

        public void RevertAllGoalVerticesToLocked()
        {
            for (int i = 0; i < _data.GetLength(0); i++)
            for (int j = 0; j < _data.GetLength(1); j++)
            for (int k = 0; k < _data.GetLength(2); k++)
            {
                if (_data[i, j, k].IsGoal)
                {
                    _data[i, j, k].IsGoal = false;
                    _data[i, j, k].IsLocked = true;
                }
            }
        }

        public void RevertAllStartVerticesToLocked()
        {
            for (int i = 0; i < _data.GetLength(0); i++)
            for (int j = 0; j < _data.GetLength(1); j++)
            for (int k = 0; k < _data.GetLength(2); k++)
            {
                if (_data[i, j, k].IsGoal)
                {
                    _data[i, j, k].IsStart = false;
                    _data[i, j, k].IsLocked = true;
                }
            }
        }
    }
}