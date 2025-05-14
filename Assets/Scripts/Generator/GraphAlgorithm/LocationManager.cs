using System;
using System.Collections.Generic;
using System.Linq;

namespace Generator.GraphAlgorithm
{
    public class LocationManager
    {
        public static void SetCenters(List<List<Room>> rooms)
        {
            int maxLineLength = MaxLineLength(rooms[0]);

            int posCol = 0;
            SetCentersForLine(posCol, rooms[0], StartPositionForLine(maxLineLength, rooms[0]));
            for (int col = 1; col < rooms.Count; col++)
            {
                posCol += CalculateDistanceBetweenRooms(
                    MaxDiameterInLine(rooms[col - 1]), 
                    MaxDiameterInLine(rooms[col]));

                SetCentersForLine(posCol, rooms[col], StartPositionForLine(maxLineLength, rooms[col]));
            }

        }

        private static void SetCentersForLine(int posCol, List<Room> rooms, int posRow = 0)
        {
            List<Room> roomLine = rooms;
            roomLine[0].CenterInt = new UnityEngine.Vector3Int(posCol, 0, posRow);
            for (int row = 1; row < roomLine.Count; row++)
            {
                posRow += CalculateDistanceBetweenRooms(roomLine[row - 1].Diameter, roomLine[row].Diameter);
                roomLine[row].CenterInt = new UnityEngine.Vector3Int(posCol, 0, posRow);
            }
        }

        private static int StartPositionForLine(int maxLineLength, List<Room> roomLine)
        {
            return (int)Math.Ceiling((maxLineLength - SumDiametersOfRoomLine(roomLine))/2f);
        }

        private static int MaxLineLength(List<Room> roomLine)
        {

            List<int> lineLength = new List<int>();
            for (int col = 0; col < roomLine.Count; col++)
            {
                lineLength.Add(SumDiametersOfRoomLine(roomLine));
            }

            return lineLength.Max();
        }

        private static int CalculateDistanceBetweenRooms(int diameterA, int diameterB)
        {
            return (int)Math.Ceiling((diameterA + diameterB) / 2f) + 1;
        }

        private static int SumDiametersOfRoomLine(List<Room> roomLine)
        {
            int sum = 0;
            foreach (var room in roomLine)
            {
                sum += room.Diameter;
            }
            return sum;
        }

        private static int MaxDiameterInLine(List<Room> rooms)
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
    }
}