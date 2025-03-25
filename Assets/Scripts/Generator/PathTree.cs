using System.Collections.Generic;
using Generator.Library;

namespace Generator
{
    public class PathTree
    {
        public readonly List<(Node, Node)> Combinations = new();
        public readonly HashSet<Node> Nodes = new();
    }
}