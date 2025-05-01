using UnityEngine;

namespace Generator.PathFinder.AStarAlgorithm
{
    public class Vertex
    {
        public Vertex Predecessor { get; set; }
        public bool IsLocked { get; set; }
        public Vector3Int Position { get; set; }
        public int MinDistance { get; set; }
        public Vertex(bool isLocked, Vector3Int position)
        {
            IsLocked = isLocked;
            Position = position;
            Predecessor = null;
            MinDistance = 0;
        }
    }
}