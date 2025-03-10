using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Generator.Library
{
    public static class Direction2
    {
        private static List<Vector2Int> _directions = new List<Vector2Int>()
        {
            Vector2Int.down,
            Vector2Int.up,
            Vector2Int.left,
            Vector2Int.right,
        };

        public static Vector2Int GetRandomDirection()
        {
            return _directions[Random.Range(0, _directions.Count)];
        }
        public static Vector2Int GetRandomDirection(List<Vector2Int> directions)
        {
            return directions[Random.Range(0, directions.Count)];
        }
    }
}
