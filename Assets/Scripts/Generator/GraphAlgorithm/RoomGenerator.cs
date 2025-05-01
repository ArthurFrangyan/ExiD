using System.Collections.Generic;
using Generator.Shape;

namespace Generator.GraphAlgorithm
{

    public static class RoomGenerator
    {
        public static List<List<List<Room>>> NewVolume(IVolumeProps volumeProps, IRoomProps roomProps)
        {
            var rooms = new List<List<List<Room>>>();
            
            var height = volumeProps.Height;
            for (var i = 0; i < height; i++)
            {
                rooms.Add(NewArea(volumeProps, roomProps));
            }
            
            return rooms;
        }
        public static List<List<Room>> NewArea(IAreaProps areaProps, IRoomProps roomProps)
        {
            List<List<Room>> rooms = new List<List<Room>>();
            
            var cols = areaProps.Cols;
            for (var i = 0; i < cols; i++)
            {
                rooms.Add(NewLine(areaProps, roomProps));
            }

            return rooms;
        }

        private static List<Room> NewLine(ILineProps lineProps, IRoomProps roomProps)
        {
            List<Room> rooms = new List<Room>();
            
            var rows = lineProps.Rows;
            for (var j = 0; j < rows; j++)
            {
                rooms.Add(new Room(roomProps.Generator, roomProps.Diameter));
            }

            return rooms;
        }
    }
}