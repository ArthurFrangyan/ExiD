using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Generator
{
    public class RandomWalkAreaGenerator
    {
        private Vector2 center;

        public int[,] Generate(int diameter)
        {
            if (diameter<1)
            {
                throw new ArgumentException();
            }


            center = new Vector2((diameter - 1) / 2f, (diameter - 1) / 2f);
            var room = new int[diameter, diameter];

            var position = new Vector2Int(diameter/2, diameter/2);

            var MaxSteps = GetCountOfCells(room, diameter);
            var MinSteps = MaxSteps*2/3;
            var steps = Random.Range(MinSteps, MaxSteps) - 1;

            room[position.y, position.x] = 1;
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
                if (room[position.y, position.x] == 1)
                {
                    continue;
                }
                room[position.y, position.x] = 1;
                count++;
            }

            return room;
        }
        private bool IsInValidRange(Vector2Int position, int diameter)
        {
            double xDelta = position.x - center.x;
            double yDelta = position.y - center.y;
            return Math.Sqrt(xDelta * xDelta + yDelta * yDelta) < diameter / 2f;
        }
        private int GetCountOfCells(int[,] room, int diameter)
        {
            int countOfCells = 0;
            for (int i = 0; i < room.GetLength(0); i++)
            {
                for (int j = 0; j < room.GetLength(1); j++)
                {
                    if (IsInValidRange(new Vector2Int(j,i), diameter))
                    {
                        countOfCells++;
                    }
                }
            }
            return countOfCells;
        }
    }
}
