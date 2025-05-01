namespace Generator.Shape
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