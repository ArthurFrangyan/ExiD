using Assets.Scripts.Data;
using Assets.Scripts.Generator.Library;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Generator
{
    public class Room : Node
    {
        private Vector3Int position;
        public Vector3Int Position { get => position; set => position = value; }
        public Vector3 Center
        {
            get => position + new Vector3((Diameter - 1) / 2f, 0, (Diameter - 1) / 2f);
        }
        public Vector3Int CenterInt
        {
            get => position + new Vector3Int((Diameter - 1) / 2, 0, (Diameter - 1) / 2);
            set => position = value - new Vector3Int((Diameter - 1) / 2, 0, (Diameter - 1) / 2);
        }
        public int Diameter { get; set; }
        public int DiameterStandard { get; set; }

        public short[,] Cells;

        public Room(int diameter)
        {
            Diameter = diameter;
            Cells = Info.RoomAreaGenerator.Generate(diameter);
        }
    }
}
