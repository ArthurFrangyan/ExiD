using System;
using System.Collections.Generic;
using Generator.Library;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Generator
{
    public class RandomWalkAreaGenerator : IAreaGenerator
    {
        private MatrixHelper _matrixHelper;

        public Block[,] Generate(int diameter)
        {
            _matrixHelper = new MatrixHelper(diameter);
            if (diameter<1)
                throw new ArgumentException();
            
            var roomArea = new Block[diameter, diameter];

            var startPosition = new Vector2Int((int)_matrixHelper.Center.x, (int)_matrixHelper.Center.y);

            var steps = GetSteps(roomArea);

            roomArea[startPosition.y, startPosition.x].IsLocked = true;
            GenerateAreaByFloorCount(roomArea, steps, startPosition);

            return roomArea;
        }

        private int GetSteps(Block[,] roomArea)
        {
            var maxSteps = _matrixHelper.GetCountOfCells(roomArea);
            var minSteps = maxSteps*2/3;
            var steps = Random.Range(minSteps, maxSteps);
            return steps;
        }

        public void GenerateAreaByFloorCount(Block[,] roomArea, int floorCount, Vector2Int startPosition)
        {
            var processedFloors = 0;
            var position = startPosition;
            
            while (processedFloors < floorCount)
            {
                position += Direction2.GetRandomDirection();
                
                if (!_matrixHelper.IsInValidRange(position))
                {
                    position = startPosition;
                    continue;
                }
                if (roomArea[position.y, position.x].IsLocked)
                    continue;
                
                roomArea[position.y, position.x].IsLocked = true;
                processedFloors++;
            }
        }
        public void GenerateAreaByStepCount(Vector2Int startPosition, Block[,] roomArea, int steps)
        {
            int countOfCells = 0;
            Vector2Int position = startPosition;
            while (countOfCells < steps)
            {
                position += Direction2.GetRandomDirection();
                if (!_matrixHelper.IsInValidRange(position))
                {
                    position = startPosition;
                    continue;
                }
                roomArea[position.y, position.x].IsLocked = true;
                countOfCells++;
            }
        }
    }
}
