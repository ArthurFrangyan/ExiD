using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Generator;
using Generator.Library;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Generator
{
    public class RoomGraph3D : IList<List<List<Room>>>
    {
        private List<List<List<Room>>> _roomGraph;

        #region RoomGraph properties
        private readonly ushort _minRows;
        private readonly ushort _maxRows;
        private readonly ushort _minCols;
        private readonly ushort _maxCols;
        private readonly ushort _height;
        private readonly ushort _minRoomDiameter;
        private readonly ushort _maxRoomDiameter;
        private readonly ushort _minRoomHeight;
        private readonly ushort _maxRoomHeight;
        #endregion

        #region IList
        public int Count => ((ICollection<List<List<Room>>>)_roomGraph).Count;

        public bool IsReadOnly => ((ICollection<List<List<Room>>>)_roomGraph).IsReadOnly;

        public List<List<Room>> this[int index] { get => ((IList<List<List<Room>>>)_roomGraph)[index]; set => ((IList<List<List<Room>>>)_roomGraph)[index] = value; }
        #endregion
        #region ctors
        public RoomGraph3D(ushort height, ushort rows, ushort cols, ushort diameter, float colsCoefficient = 0.5f, float rowsCoefficient = 0.5f, float roomDiameterCoefficient = 0.5f, float roomHeightCoefficient = 0.5f)
        {
            if (rowsCoefficient > 1)
                throw new ArgumentException($"shouldn't be '{nameof(rowsCoefficient)} > 1'");
            if (height <= 0)
                throw new ArgumentException($"shouldn't be '{nameof(height)} < 0'");

            _minRows = (ushort)(rows - rows * rowsCoefficient);
            _maxRows = (ushort)(rows + rows * rowsCoefficient);
            _minCols = (ushort)(rows - rows * rowsCoefficient);
            _maxCols = (ushort)(rows + rows * rowsCoefficient);
            _height = height;
            _minRoomDiameter = (ushort)(diameter - diameter * roomDiameterCoefficient);
            _maxRoomDiameter = (ushort)(diameter + diameter * roomDiameterCoefficient);
            Generate();
        }
        public RoomGraph3D(ushort height, ushort minRows, ushort maxRows, ushort minCols, ushort maxCols, ushort minRoomDiameter, ushort maxRoomDiameter, ushort minRoomHeight, ushort maxRoomHeight)
        {
            _minRows = minRows;
            _maxRows = maxRows;
            _minCols = minCols;
            _maxCols = maxCols;
            _height = height;
            _minRoomDiameter = minRoomDiameter;
            _maxRoomDiameter = maxRoomDiameter;
            _minRoomHeight = minRoomHeight;
            _maxRoomHeight = maxRoomHeight;
            Generate();
        }
        #endregion
        #region calculate props
        private ushort CalculateRows()
        {
            return (ushort)Random.Range(_minRows, _maxRows + 1);
        }
        private ushort CalculateCols()
        {
            return (ushort)Random.Range(_minCols, _maxCols + 1);
        }
        private ushort CalculateHeight()
        {
            return _height;
        }
        private ushort CalculateRoomDiameter()
        {
            return (ushort)Random.Range(_minRoomDiameter, _maxRoomDiameter + 1);
        }
        private ushort CalculateRoomHeight()
        {
            return (ushort)Random.Range(_minRoomHeight, _maxRoomHeight + 1);
        }
        #endregion
        public void Generate()
        {
            _roomGraph = GenerateRooms(CalculateHeight(), CalculateCols, CalculateRows, CalculateRoomDiameter);
            MakeConnections(_roomGraph);
            SetCenters(_roomGraph);
            ForceAlgorithm();
        }
        #region GenerateRooms
        private List<List<List<Room>>> GenerateRooms(ushort height, Func<ushort> getRows, Func<ushort> getCols, Func<ushort> getDiameter)
        {
            List<List<List<Room>>> rooms = new List<List<List<Room>>>();

            for (ushort i = 0; i < height; i++)
            {
                rooms.Add(GenerateNewPlaneOfRooms(getRows(), getCols, getDiameter));
            }
            return rooms;
        }
        private List<List<Room>> GenerateNewPlaneOfRooms(ushort rows, Func<ushort> getCols, Func<ushort> getDiameter)
        {
            List<List<Room>> roomPlane = new List<List<Room>>();
            for (ushort i = 0; i < rows; i++)
            {
                roomPlane.Add(GenerateNewLineOfRooms(getCols, getDiameter));
            }
            return roomPlane;
        }
        private List<Room> GenerateNewLineOfRooms(Func<ushort> getCols, Func<ushort> getDiameter)
        {
            ushort cols = getCols();
            if (cols < 0)
            {
                throw new ArgumentException("'cols < 0' shouldn't be");
            }
            List<Room> roomLine = new List<Room>();
            Room previous = new Room(new RandomWalkAreaGenerator(), getDiameter());
            roomLine.Add(previous);
            previous.Nodes = new HashSet<Node>();

            for (int j = 1; j < cols; j++)
            {
                Room current = new Room(new RandomWalkAreaGenerator(), getDiameter());
                roomLine.Add(current);

                previous.Nodes.Add(current);
                current.Nodes = new HashSet<Node>
                {
                    previous
                };

                previous = current;
            }
            return roomLine;
        }
        #endregion
        #region ConnectionsBetweenRooms
        private void MakeConnections(List<List<List<Room>>> rooms)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                MakeConnectionInPlane(rooms[i]);
            }
            for (int i = 0; i < rooms.Count-1; i++)
            {
                MakeConnectionBetweenPlanes(rooms[i], rooms[i + 1]);
            }
        }
        private void MakeConnectionBetweenPlanes(List<List<Room>> buttom, List<List<Room>> top)
        {
            if (buttom.Count > top.Count)
                (buttom, top) = (top, buttom);

            int[] degree = FindRandomDegree(buttom.Count, top.Count);

            for (int i = 0, j = 0; i < buttom.Count; i++)
            {
                int k = j + degree[i];
                for (; j < k; j++)
                {
                    MakeConnectionBetweenLines(buttom[i], top[i]);
                }
            }
        }
        private void MakeConnectionInPlane(List<List<Room>> roomPlane)
        {
            List<Room> previous = roomPlane[0];
            for (ushort i = 1; i < roomPlane.Count; i++)
            {
                List<Room> current = roomPlane[i];

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
                degree[Random.Range(0, left)] += 1;
            }
            return degree;
        }
        #endregion
        #region SetCenters/Position
        private void SetCenters(List<List<List<Room>>> rooms)
        {
            if (rooms is null)
                throw new ArgumentException($"{nameof(rooms)} is null");

            int posHeight = 0;
            SetCentersForRoomPlane(rooms[0], posHeight);
            for (int col = 1; col < rooms.Count; col++)
            {
                posHeight += CalculateDistanceBetweenRooms(
                    MaxDiameterInPlane(rooms[col - 1]),
                    MaxDiameterInPlane(rooms[col]));

                SetCentersForRoomPlane(rooms[col], posHeight);
            }
        }
        private void SetCentersForRoomPlane(List<List<Room>> roomsPlane, int posHeight)
        {
            if (roomsPlane is null)
                throw new ArgumentException($"{nameof(roomsPlane)} is null");

            int maxLineLength = MaxLineLength(roomsPlane);

            int posCol = 0;
            SetCentersForLine(roomsPlane[0], posCol, posHeight, StartPositionForLine(roomsPlane[0], maxLineLength));
            for (int col = 1; col < roomsPlane.Count; col++)
            {
                posCol += CalculateDistanceBetweenRooms(
                    MaxDiameterInLine(roomsPlane[col - 1]),
                    MaxDiameterInLine(roomsPlane[col]));

                SetCentersForLine(roomsPlane[col], posCol, posHeight, StartPositionForLine(roomsPlane[col], maxLineLength));
            }
        }
        private void SetCentersForLine(List<Room> roomLine, int posCol, int posHeight, int posRow = 0)
        {
            roomLine[0].CenterInt = new Vector3Int(posCol, posHeight, posRow);
            for (int row = 1; row < roomLine.Count; row++)
            {
                posRow += CalculateDistanceBetweenRooms(roomLine[row - 1].Diameter, roomLine[row].Diameter);
                roomLine[row].CenterInt = new Vector3Int(posCol, posHeight, posRow);
            }
        }
        private int StartPositionForLine(List<Room> roomsLine, int maxLineLength)
        {
            return (int)Math.Ceiling((maxLineLength - SumDiametersOfRoomLine(roomsLine)) / 2f);
        }
        private int MaxLineLength(List<List<Room>> roomsPlane)
        {
            List<int> lineLength = new List<int>();
            for (int col = 0; col < roomsPlane.Count; col++)
            {
                lineLength.Add(SumDiametersOfRoomLine(roomsPlane[col]));
            }

            return lineLength.Max();
        }

        private int CalculateDistanceBetweenRooms(int diameter_A, int diameter_B)
        {
            return (int)Math.Ceiling((diameter_A + diameter_B) / 2f) + 1;
        }
        private int SumDiametersOfRoomLine(List<Room> roomsLine)
        {
            int sum = 0;
            for (int i = 0; i < roomsLine.Count; i++)
            {
                sum += roomsLine[i].Diameter;
            }
            return sum;
        }
        private int MaxDiameterInPlane(List<List<Room>> rooms)
        {
            int diameter = MaxDiameterInLine(rooms[0]);
            for (int i = 1; i < rooms.Count; i++)
            {
                MaxDiameterInLine(rooms[i]);
            }
            return diameter;
        }
        private int MaxDiameterInLine(List<Room> rooms)
        {
            int diameter = rooms[0].Diameter;
            for (int row = 1; row < rooms.Count; row++)
            {
                if (diameter < rooms[row].Diameter)
                {
                    diameter = rooms[row].Diameter;
                }
            }
            return diameter;
        }
        #endregion

        private void ForceAlgorithm()
        {

        }
        #region IList
        public int IndexOf(List<List<Room>> item)
        {
            return ((IList<List<List<Room>>>)_roomGraph).IndexOf(item);
        }

        public void Insert(int index, List<List<Room>> item)
        {
            ((IList<List<List<Room>>>)_roomGraph).Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            ((IList<List<List<Room>>>)_roomGraph).RemoveAt(index);
        }

        public void Add(List<List<Room>> item)
        {
            ((ICollection<List<List<Room>>>)_roomGraph).Add(item);
        }

        public void Clear()
        {
            ((ICollection<List<List<Room>>>)_roomGraph).Clear();
        }

        public bool Contains(List<List<Room>> item)
        {
            return ((ICollection<List<List<Room>>>)_roomGraph).Contains(item);
        }

        public void CopyTo(List<List<Room>>[] array, int arrayIndex)
        {
            ((ICollection<List<List<Room>>>)_roomGraph).CopyTo(array, arrayIndex);
        }

        public bool Remove(List<List<Room>> item)
        {
            return ((ICollection<List<List<Room>>>)_roomGraph).Remove(item);
        }

        public IEnumerator<List<List<Room>>> GetEnumerator()
        {
            return ((IEnumerable<List<List<Room>>>)_roomGraph).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_roomGraph).GetEnumerator();
        }
        #endregion
    }
}
