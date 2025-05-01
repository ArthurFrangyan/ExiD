namespace Generator.WallBlockBuilder
{
    public static class WallBuilder
    {
        public static void Build(Block[,] roomArea)
        {
            BuildCentral(roomArea);
            BuildLeftRightEdge(roomArea);
            BuildTopBottomEdge(roomArea);
            BuildLeftTop(roomArea);
            BuildRightTop(roomArea);
            BuildLeftBottom(roomArea);
            BuildRightBottom(roomArea);
        }

        private static void BuildCentral(Block[,] roomArea)
        {
            for (var i = 1; i < roomArea.GetLength(0) - 1; i++)
            {
                for (var j = 1; j < roomArea.GetLength(1) - 1; j++)
                {
                    if (roomArea[i, j].HasFloor)
                    {
                        if (roomArea[i + 1, j].HasFloor)
                            roomArea[i, j].HasBottomWall = true;
                        if (roomArea[i - 1, j].HasFloor)
                            roomArea[i, j].HasTopWall = true;
                        if (roomArea[i, j + 1].HasFloor)
                            roomArea[i, j].HasRightWall = true;
                        if (roomArea[i, j - 1].HasFloor)
                            roomArea[i, j].HasLeftWall = true;
                    }
                }
            }
        }

        private static void BuildLeftRightEdge(Block[,] roomArea)
        {
            int i;
            for (i = 1; i < roomArea.GetLength(0) - 1; i++)
            {
                var j = 0;
                if (roomArea[i, j].HasFloor)
                {
                    roomArea[i, j].HasLeftWall = true;
                    if (roomArea[i + 1, j].HasFloor)
                        roomArea[i, j].HasBottomWall = true;
                    if (roomArea[i - 1, j].HasFloor)
                        roomArea[i, j].HasTopWall = true;
                    if (roomArea[i, j + 1].HasFloor)
                        roomArea[i, j].HasRightWall = true;
                }
                j = roomArea.GetLength(1) - 1;
                if (roomArea[i, j].HasFloor)
                {
                    roomArea[i, j].HasRightWall = true;
                    if (roomArea[i + 1, j].HasFloor)
                        roomArea[i, j].HasBottomWall = true;
                    if (roomArea[i - 1, j].HasFloor)
                        roomArea[i, j].HasTopWall = true;
                    if (roomArea[i, j - 1].HasFloor)
                        roomArea[i, j].HasLeftWall = true;
                }
            }
        }

        private static void BuildTopBottomEdge(Block[,] roomArea)
        {
            int j;
            for (j = 0; j < roomArea.GetLength(1); j++)
            {
                var i = 0;
                if (roomArea[i, j].HasFloor)
                {
                    roomArea[i, j].HasTopWall = true;
                    if (roomArea[i + 1, j].HasFloor)
                        roomArea[i, j].HasBottomWall = true;
                    if (roomArea[i, j + 1].HasFloor)
                        roomArea[i, j].HasRightWall = true;
                    if (roomArea[i, j - 1].HasFloor)
                        roomArea[i, j].HasLeftWall = true;
                }
                i = roomArea.GetLength(0) - 1;
                if (roomArea[i, j].HasFloor)
                {
                    roomArea[i, j].HasBottomWall = true;
                    if (roomArea[i - 1, j].HasFloor)
                        roomArea[i, j].HasTopWall = true;
                    if (roomArea[i, j + 1].HasFloor)
                        roomArea[i, j].HasRightWall = true;
                    if (roomArea[i, j - 1].HasFloor)
                        roomArea[i, j].HasLeftWall = true;
                }
            }
        }

        private static void BuildLeftTop(Block[,] roomArea)
        {
            if (roomArea[0, 0].HasFloor)
            {
                roomArea[0, 0].HasTopWall = true;
                roomArea[0, 0].HasLeftWall = true;
                if (roomArea[1, 0].HasFloor)
                    roomArea[0, 0].HasBottomWall = true;
                if (roomArea[0, 1].HasFloor)
                    roomArea[0, 0].HasRightWall = true;
            }
        }

        private static void BuildRightTop(Block[,] roomArea)
        {
            var j = roomArea.GetLength(1) - 1;
            if (roomArea[0, j].HasFloor)
            {
                roomArea[0, j].HasTopWall = true;
                roomArea[0, j].HasRightWall = true;
                if (roomArea[1, j].HasFloor)
                    roomArea[0, j].HasBottomWall = true;
                if (roomArea[0, j - 1].HasFloor)
                    roomArea[0, j].HasLeftWall = true;
            }
        }

        private static void BuildLeftBottom(Block[,] roomArea)
        {
            var i = roomArea.GetLength(0) - 1;
            if (roomArea[i, 0].HasFloor)
            {
                roomArea[i, 0].HasBottomWall = true;
                roomArea[i, 0].HasLeftWall = true;
                if (roomArea[i - 1, 0].HasFloor)
                    roomArea[i, 0].HasTopWall = true;
                if (roomArea[i, 1].HasFloor)
                    roomArea[i, 0].HasRightWall = true;
            }
        }

        private static void BuildRightBottom(Block[,] roomArea)
        {
            var i = roomArea.GetLength(0) - 1;
            var j = roomArea.GetLength(1) - 1;
            if (roomArea[i, j].HasFloor)
            {
                roomArea[i, j].HasBottomWall = true;
                roomArea[i, j].HasRightWall = true;
                if (roomArea[i - 1, j].HasFloor)
                    roomArea[i, j].HasTopWall = true;
                if (roomArea[i, j - 1].HasFloor)
                    roomArea[i, j].HasLeftWall = true;
            }
        }
    }
}