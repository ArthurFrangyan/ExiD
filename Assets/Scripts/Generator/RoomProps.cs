using Generator.Shape;
using UnityEngine;

namespace Generator
{
    public class RoomProps : IRoomProps
    {
        public RoomProps(int minDiameter, int maxDiameter, IAreaGenerator areaGenerator)
        {
            MinDiameter = minDiameter;
            MaxDiameter = maxDiameter;
            Generator = areaGenerator;
        }
        private int MinDiameter { get; }
        private int MaxDiameter { get; }
        public int Diameter => Random.Range(MinDiameter, MaxDiameter);
        public IAreaGenerator Generator { get; }
    }
}