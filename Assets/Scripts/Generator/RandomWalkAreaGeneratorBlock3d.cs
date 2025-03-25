using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Generator;
using Generator.Library;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Generator
{
    public class RandomWalkAreaGeneratorBlock3d
    {

        public Block[][,] Generate(int diameter, int height, int countOfSubRooms)
        {
            if (diameter < 1)
                throw new ArgumentException($"{nameof(diameter)} < 1");
            if (height < 1)
                throw new ArgumentException($"{nameof(height)} < 1");

            var maxSteps = GetCountOfCells(diameter);
            var minSteps = maxSteps * 2 / 3;
            var steps = Random.Range(minSteps, maxSteps);

            var floors = new Block[height][,];
            for (int i = 0; i < floors.Length; i++)
            {
                floors[i] = GenerateFloor(diameter, new Vector2Int(diameter / 2, diameter / 2), 1, 1);
            }

            return floors;
        }
        private Block[,] GenerateFloor(int diameter, Vector2Int position, int steps, int countOfSubRooms)
        {
            Block[,] roomArea = new Block[diameter, diameter];

            SimpleRandomWalkByFloorCount(position, roomArea, steps, diameter);
            WallBuilder.Build(roomArea);

            int maxStepsSubRoom = steps / countOfSubRooms;
            int minStepsSubRoom = maxStepsSubRoom * 2 / 3;
            if (maxStepsSubRoom < 1)
                maxStepsSubRoom = 1;
            if (minStepsSubRoom < 1)
                minStepsSubRoom = 1;

            for (int i = 0; i < countOfSubRooms; i++)
            {
                int stepsSubRoom = Random.Range(minStepsSubRoom, maxStepsSubRoom);
                // GenerateSubRoom(GetRandomPositionInValidRangeAndHasNotFloor(diameter, roomArea), stepsSubRoom, diameter);
            }

            return roomArea;
        }

        private Vector2Int GetRandomPositionInValidRangeAndHasNotFloor(int diameter, Block[,] area)
        {
            Vector2Int position;
            do
            {
                position = new Vector2Int(Random.Range(0, diameter), Random.Range(0, diameter));
            } while (!IsInValidRange(position, diameter) || area[position.y, position.x].HasFloor);

            return position;
        }

        private Vector2Int GetRandomPositionInValidRangeAndHasFloor(HashSet<Vector2Int> roomAreaPaths)
        {
            return roomAreaPaths.ElementAt(Random.Range(0, roomAreaPaths.Count));
        }

        private bool IsInValidRange(Vector2Int position, int diameter)
        {
            return Sphere.IsInValidRange(new Vector3Int(position.x, 0, position.y), new Vector3((diameter - 1) / 2f, 0, (diameter - 1) / 2f), diameter);
        }
        private bool IsInValidRangeForArea(Vector2Int position, Block[,] area)
        {
            return 0 <= position.x && position.x < area.GetLength(0) && 0 <= position.y && position.y < area.GetLength(1);
        }
        private int GetCountOfCells(Block[,] room, int diameter)
        {
            int countOfCells = 0;
            for (int i = 0; i < room.GetLength(0); i++)
            {
                for (int j = 0; j < room.GetLength(1); j++)
                {
                    if (IsInValidRange(new Vector2Int(j, i), diameter))
                    {
                        countOfCells++;
                    }
                }
            }
            return countOfCells;
        }
        private int GetCountOfCells(int diameter)
        {
            int countOfCells = 0;
            for (int i = 0; i < diameter; i++)
            {
                for (int j = 0; j < diameter; j++)
                {
                    if (IsInValidRange(new Vector2Int(j, i), diameter))
                    {
                        countOfCells++;
                    }
                }
            }
            return countOfCells;
        }
        public void SimpleRandomWalkByFloorCount(Vector2Int startPosition, Block[,] roomArea, int floorCount, int diameter)
        {
            int countOfCells = 0;
            Vector2Int position = startPosition;

            roomArea[position.y, position.x].HasFloor = true;
            while (countOfCells < floorCount)
            {
                Vector2Int direction = Direction2.GetRandomDirection();
                position += direction;
                if (!IsInValidRange(position, diameter))
                {
                    position = startPosition;
                    continue;
                }
                if (roomArea[position.y, position.x].HasFloor)
                {
                    continue;
                }
                roomArea[position.y, position.x].HasFloor = true;
                countOfCells++;
            }
        }
        public void SimpleRandomWalkByFloorCount(Vector2Int startPosition, Block[,] roomArea, HashSet<Vector2Int> roomAreaFloors, int floorCount, int diameter)
        {
            int countOfCells = 0;
            Vector2Int position = startPosition;

            roomArea[position.y, position.x].HasFloor = true;
            while (countOfCells < floorCount)
            {
                Vector2Int direction = Direction2.GetRandomDirection();
                position += direction;
                if (!IsInValidRange(position, diameter))
                {
                    position = startPosition;
                    continue;
                }
                if (roomArea[position.y, position.x].HasFloor)
                {
                    continue;
                }
                roomArea[position.y, position.x].HasFloor = true;
                roomAreaFloors.Add(position);
                countOfCells++;
            }
        }
        public void SimpleRandomWalkByStepCount(Vector2Int startPosition, Block[,] roomArea, int steps, int diameter)
        {
            Vector2Int position = startPosition;
            int countOfCells = 0;
            while (countOfCells < steps)
            {
                Vector2Int direction = Direction2.GetRandomDirection();
                position += direction;
                if (!IsInValidRange(position, diameter))
                {
                    position = startPosition;
                    continue;
                }
                roomArea[position.y, position.x].HasFloor = true;
                countOfCells++;
            }
        }
    }
}
