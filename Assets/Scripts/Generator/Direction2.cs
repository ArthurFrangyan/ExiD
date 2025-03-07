using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Generator
{
    public static class Direction2
    {
        private static List<Vector2Int> Directions = new List<Vector2Int>()
        {
            Vector2Int.down,
            Vector2Int.up,
            Vector2Int.left,
            Vector2Int.right,
        };

        public static Vector2Int GetRandomDirection()
        {
            return Directions[Random.Range(0, Directions.Count)];
        }
    }
}
