using UnityEngine;
using static Generator.Library.VectorExt;
using static UnityEngine.Vector3Int;

namespace Generator
{
    public static class BlockBuilder
    {
        public static void BuildBordersBasedOnLock(Dungeon dungeon)
        {
            BuildWallsBasedOnLock(dungeon, Axes.X);
            BuildWallsBasedOnLock(dungeon, Axes.Y);
            BuildWallsBasedOnLock(dungeon, Axes.Z);
        }

        public static void BuildBordersBasedOnLockLevelSeparately(Dungeon dungeon)
        {
            BuildWallsBasedOnLock(dungeon, Axes.X);
            BuildWallsBasedOnLock(dungeon, Axes.Z);
            BuildWallsBasedOnLockAndConnection(dungeon);
        }

        private static void BuildWallsBasedOnLock(Dungeon dungeon, Axes axes)
        {
            foreach (var abPos in axes.AB(zero, dungeon.Size))
            {
                var firstPos = abPos;
                if (dungeon[firstPos].IsLocked)
                    dungeon[firstPos].SetBorder(-axes.CVec, true);
                foreach (var secondPos in axes.C(abPos + axes.CVec, dungeon.Size))
                {
                    if (!dungeon[secondPos].IsLocked && dungeon[firstPos].IsLocked)
                        dungeon[firstPos].SetBorder(axes.CVec, true);
                    if (!dungeon[firstPos].IsLocked && dungeon[secondPos].IsLocked)
                        dungeon[secondPos].SetBorder(-axes.CVec, true);
                    
                    firstPos = secondPos;
                }

                if (dungeon[firstPos].IsLocked)
                    dungeon[firstPos].SetBorder(axes.CVec, true);
            }
        }

        private static void BuildWallsBasedOnLockAndConnection(Dungeon dungeon)
        {
            var axes = Axes.Y;
            foreach (var abPos in axes.AB(zero, dungeon.Size))
            {
                var firstPos = abPos;
                if (dungeon[firstPos].IsLocked)
                    dungeon[firstPos].SetBorder(-axes.CVec, true);
                foreach (var secondPos in axes.C(abPos + axes.CVec, dungeon.Size))
                {
                    ref var current = ref dungeon[firstPos];
                    ref var next = ref dungeon[secondPos];
                    if (!next.IsLocked && current.IsLocked || current is { ConnectedToRoof: false, IsLocked: true })
                        dungeon[firstPos].SetBorder(axes.CVec, true);
                    if (!current.IsLocked && next.IsLocked || next is { ConnectedToFloor: false, IsLocked: true })
                        dungeon[secondPos].SetBorder(-axes.CVec, true);
                    
                    firstPos = secondPos;
                }

                if (dungeon[firstPos].IsLocked)
                    dungeon[firstPos].SetBorder(axes.CVec, true);
            }
        }


        public static void BuildYWallsBasedOnLockSeparately(Dungeon dungeon, Axes axes)
        {
            foreach (var pos in axes.ABC(zero, dungeon.Size))
            {
                if (dungeon[pos].IsLocked)
                {
                    dungeon[pos].SetBorder(-axes.CVec, true);
                    dungeon[pos].SetBorder(axes.CVec, true);
                }
            }
        }

        public static void ReBuildEdgesByWalls(Dungeon dungeon)
        {
            ReBuildEdgesByWalls(dungeon, Axes.Y);
            // TODO stoneCorners
        }


        private static void ReBuildEdgesByWalls(Dungeon dungeon, Axes axes)
        {
            var edge = -axes.AVec - axes.BVec;
            foreach (var pos in axes.ABC(axes.AVec + axes.BVec, dungeon.Size + axes.AVec + axes.BVec)) 
                dungeon[pos].SetBorder(edge, IsValidEdgeBetween4Faces(dungeon, axes, pos));
            
            foreach (var pos in axes.BC(axes.BVec, dungeon.Size + axes.BVec))
                dungeon[pos].SetBorder(edge, IsValidEdgeBWithoutA(dungeon, axes, pos));
            
            foreach (var pos in axes.AC(axes.AVec, dungeon.Size + axes.AVec))
                dungeon[pos].SetBorder(edge, IsValidEdgeAWithoutB(dungeon, axes, pos));
            
            foreach (var pos in axes.C(zero, dungeon.Size))
                dungeon[pos].SetBorder(edge, IsValidEdge(dungeon, axes, pos));
        }

        private static bool IsValidEdgeBetween4Faces(Dungeon dungeon, Axes axes, Vector3Int pos)
        {
            return dungeon[pos].GetBorder(-axes.AVec) || dungeon[pos].GetBorder(-axes.BVec) ||
                   dungeon[pos - axes.BVec].GetBorder(-axes.AVec) || dungeon[pos - axes.BVec].GetBorder(axes.BVec) ||
                   dungeon[pos - axes.AVec].GetBorder(axes.AVec) || dungeon[pos - axes.AVec].GetBorder(-axes.BVec) || 
                   dungeon[pos - axes.AVec - axes.BVec].GetBorder(axes.AVec) || dungeon[pos - axes.BVec - axes.AVec].GetBorder(axes.BVec);
        }

        private static bool IsValidEdgeAWithoutB(Dungeon dungeon, Axes axes, Vector3Int pos)
        {
            return dungeon[pos].GetBorder(-axes.AVec) || dungeon[pos].GetBorder(-axes.BVec) ||
                   dungeon[pos - axes.AVec].GetBorder(axes.AVec) || dungeon[pos - axes.AVec].GetBorder(-axes.BVec);
        }

        private static bool IsValidEdgeBWithoutA(Dungeon dungeon, Axes axes, Vector3Int pos)
        {
            return dungeon[pos].GetBorder(-axes.AVec) || dungeon[pos].GetBorder(-axes.BVec) ||
                   dungeon[pos - axes.BVec].GetBorder(-axes.AVec) || dungeon[pos - axes.BVec].GetBorder(axes.BVec);
        }

        private static bool IsValidEdge(Dungeon dungeon, Axes axes, Vector3Int pos)
        {
            return dungeon[pos].GetBorder(-axes.AVec) || dungeon[pos].GetBorder(-axes.BVec);
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

        private static void BuildColumnsByBlockLock(Dungeon dungeon)
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

        public static void CarvePath(Dungeon dungeon, Room room, Vector3Int start)
        {
            Vector3Int pos;

            pos = start + left + dungeon.Position - room.Position;
            if (room.InBoundary(pos))
            {
                dungeon[start].HasRightWall = false;
            }
            pos = start + right + dungeon.Position - room.Position;
            if (room.InBoundary(pos))
            {
                dungeon[start].HasLeftWall = false;
            }
            pos = start + forward + dungeon.Position - room.Position;
            if (room.InBoundary(pos))
            {
                dungeon[start].HasBottomWall = false;
            }
            pos = start + back + dungeon.Position - room.Position;
            if (room.InBoundary(pos))
            {
                dungeon[start].HasTopWall = false;
            }
            pos = start + up + dungeon.Position - room.Position;
            if (room.InBoundary(pos))
            {
                dungeon[start].HasFloor = false;
            }
            pos = start + down + dungeon.Position - room.Position;
            if (room.InBoundary(pos))
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
    }
}