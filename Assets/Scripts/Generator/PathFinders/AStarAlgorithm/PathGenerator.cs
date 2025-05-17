using Generator.Library;
using UnityEngine;

namespace Generator.PathFinders.AStarAlgorithm
{
    public class PathGenerator
    {
        public void GeneratePaths(Dungeon dungeon, PathTree<Room> pathTree)
        {
            var vertices = new VoxelGridVertex(dungeon);
            foreach (Edge<Room> edge in pathTree.Edges)
            {
                vertices.MarkStartAndUnlockRoom(dungeon, edge.A);
                vertices.MarkGoalAndUnlockRoom(dungeon, edge.B);
                
                var (start, end) = GenerateBetweenRooms(vertices, dungeon,
                    edge.A.CenterInt - dungeon.Position,
                    edge.B.CenterInt - dungeon.Position);

                vertices.RevertStartRoomToLocked(dungeon, edge.A);
                vertices.RevertGoalRoomToLocked(dungeon, edge.B);
                
                vertices.ResetPathAnalysisData();
            }
            
            BlockBuilder.BuildBordersBasedOnLockLevelSeparately(dungeon);
            BlockBuilder.RebuildEdgesByFaces(dungeon);
        }

        private Edge<Vertex> GenerateBetweenRooms(VoxelGridVertex vertices, Dungeon dungeon,
            Vertex start, Vertex goal)
        {
            PathFinders.AStarAlgorithm.PathFinder algorithm = new PathFinders.AStarAlgorithm.PathFinder(vertices);
            Vertex end = algorithm.FindVertex(start, goal);
            Vertex successorStart = algorithm.ReconstructPath(end, dungeon);
            return new Edge<Vertex>(successorStart.Predecessor, successorStart);
        }


        private void GenerateBetweenRooms(VoxelGridVertex vertices, Dungeon dungeon, Edge<Vertex> edge)
        {
            GenerateBetweenRooms(vertices, dungeon, edge.A, edge.B);
        }

        private Edge<Vertex> GenerateBetweenRooms(VoxelGridVertex vertices, Dungeon dungeon,
            Vector3Int startPos, Vector3Int goalPos)
        {
            return GenerateBetweenRooms(vertices, dungeon, vertices[startPos], vertices[goalPos]);
        }

        public void GenerateBetweenRooms(VoxelGridVertex vertices, Dungeon dungeon,
            Edge<Room> edge)
        {
            GenerateBetweenRooms(vertices, dungeon, vertices[edge.A.Position], vertices[edge.B.Position]);
        }

        public void GenerateBetweenRooms(Dungeon dungeon, Vector3Int startPos, Vector3Int goalPos)
        {
            VoxelGridVertex vertices = new VoxelGridVertex(dungeon);
            GenerateBetweenRooms(vertices, dungeon, vertices[startPos], vertices[goalPos]);
        }
    }
}