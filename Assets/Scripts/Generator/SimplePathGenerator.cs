using System;
using System.Collections;
using System.Collections.Generic;
using Generator.GraphAlgorithm;
using Generator.Library;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Generator
{
    public class SimplePathGenerator
    {
        private HashSet<Vector3Int> PathPositions { get; } = new();

        public HashSet<Vector3Int> GeneratePaths(PathTree<Node> pathTree)
        {
            foreach ((Node first, Node second) in pathTree.Edges)
            {
                GeneratePathBetween((Room)first, (Room)second, pathTree.Nodes);
            }

            return PathPositions;
        }
        public T ConvertTo<T>(RoomGraph2D roomGraph) where T : IList, new()
        {
            T nodes = new T();
            foreach (var rooms in roomGraph)
            {
                foreach (var room in rooms)
                {
                    nodes.Add(room);
                }
            }
            return nodes;
        }

        private void GeneratePathBetween(Room first, Room second, List<Node> nodes)
        {
            Vector3Int position = Vector3ToVector3Int(first.Center);
            Vector3 destinyPosition = second.Center;
            Vector3 destinyDirection = destinyPosition - position;
            Vector3Int direction = GetRandomDirection(destinyDirection);

            while (!IsInTheRoom(position, second))
            {
                if (CanAddPath(position, nodes))
                {
                    PathPositions.Add(position);
                }
                destinyDirection = destinyPosition - position;
                direction = GetRandomDirection(destinyDirection, direction);
                position += direction;
            }
        }
        private bool CanAddPath(Vector3Int position, List<Node> nodes)
        {
            if (IsInThePath(position))
            {
                return false;
            }
            foreach (Room room in nodes)
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
            (int z, int x )= Sphere.GetMatrixIndex_XZ(position, room.Center, room.Diameter);
            return room[x,z].HasFloor;
        }
        private bool IsInThePath(Vector3Int position)
        {
            return PathPositions.Contains(position);
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
            if (number < 0)
                return -1;
            return 0;
        }
    }
}
