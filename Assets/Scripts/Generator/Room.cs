using Assets.Scripts.Data;
using Assets.Scripts.Generator.Library;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Generator
{
    public class Room : Node
    {
        public Vector3 center;
        public int Diameter { get; }
        public GameObject floor;
        public short[,] Cells;
        public Room(int diameter)
        {
            Diameter = diameter;
            Cells = Info.RoomAreaGenerator.Generate(diameter);
        }
    }
}
