using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Generator
{
    public class RandomWalkAreaGeneratorVector
    {

        public HashSet<Vector2Int> Generate(int diameter)
        {
            if (diameter < 1)
            {
                throw new ArgumentException();
            }

            HashSet<Vector2Int> roomArea = new HashSet<Vector2Int>();
            var position = new Vector2Int(0, 0);

            var MaxSteps = GetCountOfCells(roomArea, diameter);
            var MinSteps = MaxSteps * 2 / 3;
            var steps = Random.Range(MinSteps, MaxSteps) - 1;

            return SimpleRandomWalk(position, roomArea, steps, diameter);
        }
        private bool IsInValidRange(Vector2Int position, int diameter)
        {
            double xDelta = position.x;
            double yDelta = position.y;
            return Math.Sqrt(xDelta * xDelta + yDelta * yDelta) < diameter / 2f;
        }
        private int GetCountOfCells(HashSet<Vector2Int> roomArea, int diameter)
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
        public HashSet<Vector2Int> SimpleRandomWalk(Vector2Int position, HashSet<Vector2Int> roomArea, int steps, int diameter)
        {

            roomArea.Add(position);
            int count = 0;

            while (count < steps)
            {
                Vector2Int direction = Direction2.GetRandomDirection();
                position += direction;
                if (!IsInValidRange(position, diameter))
                {
                    position -= direction;
                    continue;
                }
                if (roomArea.Contains(position))
                {
                    continue;
                }
                roomArea.Add(position);
                count++;
            }
            return roomArea;
        }
    }
}
