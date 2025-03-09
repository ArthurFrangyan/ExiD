using Assets.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
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
            _roomGraph = new List<List<Room>>();
            for (ushort i = 0; i < _cols; i++)
            {
                GenerateNewLineOfRooms((ushort)Random.Range(_minRows, _maxRows + 1));
            }

            List<Room> previous = _roomGraph[0];
            for (ushort i = 1; i < _cols; i++)
            {
                List<Room> current = _roomGraph[i];

                MakeConnectionBetweenLines(previous, current);

                previous = current;
            }
            SetCenters(_roomGraph);
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
                degree[Random.Range(0, left)] += 1;
            }
            return degree;
        }
        private void GenerateNewLineOfRooms(ushort rows)
        {
            if (rows < 0)
            {
                throw new ArgumentException("'rows < 0' shouldn't be");
            }
            int col = _roomGraph.Count;
            _roomGraph.Add(new List<Room>());
            Room previous = new Room(Random.Range(_minDiameter, _maxDiameter + 1));
            _roomGraph[col].Add(previous);
            previous.Nodes = new HashSet<Room>();

            for (int j = 1; j < rows; j++)
            {
                Room current = new Room(Random.Range(_minDiameter, _maxDiameter + 1));
                _roomGraph[col].Add(current);

                previous.Nodes.Add(current);
                current.Nodes = new HashSet<Room>
                {
                    previous
                };

                previous = current;
            }
        }
        private void SetCenters(List<List<Room>> roomGraph)
        {
            if (roomGraph is null)
                throw new ArgumentException($"{nameof(roomGraph)} is null");

            int maxLineLength = MaxLineLength();

            int posCol = 0;
            SetCentersForLine(0, posCol, StartPositionForLine(0, maxLineLength));
            for (int col = 1; col < roomGraph.Count; col++)
            {
                posCol += CalculateDistanceBetweenRooms(
                    MaxDiameterInLine(roomGraph[col - 1]), 
                    MaxDiameterInLine(roomGraph[col]));

                SetCentersForLine(col, posCol, StartPositionForLine(col, maxLineLength));
            }

        }
        private void SetCentersForLine(int col, int posCol, int posRow = 0)
        {
            List<Room> roomLine = _roomGraph[col];
            roomLine[0].CenterInt = new Vector3Int(posCol, 0, posRow);
            for (int row = 1; row < roomLine.Count; row++)
            {
                posRow += CalculateDistanceBetweenRooms(roomLine[row - 1].Diameter, roomLine[row].Diameter);
                roomLine[row].CenterInt = new Vector3Int(posCol, 0, posRow);
            }
        }
        private int StartPositionForLine(int col, int maxLineLength)
        {
            return (int)Math.Ceiling((maxLineLength - SumDiametersOfRoomLine(col))/2f);
        }
        private int MaxLineLength()
        {

            List<int> lineLength = new List<int>();
            for (int col = 0; col < _roomGraph.Count; col++)
            {
                lineLength.Add(SumDiametersOfRoomLine(col));
            }

            return lineLength.Max();
        }

        private int CalculateDistanceBetweenRooms(int diameter_A, int diameter_B)
        {
            return (int)Math.Ceiling((diameter_A + diameter_B) / 2f) + 1;
        }
        private int SumDiametersOfRoomLine(int col)
        {
            List<Room> rooms = _roomGraph[col];
            int sum = 0;
            for (int i = 0; i < rooms.Count; i++)
            {
                sum += rooms[i].Diameter;
            }
            return sum;
        }

        private int MaxDiameterInLine(List<Room> rooms)
        {
            int Diameter = rooms[0].Diameter;
            for (int row = 1; row < rooms.Count; row++)
            {
                if (Diameter < rooms[row].Diameter)
                {
                    Diameter = rooms[row].Diameter;
                }
            }
            return Diameter;
        }
    }
}
