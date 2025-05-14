namespace Generator.Shape
{
    public class LineProps : ILineProps
    {
        public LineProps(int rows)
        {
            _rows = rows;
        }

        private readonly int _rows;
        public int GetRows() => _rows;
    }
}