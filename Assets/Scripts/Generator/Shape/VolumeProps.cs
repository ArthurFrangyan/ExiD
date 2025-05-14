using UnityEngine;

namespace Generator.Shape
{
    public class VolumeProps : IVolumeProps
    {
        public VolumeProps(int height, int maxCols, int minCols, int maxRows, int minRows, int maxDiameter, int minDiameter)
        {
            _maxCols = maxCols;
            _minCols = minCols;
            _maxDiameter = maxDiameter;
            _minDiameter = minDiameter;
            _maxRows = maxRows;
            _minRows = minRows;
            _height = height;
        }

        public int GetHeight() => _height;
        private readonly int _height;
        
        public int GetRows() => Random.Range(_minRows, _maxRows + 1);
        private readonly int _minRows;
        private readonly int _maxRows;
        
        public int GetCols() => Random.Range(_minCols, _maxCols + 1);
        private readonly int _minCols;
        private readonly int _maxCols;

        public int GetDiameter => Random.Range(_minDiameter, _maxDiameter + 1);
        private readonly int _minDiameter;
        private readonly int _maxDiameter;
    }
}