using System;
using Generator.PathFinders.Movements;
using UnityEngine;

namespace Generator.PathFinders.AStarAlgorithm
{
    public class Vertex
    {
        public Vertex Predecessor { get; set; }
        public bool IsLocked { get; set; }
        public bool IsGoal { get; set; }
        public bool IsStart { get; set; }
        public Vector3Int Position { get; set; }
        public IMovement Movement { private get; set; }
        public Vector3Int Direction { get; set; }
        public int MinDistance { get; set; }
        public Vertex(bool isLocked, Vector3Int position)
        {
            IsLocked = isLocked;
            Position = position;
            Predecessor = null;
            MinDistance = 0;
        }

        public Vector3Int MovementBuild(Dungeon dung)
        {
            var direction = Predecessor.Position - Position;
            return Movement.BuildIn(dung, Position, direction);
        }
    }
}