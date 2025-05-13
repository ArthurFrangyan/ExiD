using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Generator.Library;
using Generator.Shape;

namespace Generator.GraphAlgorithm.RoomGraph3D
{
    public class RoomGraph3D : IList<List<List<Room>>>
    {
        private List<List<List<Room>>> _roomGraph;

        public int Count => ((ICollection<List<List<Room>>>)_roomGraph).Count;

        public bool IsReadOnly => ((ICollection<List<List<Room>>>)_roomGraph).IsReadOnly;

        public List<List<Room>> this[int index] { get => ((IList<List<List<Room>>>)_roomGraph)[index]; set => ((IList<List<List<Room>>>)_roomGraph)[index] = value; }
        
        public RoomGraph3D(VolumeProps volumeProps, RoomProps roomProps)
        {
            _roomGraph = new List<List<List<Room>>>();
            Generate(volumeProps, roomProps);
        }
        
        public void Generate(VolumeProps areaProps, RoomProps roomProps)
        {
            _roomGraph = RoomGenerator.NewVolume(areaProps, roomProps);
            NodeConnector<Room>.ConnectVolume(_roomGraph);
            SetCenters(_roomGraph);
            ForceAlgorithm();
        }
        #region SetCenters/Position
        private void SetCenters(List<List<List<Room>>> rooms)
        {
            if (rooms is null)
                throw new ArgumentException($"{nameof(rooms)} is null");

            int posHeight = 0;
            SetCentersForRoomPlane(rooms[0], posHeight);
            for (int col = 1; col < rooms.Count; col++)
            {
                // posHeight += CalculateDistanceBetweenRooms(
                //     MaxDiameterInPlane(rooms[col - 1]),
                //     MaxDiameterInPlane(rooms[col]));

                posHeight += 1;
                
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
            roomLine[0].CenterInt = new UnityEngine.Vector3Int(posCol, posHeight, posRow);
            for (int row = 1; row < roomLine.Count; row++)
            {
                posRow += CalculateDistanceBetweenRooms(roomLine[row - 1].Diameter, roomLine[row].Diameter);
                roomLine[row].CenterInt = new UnityEngine.Vector3Int(posCol, posHeight, posRow);
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
        public List<Room> ConvertList()
        {
            List<Room> rooms = new List<Room>();
            foreach (var roomArea in _roomGraph)
            {
                foreach (var roomList in roomArea)
                    rooms.AddRange(roomList);
            }
            return rooms;
        }

        public List<Node> ConvertListNodes()
        {
            List<Node> rooms = new List<Node>();
            foreach (var roomArea in _roomGraph)
            {
                foreach (var roomList in roomArea)
                    rooms.AddRange(roomList);
            }
            return rooms;
        }

        public List<Room> ConvertListRoom()
        {
            List<Room> rooms = new List<Room>();
            foreach (var roomArea in _roomGraph)
            {
                foreach (var roomList in roomArea)
                    rooms.AddRange(roomList);
            }
            return rooms;
        }
    }
}
