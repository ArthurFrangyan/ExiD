using System;
using UnityEngine;

namespace Assets.Scripts.Generator
{
    public static class Sphere
    {
        public static bool IsInValidRange(Vector3Int position, Vector3 center, int diameter)
        {
            float xDelta = position.x - center.x;
            float yDelta = position.z - center.z;
            return Math.Sqrt(xDelta * xDelta + yDelta * yDelta) < diameter / 2f;
        }
        public static Vector2Int GetMatrixStartPosition_XZ(Vector3 center, int diameter)
        {
            return new Vector2Int((int)Math.Round(center.x - (diameter - 1) / 2f), (int)Math.Round(center.z - (diameter - 1) / 2f));
        }
        public static (int i,int j) GetMatrixIndex_XZ(Vector3Int position3, Vector3 center, int diameter)
        {
            Vector2Int position2 = new Vector2Int(position3.x, position3.z);
            Vector2Int startPosition = GetMatrixStartPosition_XZ(center, diameter);
            return (position2.y - startPosition.y, position2.x - startPosition.x);
        }
    }
}
