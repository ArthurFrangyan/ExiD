using System.Collections.Generic;

namespace Generator.Library
{
    public class Node
    {
        public HashSet<Node> Nodes;

        public Node()
        {
            Nodes = new HashSet<Node>();
        }

        public Node(HashSet<Node> nodes)
        {
            Nodes = nodes;
        }
    }
}
