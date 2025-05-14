using UnityEngine;

namespace Generator.Shape
{
    public class AreaProps : IAreaProps
    {
        public AreaProps(int cols, int minRows, int maxRows)
        {
            _cols = cols;
            _minRows = minRows;
            _maxRows = maxRows;
        }

        public int GetCols() => _cols;
        private readonly int _cols;

        public int GetRows() => Random.Range(_minRows, _maxRows + 1);
        private readonly int _minRows;
        private readonly int _maxRows;
    }
}