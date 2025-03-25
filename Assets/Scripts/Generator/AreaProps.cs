using UnityEngine;

namespace Generator
{
    public class AreaProps : IAreaProps
    {
        public AreaProps(int cols, int minRows, int maxRows)
        {
            Cols = cols;
            MinRows = minRows;
            MaxRows = maxRows;
        }

        public int Cols { get; }
        
        private int MinRows { get; }
        private int MaxRows { get; }
        
        public int Rows => Random.Range(MinRows, MaxRows + 1);
    }
}