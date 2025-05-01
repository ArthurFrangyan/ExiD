using Assets.Scripts.Data;
using Generator.Library;
using UnityEngine;

namespace Generator
{
    public class Room : Node, ICircle
    {
        public Vector3Int Position { get; set; }

        public Vector3 Center => Position + new Vector3((Diameter - 1) / 2f, 0, (Diameter - 1) / 2f);

        public UnityEngine.Vector3Int CenterInt
        {
            get => Position + new UnityEngine.Vector3Int((Diameter - 1) / 2, 0, (Diameter - 1) / 2);
            set => Position = value - new UnityEngine.Vector3Int((Diameter - 1) / 2, 0, (Diameter - 1) / 2);
        }
        public int Diameter { get; }
        public int Height { get; }

        public readonly Block[,] Blocks;

        public Room(IAreaGenerator areaGenerator, int diameter, int height = 1)
            : base()
        {
            Diameter = diameter;
            Height = height;
            Blocks = areaGenerator.Generate(diameter);
        }
    }
}
