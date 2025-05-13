using UnityEngine;
using static Generator.Library.VectorExt;
using static UnityEngine.Vector3Int;

namespace Generator
{
    public static class BlockBuilder
    {
        public static void BuildBordersBasedOnLock(Dungeon dungeon)
        {
            BuildXWallsBasedOnLock(dungeon);
            BuildZWallsBasedOnLock(dungeon);
            BuildYWallsBasedOnLock(dungeon);
        }

        private static void BuildXWallsBasedOnLock(Dungeon dungeon)
        {
            for (int z = 0; z < dungeon.Size.z; z++)
            for (int y = 0; y < dungeon.Size.y; y++)
            {
                if (dungeon[0, y, z].IsLocked)
                    dungeon[0, y, z].HasLeftWall = true;
                for (int x = 1; x < dungeon.Size.x; x++)
                {
                    if (!dungeon[x, y, z].IsLocked && dungeon[x - 1, y, z].IsLocked)
                        dungeon[x - 1, y, z].HasRightWall = true;
                    if (!dungeon[x - 1, y, z].IsLocked && dungeon[x, y, z].IsLocked)
                        dungeon[x, y, z].HasLeftWall = true;
                }

                if (dungeon[dungeon.Size.x - 1, y, z].IsLocked)
                    dungeon[dungeon.Size.x - 1, y, z].HasRightWall = true;
            }
        }

        private static void BuildZWallsBasedOnLock(Dungeon dungeon)
        {
            for (int x = 0; x < dungeon.Size.x; x++)
            for (int y = 0; y < dungeon.Size.y; y++)
            {
                if (dungeon[x, y, 0].IsLocked)
                    dungeon[x, y, 0].HasBottomWall = true;
                for (int z = 1; z < dungeon.Size.z; z++)
                {
                    ref var current = ref dungeon[x, y, z - 1];
                    ref var next = ref dungeon[x, y, z];
                    if (!next.IsLocked && current.IsLocked)
                        current.HasTopWall = true;
                    if (!current.IsLocked && next.IsLocked)
                        next.HasBottomWall = true;
                }

                if (dungeon[x, y, dungeon.Size.z - 1].IsLocked)
                    dungeon[x, y, dungeon.Size.z - 1].HasTopWall = true;
            }
        }

        private static void BuildYWallsBasedOnLock(Dungeon dungeon)
        {
            for (int x = 0; x < dungeon.Size.x; x++)
            for (int z = 0; z < dungeon.Size.z; z++)
            {
                if (dungeon[x, 0, z].IsLocked)
                    dungeon[x, 0, z].HasFloor = true;
                for (int y = 1; y < dungeon.Size.y; y++)
                {
                    ref var current = ref dungeon[x, y - 1, z];
                    ref var next = ref dungeon[x, y, z];
                    if (!next.IsLocked && current.IsLocked)
                        current.HasRoof = true;
                    if (!current.IsLocked && next.IsLocked)
                        next.HasFloor = true;
                }

                if (dungeon[x, dungeon.Size.y - 1, z].IsLocked)
                    dungeon[x, dungeon.Size.y - 1, z].HasRoof = true;
            }
        }

        public static void BuildBordersBasedOnLockLevelSeparately(Dungeon dungeon)
        {
            BuildXWallsBasedOnLock(dungeon);
            BuildZWallsBasedOnLock(dungeon);
            BuildYWallsBasedOnLockCountConnection(dungeon);
        }
        private static void BuildYWallsBasedOnLockCountConnection(Dungeon dungeon)
        {
            for (int x = 0; x < dungeon.Size.x; x++)
            for (int z = 0; z < dungeon.Size.z; z++)
            {
                if (dungeon[x, 0, z].IsLocked)
                    dungeon[x, 0, z].HasFloor = true;
                for (int y = 1; y < dungeon.Size.y; y++)
                {
                    ref var current = ref dungeon[x, y - 1, z];
                    ref var next = ref dungeon[x, y, z];
                    if (!next.IsLocked && current.IsLocked || current is { ConnectedToRoof: false, IsLocked: true })
                        current.HasRoof = true;
                    if (!current.IsLocked && next.IsLocked || next is { ConnectedToFloor: false, IsLocked: true })
                        next.HasFloor = true;
                }

                if (dungeon[x, dungeon.Size.y - 1, z].IsLocked)
                    dungeon[x, dungeon.Size.y - 1, z].HasRoof = true;
            }
        }


        public static void BuildYWallsBasedOnLockSeparately(Dungeon dungeon)
        {
            for (int y = 0; y < dungeon.Size.y; y++)
            for (int x = 0; x < dungeon.Size.x; x++)
            for (int z = 0; z < dungeon.Size.z; z++)
            {
                if (dungeon[x, y, z].IsLocked)
                {
                    dungeon[x, y, z].HasFloor = true;
                    dungeon[x, y, z].HasRoof = true;
                }
            }
        }

        public static void CarvePath(Dungeon dungeon, Room room, Vector3Int start)
        {
            Vector3Int pos;

            pos = start + left + dungeon.Position - room.Position;
            if (room.InBoundaryRoom(pos))
            {
                dungeon[start].HasRightWall = false;
            }
            pos = start + right + dungeon.Position - room.Position;
            if (room.InBoundaryRoom(pos))
            {
                dungeon[start].HasLeftWall = false;
            }
            pos = start + forward + dungeon.Position - room.Position;
            if (room.InBoundaryRoom(pos))
            {
                dungeon[start].HasBottomWall = false;
            }
            pos = start + back + dungeon.Position - room.Position;
            if (room.InBoundaryRoom(pos))
            {
                dungeon[start].HasTopWall = false;
            }
            pos = start + up + dungeon.Position - room.Position;
            if (room.InBoundaryRoom(pos))
            {
                dungeon[start].HasFloor = false;
            }
            pos = start + down + dungeon.Position - room.Position;
            if (room.InBoundaryRoom(pos))
            {
                dungeon[start].HasRoof = false;
            }
        }
        public static void CarvePath(Dungeon dungeon, Vector3Int start, Vector3Int end)
        {
            Vector3Int dir = end - start;

            switch (dir)
            {
                case var d when d == right:
                    dungeon[start].HasRightWall = false;
                    dungeon[end].HasLeftWall = false;
                    break;
                case var d when d == left:
                    dungeon[start].HasLeftWall = false;
                    dungeon[end].HasRightWall = false;
                    break;
                case var d when d == forward:
                    dungeon[start].HasTopWall = false;
                    dungeon[end].HasBottomWall = false;
                    break;
                case var d when d == back:
                    dungeon[start].HasBottomWall = false;
                    dungeon[end].HasTopWall = false;
                    break;
                case var d when d == up:
                    dungeon[start].HasRoof = false;
                    dungeon[end].HasFloor = false;
                    break;
                case var d when d == down:
                    dungeon[start].HasFloor = false;
                    dungeon[end].HasRoof = false;
                    break;
            }
        }

        public static void BuildStoneCornersByWalls(Dungeon dungeon)
        {
            throw new System.NotImplementedException();
        }
        public static void ReBuildColumnsByWalls(Dungeon dungeon)
        {
            for (int y = 0; y < dungeon.Size.y; y++)
            for (int x = 1; x <= dungeon.Size.x; x++)
            for (int z = 1; z <= dungeon.Size.z; z++)
            {
                dungeon[x, y, z].HasBottomLeftColumn = 
                    IsValidColumnBetween4Walls(dungeon, new Vector3Int(x, y, z));
            }
            
            for (int y = 0; y < dungeon.Size.y; y++)
            for (int x = 1; x < dungeon.Size.x; x++)
            {
                dungeon[x, y, 0].HasBottomLeftColumn = 
                    IsValidColumnXWallsWithoutZ(dungeon, new Vector3Int(x, y, 0));
            }
            for (int y = 0; y < dungeon.Size.y; y++)
            for (int z = 1; z < dungeon.Size.z; z++)
            {
                dungeon[0, y, z].HasBottomLeftColumn = 
                    IsValidColumnZWallsWithoutX(dungeon, new Vector3Int(0, y, z));
            }
            for (int y = 0; y < dungeon.Size.y; y++)
            {
                dungeon[0, y, 0].HasBottomLeftColumn = 
                    IsValidColumn(dungeon, new Vector3Int(0, y, 0));
            }
        }

        private static bool IsValidColumnBetween4Walls(Dungeon dungeon, Vector3Int pos)
        {
            return dungeon[pos].HasLeftWall || dungeon[pos].HasBottomWall ||
                   dungeon[pos + back].HasLeftWall || dungeon[pos + back].HasTopWall ||
                   dungeon[pos + left].HasRightWall || dungeon[pos + left].HasBottomWall || 
                   dungeon[pos + BackLeft].HasRightWall || dungeon[pos + BackLeft].HasTopWall;
        }

        private static bool IsValidColumnXWallsWithoutZ(Dungeon dungeon, Vector3Int pos)
        {
            return dungeon[pos].HasLeftWall || dungeon[pos].HasBottomWall ||
                   dungeon[pos + left].HasRightWall || dungeon[pos + left].HasBottomWall;
        }

        private static bool IsValidColumnZWallsWithoutX(Dungeon dungeon, Vector3Int pos)
        {
            return dungeon[pos].HasLeftWall || dungeon[pos].HasBottomWall ||
                   dungeon[pos + back].HasLeftWall || dungeon[pos + back].HasTopWall;
        }

        private static bool IsValidColumn(Dungeon dungeon, Vector3Int pos)
        {
            return dungeon[pos].HasLeftWall || dungeon[pos].HasBottomWall;
        }

        public static void BuildColumnsByBlockLock(Dungeon dungeon)
        {
            for (int y = 0; y < dungeon.Size.y; y++)
            for (int x = 1; x <= dungeon.Size.x; x++)
            for (int z = 1; z <= dungeon.Size.z; z++)
            {
                if (IsValidColumnBetween4Blocks(dungeon, new Vector3Int(x, y, z)))
                    dungeon[x, y, z].HasBottomLeftColumn = true;
            }
            
            for (int y = 0; y < dungeon.Size.y; y++)
            for (int x = 1; x < dungeon.Size.x; x++)
            {
                if (IsValidColumnXBlocksWithoutZ(dungeon, new Vector3Int(x, y, 0)))
                    dungeon[x, y, 0].HasBottomLeftColumn = true;
            }
            for (int y = 0; y < dungeon.Size.y; y++)
            for (int z = 1; z < dungeon.Size.z; z++)
            {
                if (IsValidColumnZBlocksWithoutX(dungeon, new Vector3Int(0, y, z)))
                    dungeon[0, y, z].HasBottomLeftColumn = true;
            }
            for (int y = 0; y < dungeon.Size.y; y++)
            {
                if (dungeon[0, y, 0].IsLocked) 
                    dungeon[0, y, 0].HasBottomLeftColumn = true;
            }
        }

        private static bool IsValidColumnBetween4Blocks(Dungeon dungeon, Vector3Int position)
        {
            return (dungeon[position].IsLocked ||
                    dungeon[position + back].IsLocked ||
                    dungeon[position + left].IsLocked ||
                    dungeon[position + BackLeft].IsLocked) 
                   &&
                   !(dungeon[position].IsLocked && 
                     dungeon[position + back].IsLocked && 
                     dungeon[position + left].IsLocked &&
                     dungeon[position + BackLeft].IsLocked);
        }

        private static bool IsValidColumnXBlocksWithoutZ(Dungeon dungeon, Vector3Int position)
        {
            return dungeon[position].IsLocked ||
                   dungeon[position + left].IsLocked;
        }

        private static bool IsValidColumnZBlocksWithoutX(Dungeon dungeon, Vector3Int position)
        {
            return dungeon[position].IsLocked ||
                   dungeon[position + back].IsLocked;
        }
    }
}