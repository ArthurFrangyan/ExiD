using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Generator;
using Generator.Library;
using UnityEngine;

namespace Generator
{

    public class TreeGenerator
    {
        private Dictionary<Node, HashSet<Node>> _possibleMovements;
        private HashSet<Node> _nextNodes;
        private readonly PathTree<Node> _pathTree;

        public TreeGenerator()
        {
            _possibleMovements = new Dictionary<Node, HashSet<Node>>();
            _nextNodes = new HashSet<Node>();
            _pathTree = new PathTree<Node>();
        }
        public PathTree<Node> GenerateTree(List<Node> nodes)
        {
            _possibleMovements = MovePossibleMovementsToDictionary(nodes);

            GetRandomDisconnectedRoom(nodes);
            while (_pathTree.Nodes.Count < nodes.Count)
            {
                ConnectNextRandomNodes();
            }

            return _pathTree;
        }

        private void ConnectNextRandomNodes()
        {
            var nextNode = GetRandomNodeFrom(_nextNodes);

            var previousNodePossibleOptions = new HashSet<Node>(_possibleMovements[nextNode]);
            previousNodePossibleOptions.IntersectWith(_pathTree.Nodes);

            var previousNode = GetRandomNodeFrom(previousNodePossibleOptions);
            
            Connect(previousNode, nextNode);
        }

        private void Connect(Node previousNode, Node nextNode)
        {
            previousNode.Nodes.Add(nextNode);
            nextNode.Nodes.Add(previousNode);

            _pathTree.Nodes.Add(nextNode);
            _nextNodes.UnionWith(_possibleMovements[nextNode]);
            _nextNodes.ExceptWith(_pathTree.Nodes);

            _pathTree.Edges.Add(new Edge<Node>(previousNode, nextNode));
        }

        private Dictionary<Node, HashSet<Node>> MovePossibleMovementsToDictionary(List<Node> nodes)
        {
            List<Node> pathNodes = new List<Node>();
            for (int i = 0; i < nodes.Count; i++)
            {
                pathNodes.Add(new Node(nodes[i].Nodes));;
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

        private void GetRandomDisconnectedRoom(ICollection<Node> nodes)
        {
            Node node = GetRandomNodeFrom(nodes);
            _nextNodes = new HashSet<Node>();
            _nextNodes.UnionWith(_possibleMovements[node]);
            _pathTree.Nodes.Add(node);
        }

        private Node GetRandomNodeFrom<T>(T nodes) where T : ICollection<Node>
        {
            return nodes.ElementAt(Random.Range(0, nodes.Count));
        }
    }
}