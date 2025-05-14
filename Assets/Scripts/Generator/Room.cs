using Assets.Scripts.Data;
using Generator.Library;
using UnityEngine;

namespace Generator
{
    public class Room : Node, ICircle
    {
        public Vector3Int Position { get; set; }
        public Vector3Int Size => new Vector3Int(Diameter, Height, Diameter);

        public Vector3 Center => Position + new Vector3((Diameter - 1) / 2f, 0, (Diameter - 1) / 2f);

        public UnityEngine.Vector3Int CenterInt
        {
            get => Position + new UnityEngine.Vector3Int((Diameter - 1) / 2, 0, (Diameter - 1) / 2);
            set => Position = value - new UnityEngine.Vector3Int((Diameter - 1) / 2, 0, (Diameter - 1) / 2);
        }
        public int Diameter { get; }
        public int Height { get; }

        private readonly Block[,] _blocks;
        public Block this[Vector3Int pos] => _blocks[pos.z, pos.x];
        public Block this[int x, int y, int z] => _blocks[z, x];
        public Block this[int x, int z] => _blocks[z, x];

        public Room(IAreaGenerator areaGenerator, int diameter, int height = 1)
        {
            Diameter = diameter;
            Height = height;
            _blocks = areaGenerator.Generate(diameter);
        }

        public bool InBoundary(Vector3Int pos)
        {
            return pos.x >= 0 && pos.x < _blocks.GetLength(1) &&
                   pos.z >= 0 && pos.z < _blocks.GetLength(0);
        }
    }
}
