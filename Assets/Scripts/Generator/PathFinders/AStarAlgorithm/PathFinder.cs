using System.Collections.Generic;
using Generator.Collections;
using Generator.PathFinders.Movements;
using UnityEngine;
using static Generator.Library.VectorExt;

namespace Generator.PathFinders.AStarAlgorithm
{
    public class PathFinder
    {

        private readonly Vector3Int[,] _directions = {
            { Vector3Int.right, Vector3Int.left },
            { Vector3Int.forward, Vector3Int.back },
            { Vector3Int.up, Vector3Int.down, }
        };

        private readonly VoxelGridVertex _vertices;
        public PathFinder(VoxelGridVertex vertices)
        {
            _vertices = vertices;
        }

        private enum DirAxes
        {
            XRightLeft = 0,
            ZFrontBack = 1,
            YUpDown = 2
        }

        public Vertex ReconstructPath(Vertex end, Dungeon dungeon)
        {
            if (end is null)
                return null;
            
            Vertex successor = end;
            
            end.IsLocked = true;
            
            while (end.Predecessor is not null && !successor.IsStart)
            {
                successor = end;

                if (end.Predecessor.Predecessor is null)
                    break;
                
                var vector = end.Predecessor.Position - end.Position;
                
                end = _vertices[end.MovementBuild(dungeon)];
            }
            return successor;
        }

        private Vertex DiagonalCase(Vertex end, Dungeon dungeon, Vector3Int vector)
        {
            dungeon[end.Position].IsLocked = true;
            dungeon[end.Position + VectorY(vector)].IsLocked = true;
            dungeon[end.Position + VectorXZ(vector)].IsLocked = true;
            dungeon[end.Predecessor.Position].IsLocked = true;
            return end.Predecessor;
        }

        private Vertex CornerCase(Vertex end, Dungeon dungeon, Vector3Int vector)
        {
            dungeon[end.Position + VectorY(vector)].IsLocked = true;
            return DiagonalCase(end.Predecessor, dungeon, vector);
        }

        public Vertex FindVertex(Vertex start, Vertex goal)
        {
            start.Movement = new StartPointMovement();
            start.Predecessor = new Vertex(true, new Vector3Int(-1, -1, -1));;
            PriorityQueue<Vertex, int> q = new PriorityQueue<Vertex, int>();
            q.Enqueue(start, _vertices[start.Position].MinDistance + Heuristic(start, goal));
            _vertices[start.Position].MinDistance = 0;
            
            while (q.Count != 0)
            {
                Vertex current = q.Dequeue();
                
                var adjVertices = FindNextVertices( current);
                foreach (Vertex adjVertex in adjVertices)
                {
                    if (adjVertex.IsGoal)
                        return adjVertex;
                    q.Remove(adjVertex);
                    q.Enqueue(adjVertex, _vertices[adjVertex.Position].MinDistance + Heuristic(goal, adjVertex));
                }
            }

            return goal;
        }

        private List<Vertex> FindNextVertices(Vertex current)
        {
            var nextVertices = new List<Vertex>();

            StraightTraversal(current, nextVertices);
            StairsStraightTraversal(current, nextVertices);
            // GoStairsLShaped(current, nextVertices);
            
            return nextVertices;
        }

        private void StraightTraversal(Vertex current, List<Vertex> nextVertices)
        {
            for (int dir = (int)DirAxes.XRightLeft; dir <= (int)DirAxes.ZFrontBack; dir++)
            for (var sign = 0; sign < 2; sign++)
            {
                var nextPos = current.Position + _directions[dir, sign];
                if (ValidTraversal(current, _vertices[nextPos]))
                {
                    GoStraight(current, _vertices[nextPos], nextVertices);
                }
            }
        }

        private void StairsStraightTraversal(Vertex current, List<Vertex> nextVertices)
        {
            for (int idxDirXZ = (int)DirAxes.XRightLeft; idxDirXZ < (int)DirAxes.ZFrontBack; idxDirXZ++)
            for (var idxSignXZ = 0; idxSignXZ < _directions.GetLength(1); idxSignXZ++)
            for (var idxSignY = 0; idxSignY < _directions.GetLength(1); idxSignY++)
            {
                var idxDirY = (int)DirAxes.YUpDown;

                var dirXZ = _directions[idxDirXZ, idxSignXZ];
                var dirY = _directions[idxDirY, idxSignY];
                var pos1 = current.Position + dirXZ;
                var pos2 = pos1 + dirXZ + dirY;
                var pos3 = pos2 + dirXZ;

                if (!IsStraightStairsTraversalValid(_vertices[pos1], current, dirXZ + dirY)) continue;
                
                GoStraightStairs(current, _vertices[pos3], nextVertices);
                    
                // GoStraight(current, _vertices[pos1], nextVertices);
                // GoStraight(_vertices[pos1], _vertices[pos2], nextVertices);
                // GoStraight(_vertices[pos1], _vertices[pos1 + dirXZ], nextVertices);
                // GoStraight(_vertices[pos3], _vertices[pos3 - dirXZ], nextVertices);
            }
        }

        private void StairsLShapedTraversal(Vertex current, List<Vertex> nextVertices)
        {
            for (var axesXZ = DirAxes.XRightLeft; axesXZ < DirAxes.ZFrontBack; axesXZ++)
            for (var signXZ = 0; signXZ < _directions.GetLength(1); signXZ++)
            for (var signY = 0; signY < _directions.GetLength(1); signY++)
            for (var signTurnXZ = 0; signTurnXZ < _directions.GetLength(1); signTurnXZ++)
            {
                var idxDirXZ = (int)axesXZ;
                int idxDirTurnXZ;
                if (axesXZ == DirAxes.XRightLeft)
                    idxDirTurnXZ = (int)DirAxes.ZFrontBack;
                else
                    idxDirTurnXZ = (int)DirAxes.XRightLeft;
                var idxDirY = (int)DirAxes.YUpDown;
                
                var dirXZ = _directions[idxDirXZ, signXZ];
                var dirY = _directions[idxDirY, signY];
                var dir = dirXZ + dirY;
                var dirTurn = _directions[idxDirTurnXZ, signTurnXZ];
                
                var pos1 = current.Position + dirXZ;
                var pos2 = pos1 + dirXZ + dirY;
                var pos3 = pos2 + dirTurn;
                var pos4 = pos3 + dirTurn;

                if (!IsStairsLShapedTraversalValid(_vertices[pos1], current, dir, dirTurn)) continue;
                GoLShapedStairs(current, _vertices[pos4], nextVertices, dir, dirTurn);
                    
                GoStraight(current, _vertices[pos1 + dirXZ], nextVertices);
                GoStraight(_vertices[pos1], _vertices[pos2], nextVertices);
                GoStraight(_vertices[pos2], _vertices[pos3], nextVertices);
                GoStraight(_vertices[pos2], _vertices[pos1 + dirY], nextVertices);
                GoStraight(_vertices[pos1], _vertices[pos1 + dirXZ], nextVertices);
                GoStraight(_vertices[pos1 + dirXZ], _vertices[pos1 + dirXZ + dirTurn], nextVertices);
            }
        }

        private void GoStraight(Vertex from, Vertex to, ICollection<Vertex> nextVertices)
        {
            to.Movement = new StraightMovement();
            to.MinDistance = from.MinDistance + Math.ManhattanDistance(from, to);
            to.Predecessor = from;
            nextVertices.Add(to);
        }

        private void GoStraightStairs(Vertex from, Vertex to, ICollection<Vertex> nextVertices)
        {
            to.Movement = new StairsStraightMovement();
            to.MinDistance = from.MinDistance + Math.ManhattanDistance(from, to);
            to.Predecessor = from;
            nextVertices.Add(to);
        }

        private void GoLShapedStairs(Vertex from, Vertex to, ICollection<Vertex> nextVertices, Vector3Int dir, Vector3Int dirTurn)
        {
            to.Movement = StairsLShapedMovement.ConstructStairsLShaped(dir, dirTurn);
            to.MinDistance = from.MinDistance + Math.ManhattanDistance(from, to);
            to.Predecessor = from;
            nextVertices.Add(to);
        }

        private bool IsStraightStairsTraversalValid(Vertex start, Vertex predecessor, Vector3Int dir)
        {
            return NotLocked(start) &&
                   NotLocked(_vertices[start.Position + VectorXZ(dir)]) &&
                   NotLocked(_vertices[start.Position + VectorY(dir)]) &&
                   NotLocked(_vertices[start.Position + dir]) &&
                   ValidTraversal(predecessor, _vertices[start.Position + dir + VectorXZ(dir)]);
        }

        private bool ValidTraversalInMiddle(Vertex current, Vertex next)
        {
            return NotLocked(next) &&
                   PathShorterOrEqual(current, next);
        }

        private bool ValidTraversal(Vertex current, Vertex next)
        {
            return NotLocked(next) &&
                   PathShorter(current, next);
        }

        private bool NotLocked(Vertex next)
        {
            return next is not null && 
                   !next.IsLocked;
        }

        private bool PathShorterEqualAndVisited(Vertex current, Vertex next)
        {
            return next.Predecessor is not null &&
                   next.MinDistance >= current.MinDistance + Math.ManhattanDistance(current, next);
        }

        private bool PathShorter(Vertex current, Vertex next)
        {
            return next.Predecessor is null || 
                   next.MinDistance > current.MinDistance + Math.ManhattanDistance(current, next);
        }

        private bool PathShorterOrEqual(Vertex current, Vertex next)
        {
            return next.Predecessor is null || 
                   next.MinDistance >= current.MinDistance + Math.ManhattanDistance(current, next);
        }

        private bool IsStairsLShapedTraversalValid(Vertex start, Vertex predecessor, 
            Vector3Int dirDiagonal, Vector3Int dirTurn)
        {
            return IsStraightStairsTraversalValid(start, predecessor, dirDiagonal) &&
                   NotLocked(_vertices[start.Position + dirDiagonal + dirTurn]) &&
                   NotLocked(_vertices[start.Position + dirDiagonal + dirTurn + dirTurn]) &&
                   !IsLocked(_vertices[0, dirDiagonal.y, 0]);
        }

        private bool IsLocked(Vertex current)
        {
            if (current is null)
                return false;
            
            return current.IsLocked;
        }

        private int Heuristic(Vertex first, Vertex second)
        {
            return Heuristic(first.Position - second.Position);
        }

        private int Heuristic(Vector3Int dPos)
        {
            return Math.ManhattanDistance(dPos);
        }
    }
} 