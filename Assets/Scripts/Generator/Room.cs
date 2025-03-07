using Assets.Scripts.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Generator
{
    public class Room
    {
        public Vector3 center;
        public int Diameter { get; }
        public GameObject floor;
        public HashSet<Vector2Int> Cells;
        public HashSet<Room> roomAdjacency;
        public Room(int diameter)
        {
            Diameter = diameter;
            Cells = Info.RoomAreaGenerator.Generate(diameter);
        }
    }
}
