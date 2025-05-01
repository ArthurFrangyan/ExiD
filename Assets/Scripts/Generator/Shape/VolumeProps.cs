using UnityEngine;

namespace Generator.Shape
{
    public class VolumeProps : IVolumeProps
    {
        public VolumeProps(int height, int maxCols, int minCols, int maxRows, int minRows, int maxDiameter, int minDiameter)
        {
            MaxCols = maxCols;
            MinCols = minCols;
            MaxDiameter = maxDiameter;
            MinDiameter = minDiameter;
            MaxRows = maxRows;
            MinRows = minRows;
            Height = height;
        }

        public int Height { get; }
        
        private int MinRows { get; }
        private int MaxRows { get; }
        public int Rows => Random.Range(MinRows, MaxRows + 1);
        
        private int MinCols { get; }
        private int MaxCols { get; }
        public int Cols => Random.Range(MinCols, MaxCols + 1);

        private int MinDiameter { get; }
        private int MaxDiameter { get; }
        public int Diameter => Random.Range(MinDiameter, MaxDiameter + 1);
    }
}