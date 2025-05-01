using System;

namespace Generator
{
    public static class BlockBuilder
    {
        public static void BuildWallsCeilFloorBasedOnLock(Dungeon dungeon)
        {
            BuildXWallsBasedOnLock(dungeon);
            BuildYWallsBasedOnLock(dungeon);
            BuildZFloorAndCeilBasedOnLock(dungeon);
        }

        [Obsolete]
        private static void TempBuildFloors(Dungeon dungeon)
        {
            for (int x = 0; x < dungeon.Size.x; x++)
            for (int y = 0; y < dungeon.Size.y; y++)
            for (int z = 0; z < dungeon.Size.z; z++)
                if (dungeon[x, y, z].IsLocked)
                    dungeon[x, y, z].HasFloor = true;
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

        private static void BuildYWallsBasedOnLock(Dungeon dungeon)
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

        private static void BuildZFloorAndCeilBasedOnLock(Dungeon dungeon)
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
                        current.HasCeil = true;
                    if (!current.IsLocked && next.IsLocked)
                        next.HasFloor = true;
                    current = next;
                }

                if (dungeon[x, dungeon.Size.y - 1, z].IsLocked)
                    dungeon[x, dungeon.Size.y - 1, z].HasCeil = true;
            }
        }
    }
}