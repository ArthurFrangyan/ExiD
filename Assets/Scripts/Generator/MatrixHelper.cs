using Assets.Scripts.Generator;
using UnityEngine;

#nullable disable
namespace Generator
{
    public class MatrixHelper
    {
        public readonly Vector2 Center;
        public readonly int Diameter;

        public MatrixHelper(int diameter)
        {
            Center = new Vector2((diameter - 1) / 2f, (diameter - 1) / 2f);
            Diameter = diameter;
        }

        public bool IsInValidRange(Vector2Int position)
        {
            return Sphere.IsInValidRange(new UnityEngine.Vector3Int(position.x, 0, position.y), new Vector3(this.Center.x, 0.0f, this.Center.y), Diameter);
        }

        public int GetCountOfCells<T>(T[,] room)
        {
            var countOfCells = 0;
            for (var y = 0; y < room.GetLength(0); ++y)
            {
                for (var x = 0; x < room.GetLength(1); ++x)
                {
                    if (this.IsInValidRange(new Vector2Int(x, y)))
                        ++countOfCells;
                }
            }
            return countOfCells;
        }
    }
}