using System.Collections;
using System.Collections.Generic;
using Generator.Library;
using Generator.Shape;

namespace Generator.GraphAlgorithm
{
    public class RoomGraph2D : IList<List<Room>>
    {
        private List<List<Room>> _roomGraph;

        public int Count => _roomGraph.Count;

        public bool IsReadOnly => false;

        public List<Room> this[int index] { get => _roomGraph[index]; set => _roomGraph[index] = value; }
        public RoomGraph2D(AreaProps areaProps, RoomProps roomProps)
        {
            _roomGraph = new List<List<Room>>();
            Generate(areaProps, roomProps);
        }
        public void Generate(AreaProps areaProps, RoomProps roomProps)
        {
            _roomGraph = RoomGenerator.NewArea(areaProps, roomProps);
            NodeConnector<Room>.ConnectArea(_roomGraph);
            LocationManager.SetCenters(_roomGraph);
        }

        public List<Room> ConvertListRoom()
        {
            List<Room> rooms = new List<Room>();
            foreach (var roomList in _roomGraph)
            {
                rooms.AddRange(roomList);
            }
            return rooms;
        }
        public List<Node> ConvertListNodes()
        {
            List<Node> rooms = new List<Node>();
            foreach (var roomList in _roomGraph)
            {
                rooms.AddRange(roomList);
            }
            return rooms;
        }
        
        
        #region IList
        public void Add(List<Room> item)
        {
            _roomGraph.Add(item);
        }

        public void Clear()
        {
            _roomGraph.Clear();
        }

        public bool Contains(List<Room> item)
        {
            return _roomGraph.Contains(item);
        }

        public void CopyTo(List<Room>[] array, int arrayIndex)
        {
            _roomGraph.CopyTo(array, arrayIndex);
        }

        public bool Remove(List<Room> item)
        {
            return _roomGraph.Remove(item);
        }

        public IEnumerator<List<Room>> GetEnumerator()
        {
            return _roomGraph.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(List<Room> item)
        {
            return _roomGraph.IndexOf(item);
        }

        public void Insert(int index, List<Room> item)
        {
            _roomGraph.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _roomGraph.RemoveAt(index);
        }
        #endregion

        
    }
}
