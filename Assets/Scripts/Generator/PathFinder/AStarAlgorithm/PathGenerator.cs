using Generator.Library;
using UnityEngine;

namespace Generator.PathFinder.AStarAlgorithm
{
    public class PathGenerator
    {
        public void GeneratePaths(Dungeon dungeon, PathTree<Room> pathTree)
        {
            foreach (Edge<Room> edge in pathTree.Edges)
            {
                var vertices = VoxelGridVertex.GenerateVertexList(dungeon);
                vertices.ExcludeEdgeLock(dungeon, edge);
                GenerateBetweenRooms(vertices, dungeon,
                    edge.A.Position - dungeon.Position,
                    edge.B.Position - dungeon.Position);
                vertices.IncludeEdgeLock(dungeon, edge);

                // TODO: BlockBuilder
            }
            BlockBuilder.BuildWallsCeilFloorBasedOnLock(dungeon);
        }

        private void GenerateBetweenRooms(VoxelGridVertex vertices, Dungeon dungeon,
            Vertex start, Vertex goal)
        {
            AStarAlgorithm algorithm = new AStarAlgorithm();
            algorithm.FindVertex(vertices, start, goal);
            algorithm.ReconstructPath(goal, dungeon);
        }


        private void GenerateBetweenRooms(VoxelGridVertex vertices, Dungeon dungeon, Edge<Vertex> edge)
        {
            GenerateBetweenRooms(vertices, dungeon, edge.A, edge.B);
        }

        private void GenerateBetweenRooms(VoxelGridVertex vertices, Dungeon dungeon,
            Vector3Int startPos, Vector3Int goalPos)
        {
            GenerateBetweenRooms(vertices, dungeon, vertices[startPos], vertices[goalPos]);
        }

        public void GenerateBetweenRooms(VoxelGridVertex vertices, Dungeon dungeon,
            Edge<Room> edge)
        {
            GenerateBetweenRooms(vertices, dungeon, vertices[edge.A.Position], vertices[edge.B.Position]);
        }

        public void GenerateBetweenRooms(Dungeon dungeon, Vector3Int startPos, Vector3Int goalPos)
        {
            VoxelGridVertex vertices = VoxelGridVertex.GenerateVertexList(dungeon);
            GenerateBetweenRooms(vertices, dungeon, vertices[startPos], vertices[goalPos]);
        }
    }
}