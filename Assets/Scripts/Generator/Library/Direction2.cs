using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Generator.Library
{
    public static class Direction2
    {
        private static readonly Vector2Int[] _directions = new Vector2Int[]
        {
            Vector2Int.down,
            Vector2Int.up,
            Vector2Int.left,
            Vector2Int.right,
        };

        public static Vector2Int GetRandomDirection()
        {
            return _directions[Random.Range(0, _directions.Length)];
        }
        public static Vector2Int GetRandomDirection(List<Vector2Int> directions)
        {
            return directions[Random.Range(0, directions.Count)];
        }
        public static Vector2Int GetRandomDirection(params Vector2Int[] directions)
        {
            return directions[Random.Range(0, directions.Length)];
        }
        public static Vector2Int GetRandomDirectionExcept(params Vector2Int[] excludedDirections)
        {
            List<Vector2Int> availableDirections = _directions.Except(excludedDirections).Cast<Vector2Int>().ToList();

            if (!availableDirections.Any())
                throw new ArgumentException("no available directions");

            return availableDirections.ElementAt(Random.Range(0, availableDirections.Count));
        }
    }
}
