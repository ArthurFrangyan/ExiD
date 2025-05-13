using System.Collections.Generic;
using Generator.Library;
using UnityEngine;

namespace Generator
{
    public static class NodeConnector<TNode> where TNode : Node
    {
        public static void ConnectVolume(List<List<List<TNode>>> rooms)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                ConnectArea(rooms[i]);
            }
            for (int i = 0; i < rooms.Count-1; i++)
            {
                ConnectBetweenPlanes(rooms[i], rooms[i + 1]);
            }
        }
        private static void ConnectBetweenPlanes(List<List<TNode>> button, List<List<TNode>> top)
        {
            if (button.Count > top.Count)
                (button, top) = (top, button);

            int[] degree = FindRandomDegree(button.Count, top.Count);

            for (int i = 0, j = 0; i < button.Count; i++)
            {
                int k = j + degree[i];
                for (; j < k; j++)
                {
                    ConnectBetweenLines(button[i], top[i]);
                }
            }
        }

        public static void ConnectArea(IList<List<TNode>> nodes)
        {
            if (nodes.Count == 0)
                return;
            
            for (int i = 0; i < nodes.Count - 1; i++)
            {
                ConnectBetweenLines(nodes[i], nodes[i + 1]);
                ConnectInLine(nodes[i]);
            }
            ConnectInLine(nodes[^1]);
        }

        private static void ConnectInLine(IList<TNode> line)
        {
            for (int i = 0; i < line.Count - 1; i++)
            {
                ConnectNodes(line[i], line[i + 1]);
            }
        }

        private static void ConnectBetweenLines(IList<TNode> left, IList<TNode> right)
        {
            if (left.Count > right.Count)
                (left, right) = (right, left);

            int[] degree = FindRandomDegree(left.Count, right.Count);

            for (int i = 0, j = 0; i < left.Count; i++)
            {
                int k = j + degree[i];
                for (; j < k; j++)
                {
                    ConnectNodes(left[i], right[j]);
                }
            }
        }

        private static void ConnectNodes(TNode first, TNode second)
        {
            first.Nodes.Add(second);
            second.Nodes.Add(first);
        }

        private static int[] FindRandomDegree(int left, int right)
        {
            if (left > right)
                (left, right) = (right, left);

            int[] degree = new int[left];
            for (int i = 0; i < left; i++)
            {
                degree[i] = 1;
            }
            int delta = right - left;
            for (int i = 0; i < delta; i++)
            {
                degree[Random.Range(0, left)] += 1;
            }
            return degree;
        }
    }
}