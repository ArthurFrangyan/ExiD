using Assets.Scripts.Data;
using Assets.Scripts.Generator.Library;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Generator
{
    class PathTreeGenerator
    {
        private HashSet<Vector3Int> _pathsPositions;

        public HashSet<Vector3Int> PathPositions
        {
            get { return _pathsPositions; }
            set { _pathsPositions = value; }
        }

        private HashSet<Node> _paths;
        private HashSet<Node> _nextNodes;
        private Dictionary<Node, HashSet<Node>> _possibleMovements;
        private List<(Node, Node)> combinations;
        public PathTreeGenerator(RoomGraph roomGraph)
        {
            combinations = new List<(Node, Node)>();
            _pathsPositions = new HashSet<Vector3Int>();
            _paths = new HashSet<Node>();
            GeneratePaths(roomGraph);
        }
        public void GeneratePaths(RoomGraph roomGraph)
        {
            GeneratePathTree(ConvertTo<List<Node>>(roomGraph));
            GeneratePathsForRooms();
        }
        #region PathTreeGeneration
        private void GeneratePathTree(List<Node> nodes)
        {
            _possibleMovements = MovePossibleMovementsToDictionary(nodes);

            GetRandomDisconnectedRoom(nodes);
            while (_paths.Count < nodes.Count)
            {
                ConnectNextRandomNodes();
            }
        }
        private void ConnectNextRandomNodes()
        {
            Node nextNode = GetRandomNodeFrom(_nextNodes);

            HashSet<Node> previousNodePossibleVariants = new HashSet<Node>(_possibleMovements[nextNode]);
            previousNodePossibleVariants.IntersectWith(_paths);

            Node previousNode = GetRandomNodeFrom(previousNodePossibleVariants);


            ConnectAndSet_nextNodes(previousNode, nextNode);
        }
        private void ConnectAndSet_nextNodes(Node previousNode, Node nextNode)
        {
            previousNode.Nodes.Add(nextNode);
            nextNode.Nodes.Add(previousNode);

            _paths.Add(nextNode);
            _nextNodes.UnionWith(_possibleMovements[nextNode]);
            _nextNodes.ExceptWith(_paths);

            combinations.Add((previousNode, nextNode));
        }
        private Dictionary<Node, HashSet<Node>> MovePossibleMovementsToDictionary(List<Node> nodes)
        {
            List<Node> pathNodes = new List<Node>();
            for (int i = 0; i < nodes.Count; i++)
            {
                pathNodes.Add(new Node());
                pathNodes[i].Nodes = nodes[i].Nodes;
            }
            foreach (Node node in nodes)
            {
                node.Nodes = new HashSet<Node>();
            }

            Dictionary<Node, HashSet<Node>> dictionaryNodes = new Dictionary<Node, HashSet<Node>>();
            for (int i = 0; i < nodes.Count; i++)
            {
                dictionaryNodes.Add(nodes[i], pathNodes[i].Nodes);
            }
            return dictionaryNodes;
        }
        private Node GetRandomNodeFrom<T>(T nodes) where T : ICollection<Node>
        {
            return nodes.ElementAt(Random.Range(0, nodes.Count));
        }
        private void GetRandomDisconnectedRoom(List<Node> nodes)
        {
            Node node = GetRandomNodeFrom(nodes);
            _nextNodes = new HashSet<Node>();
            _nextNodes.UnionWith(_possibleMovements[node]);
            _paths.Add(node);
        }
        public T ConvertTo<T>(RoomGraph roomGraph) where T : IList, new()
        {
            T nodes = new T();
            for (int i = 0; i < roomGraph.Count; i++)
            {
                for (int j = 0; j < roomGraph[i].Count; j++)
                {
                    nodes.Add(roomGraph[i][j]);
                }
            }
            return nodes;
        }
        #endregion
        #region PathPositionGeneration
        public void GeneratePathsForRooms()
        {
            foreach ((Node first, Node second) in combinations)
            {
                GeneratePathBetween((Room)first, (Room)second);
            }
        }
        private void GeneratePathBetween(Room first, Room second)
        {
            Vector3Int position = Vector3ToVector3Int(first.Center);
            Vector3 destinyPosition = second.Center;
            Vector3 destinyDirection = destinyPosition - position;
            Vector3Int direction = GetRandomDirection(destinyDirection);

            while (!IsInTheRoom(position, second))
            {
                if (CanAddPath(position))
                {
                    _pathsPositions.Add(position);
                }
                destinyDirection = destinyPosition - position;
                direction = GetRandomDirection(destinyDirection, direction);
                position += direction;
            }
        }
        private bool CanAddPath(Vector3Int position)
        {
            if (IsInThePath(position))
            {
                return false;
            }
            foreach (Room room in _paths)
            {
                if (IsInTheRoom(position, room))
                {
                    return false;
                }
            }
            return true;
        }
        private Vector3Int Vector3ToVector3Int(Vector3 vector)
        {
            return new Vector3Int((int)vector.x, (int)vector.y, (int)vector.z);
        }
        private bool IsInTheRoom(Vector3Int position, Room room)
        {
            if (!IsInValidRange(position, room.Center, room.Diameter))
            {
                return false;
            }
            (int i, int j )= Sphere.GetMatrixIndex_XZ(position, room.Center, room.Diameter);
            return room.Cells[i,j] != 0;
        }
        private bool IsInThePath(Vector3Int position)
        {
            return _pathsPositions.Contains(position);
        }
        private bool IsInValidRange(Vector3Int position, Vector3 center, int diameter)
        {
            return Sphere.IsInValidRange(position, center, diameter);
        }

        private Vector3Int GetRandomDirection(Vector3 destinyDirection)
        {
            List<Vector3Int> directions = GetVectorBases(destinyDirection);
            return directions[Random.Range(0, directions.Count)];

        }
        private Vector3Int GetRandomDirection(Vector3 destinyDirection, Vector3Int previousDirection)
        {
            List<Vector3Int> directions = GetVectorBases(destinyDirection);
            if (directions.Contains(previousDirection))
            {
                directions.Add(previousDirection);
            }

            if (directions.Count == 0)
            {
                throw new InvalidOperationException("No valid directions available.");
            }

            return directions[Random.Range(0, directions.Count)];

        }
        private List<Vector3Int> GetVectorBases(Vector3 vector)
        {
            List<Vector3Int> vectors =  new List<Vector3Int>();
            if (vector.x != 0)
            {
                vectors.Add(new Vector3Int(Identity(vector.x), 0, 0));
            }
            if (vector.y != 0)
            {
                vectors.Add(new Vector3Int(0, Identity(vector.y), 0));
            }
            if (vector.z != 0)
            {
                vectors.Add(new Vector3Int(0, 0, Identity(vector.z)));
            }
            return vectors;
        }
        private int Identity(float number)
        {
            if (number > 0)
                return 1;
            else if (number < 0)
                return -1;
            else
                return default;
        }
        #endregion
    }
}
