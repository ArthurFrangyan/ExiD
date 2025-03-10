using Assets.Scripts.Generator.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Generator
{
    public class RandomWalkAreaGenerator : IGenerator
    {
        private Vector2 center;
        private int countOfCells;
        public short[,] Generate(int diameter)
        {
            if (diameter<1)
            {
                throw new ArgumentException();
            }


            center = new Vector2((diameter - 1) / 2f, (diameter - 1) / 2f);
            var roomArea = new short[diameter, diameter];
            var roomAreaPaths = new HashSet<Vector2Int>();

            var position = new Vector2Int(diameter/2, diameter/2);

            var MaxSteps = GetCountOfCells(roomArea, diameter);
            var MinSteps = MaxSteps*2/3;
            var steps = Random.Range(MinSteps, MaxSteps);

            roomArea[position.y, position.x] = 1;
            roomAreaPaths.Add(position);
            countOfCells = 0;
            while(SimpleRandomWalk(position,roomAreaPaths, roomArea, steps, diameter));

            return roomArea;
        }
        private bool IsInValidRange(Vector2Int position, int diameter)
        {
            return Sphere.IsInValidRange(new Vector3Int(position.x, 0, position.y), new Vector3(center.x, 0, center.y), diameter);
        }
        private int GetCountOfCells(short[,] room, int diameter)
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
        public bool SimpleRandomWalk(Vector2Int position, HashSet<Vector2Int> roomAreaPaths, short[,] roomArea, int steps, int diameter)
        {

            while (countOfCells < steps)
            {
                Vector2Int direction = Direction2.GetRandomDirection();
                position += direction;
                if (!IsInValidRange(position, diameter))
                {
                    return true;
                }
                if (roomArea[position.y, position.x] == 1)
                {
                    continue;
                }
                roomArea[position.y, position.x] = 1;
                roomAreaPaths.Add(position);
                countOfCells++;
            }

            return false;
        }
    }
}
