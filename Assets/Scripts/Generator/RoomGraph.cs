using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Generator
{
    public class RoomGraph
    {
        public List<List<Room>> _roomGraph;
        private ushort _minRows;
        private ushort _maxRows;
        private ushort _cols;
        private ushort _minDiameter;
        private ushort _maxDiameter;

        public RoomGraph(ushort rows, ushort cols, ushort diameter, float diameterCoefficient = 0.5f, float rowsCoefficient = 0.5f)
        {
            if (rowsCoefficient > 1)
                throw new ArgumentException("shouldn't be 'rowsCoefficient > 1'");
            if (cols <= 0)
                throw new ArgumentException("shouln't be 'cols < 0'");


            _minRows = (ushort) (rows - rows * rowsCoefficient);
            _maxRows = (ushort) (rows + rows * rowsCoefficient);
            _cols = cols;
            _minDiameter = (ushort) (diameter - diameter * diameterCoefficient);
            _maxDiameter = (ushort) (diameter + diameter * diameterCoefficient);
            Generate();
        }
        public RoomGraph(ushort minRows, ushort maxRows, ushort cols, ushort minDiameter, ushort maxDiameter)
        {
            _minRows = minRows;
            _maxRows = maxRows;
            _cols = cols;
            _minDiameter = minDiameter;
            _maxDiameter = maxDiameter;
            Generate();
        }
        public void Generate()
        {
            for (ushort i = 0; i < _cols; i++)
            {
                GenerateLine((ushort)Random.Range(_minRows, _maxRows), i);
            }

            List<Room> previous = _roomGraph[0];
            for (ushort i = 0; i < _cols; i++)
            {
                List<Room> current = _roomGraph[i];

                MakeConnectionBetweenLines(previous, current);

                previous = current;
            }
        }
        private void MakeConnectionBetweenLines(List<Room> left, List<Room> right)
        {
            if (left.Count > right.Count)
                (left, right) = (right, left);

            int[] degree = FindRandomDegree(left.Count, right.Count);

            for (int i = 0, j = 0; i < left.Count; i++)
            {
                int k = j + degree[i];
                for (; j < k; j++)
                {
                    left[i].Nodes.Add(right[j]);
                    right[j].Nodes.Add(left[i]);
                }
            }
        }
        private int[] FindRandomDegree(int left, int right)
        {
            if (left > right)
                (left, right) = (right, left);

            int[] degree = new int[left];
            for (int i = 0; i < left; i++)
            {
                degree[i] = 1;
            }
            int Delta = right - left;
            for (int i = 0; i < Delta; i++)
            {
                degree[Random.Range(0, Delta)] += 1;
            }
            return degree;
        }
        private void GenerateLine(ushort rows, ushort col)
        {
            if (rows < 0)
            {
                throw new ArgumentException("'rows < 0' shouldn't be");
            }

            Room previous = _roomGraph[col][0] = new Room(Random.Range(_minDiameter, _maxDiameter));

            for (int j = 1; j < rows; j++)
            {
                Room current = _roomGraph[col][j] = new Room(Random.Range(_minDiameter, _maxDiameter));

                previous.Nodes.Add(current);
                current.Nodes.Add(previous);

                previous = current;
            }
        }
    }
}
