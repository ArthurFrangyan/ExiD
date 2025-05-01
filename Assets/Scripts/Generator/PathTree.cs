using System.Collections.Generic;
using Generator.Library;

namespace Generator
{
    public class PathTree<TNode> where TNode : Node
    {
        public readonly List<Edge<TNode>> Edges = new();
        public readonly List<TNode> Nodes = new();

        public PathTree<Room> ToPathTreeRoom()
        {
            PathTree<Room> pathTree = new();
            foreach (Edge<TNode> edgeNode in Edges)
            {
                Room a = edgeNode.A as Room;
                Room b = edgeNode.B as Room;
                pathTree.Edges.Add(new Edge<Room>(a, b));
            }
            return pathTree;
        }
    }
}