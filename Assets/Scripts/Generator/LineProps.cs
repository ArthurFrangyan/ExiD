using UnityEngine;

namespace Generator
{
    public class LineProps : ILineProps
    {
        public LineProps(int rows)
        {
            Rows = rows;
        }
        public int Rows { get; }
    }
}