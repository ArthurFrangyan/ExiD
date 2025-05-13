using Generator.PathFinders.AStarAlgorithm;
using Unity.Mathematics;
using UnityEngine;

namespace Generator.PathFinders
{
    public static class Math
    {
        public static int ManhattanDistance(Vertex a, Vertex b)
        {
            return ManhattanDistance(a.Position - b.Position);
        }

        public static int ManhattanDistance(Vector3Int a, Vector3Int b)
        {
            return ManhattanDistance(a - b);
        }

        public static int ManhattanDistance(Vector3Int vector)
        {
            return math.abs(vector.x) + math.abs(vector.y) + math.abs(vector.z);
        }
    }
}