using System.Collections.Generic;
using Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Generator.PathFinder.AStarAlgorithm
{
    public class AStarAlgorithm
    {

        private readonly Vector3Int[,] _directions = {
            { Vector3Int.right, Vector3Int.left },
            { Vector3Int.forward, Vector3Int.back },
            { Vector3Int.up, Vector3Int.down, }
        };

        private enum DirAxes
        {
            XRightLeft = 0,
            ZFrontBack = 1,
            YUpDown = 2
        }

        public void ReconstructPath(Vertex end, Dungeon dungeon)
        {
            while (end.Predecessor is not null)
            {
                dungeon[end.Position].IsLocked = true;

                var vector1 = end.Predecessor.Position - end.Position;
                Vector3Int vector2;
                if (end.Predecessor.Predecessor is not null)
                    vector2 = end.Predecessor.Predecessor.Position - end.Predecessor.Position;
                else
                    vector2 = Vector3Int.zero;

                if (VectorXZ(vector2) == vector1)
                {
                    if (HasHeightDelta(vector2))
                    {
                        end = DiagonalCase(end.Predecessor, dungeon, vector2);
                        continue;
                    }
                }
                else
                {
                    if (HasHeightDelta(vector2))
                    {
                        dungeon.Stairs.Add(vector2.y > 0
                            ? new Stairs(end.Position, vector1, VectorXZ(vector2))
                            : new Stairs(end.Predecessor.Predecessor.Position, -VectorXZ(vector2), -vector1));
                        end = CornerCase(end, dungeon, vector2);
                        continue;
                    }
                }
                
                end = end.Predecessor;
            }
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

        private bool HasHeightDelta(Vector3Int dVector)
        {
            return dVector.y != 0;
        }

        private Vector3Int VectorY(Vector3Int dVector)
        {
            return new Vector3Int(0, dVector.y, 0);
        }

        private Vector3Int VectorXZ(Vector3Int dVector)
        {
            dVector.y = 0;
            return dVector;
        }

        public void FindVertex(VoxelGridVertex vertices, Vertex start, Vertex goal)
        {
            start.Predecessor = new Vertex(false, Vector3Int.zero);
            PriorityQueue<Vertex, int> q = new PriorityQueue<Vertex, int>();
            q.Enqueue(start, vertices[start.Position].MinDistance + Heuristic(start, goal));
            vertices[start.Position].MinDistance = 0;
            
            while (q.Count != 0)
            {
                Vertex current = q.Dequeue();
                
                var adjVertices = FindNextVertices(vertices, current, goal);
                foreach (Vertex adjVertex in adjVertices)
                {
                    q.Enqueue(adjVertex, vertices[adjVertex.Position].MinDistance + Heuristic(goal, adjVertex));
                }
            }
        }

        private List<Vertex> FindNextVertices(VoxelGridVertex vertices, Vertex current, Vertex goal)
        {
            var nextVertices = new List<Vertex>();

            for (int dir = (int)DirAxes.XRightLeft; dir <= (int)DirAxes.ZFrontBack; dir++)
            for (var sign = 0; sign < 2; sign++)
            {
                var nextPos = current.Position + _directions[dir, sign];
                if (IsTraversalValid(vertices[nextPos], current, goal))
                {
                    nextVertices.Add(vertices[nextPos]);
                    vertices[nextPos].MinDistance = current.MinDistance + 1;
                    vertices[nextPos].Predecessor = current;
                }
            }

            // for (int dirXZ = (int)DirAxes.XRightLeft; dirXZ < (int)DirAxes.ZFrontBack; dirXZ++)
            // for (var signXZ = 0; signXZ < _directions.GetLength(1); signXZ++)
            // for (var signY = 0; signY < _directions.GetLength(1); signY++)
            // {
            //     var dirY = (int)DirAxes.YUpDown;
            //
            //     var pos1 = current.Position + _directions[dirXZ, signXZ];
            //     var pos2 = pos1 + _directions[dirXZ, signXZ] + _directions[dirY, signY];
            //     var pos3 = pos2 + _directions[dirXZ, signXZ];
            //     
            //     if (IsDiagonalTraversalValid(vertices, vertices[pos1], current, goal, _directions[dirXZ, signXZ] + _directions[dirY, signY]) &&
            //         IsTraversalValid(vertices[pos3], current, goal))
            //     {
            //         nextVertices.Add(vertices[pos3]);
            //         
            //         vertices[pos1].MinDistance = current.MinDistance + 1;
            //         vertices[pos2].MinDistance = current.MinDistance + 2;
            //         vertices[pos3].MinDistance = current.MinDistance + 3;
            //         
            //         vertices[pos1].Predecessor = vertices[pos2];
            //         vertices[pos2].Predecessor = vertices[pos3];
            //         vertices[pos3] = current;
            //     }
            // }

            // for (var axesXZ = DirAxes.XRightLeft; axesXZ < DirAxes.ZFrontBack; axesXZ++)
            // for (var signXZ = 0; signXZ < _directions.GetLength(1); signXZ++)
            // for (var signY = 0; signY < _directions.GetLength(1); signY++)
            // for (var signTurnXZ = 0; signTurnXZ < _directions.GetLength(1); signTurnXZ++)
            // {
            //     var dirXZ = (int)axesXZ;
            //     int dirTurnXZ;
            //     if (axesXZ == DirAxes.XRightLeft)
            //         dirTurnXZ = (int)DirAxes.ZFrontBack;
            //     else
            //         dirTurnXZ = (int)DirAxes.XRightLeft;
            //
            //     var dirY = (int)DirAxes.YUpDown;
            //
            //     var pos1 = current.Position + _directions[dirXZ, signXZ];
            //     var pos2 = pos1 + _directions[dirXZ, signXZ] + _directions[dirY, signY];
            //     var pos3 = pos2 + _directions[dirTurnXZ, signTurnXZ];
            //     var pos4 = pos3 + _directions[dirTurnXZ, signTurnXZ];
            //     
            //     if (IsDiagonalCornerTraversalValid(vertices, vertices[pos1], current, goal,
            //             _directions[dirXZ, signXZ] + _directions[dirY, signY], 
            //             _directions[dirTurnXZ, signTurnXZ]) &&
            //         IsTraversalValid(vertices[pos4], current, goal))
            //     {
            //         nextVertices.Add(vertices[pos4]);
            //         
            //         vertices[pos1].MinDistance = current.MinDistance + 1;
            //         vertices[pos2].MinDistance = current.MinDistance + 2;
            //         vertices[pos3].MinDistance = current.MinDistance + 3;
            //         vertices[pos4].MinDistance = current.MinDistance + 4;
            //
            //         vertices[pos4].Predecessor = vertices[pos3];
            //         vertices[pos3].Predecessor = vertices[pos2];
            //         vertices[pos2].Predecessor = vertices[pos1];
            //         vertices[pos1].Predecessor = current;
            //     }
            // }
            
            return nextVertices;
        }

        private bool IsTraversalValid(Vertex current, Vertex predecessor, Vertex goal)
        {
            if (current is null)
                return false;
            
            if (current.IsLocked)
                return false;

            if (current.Predecessor is null)
                return true;

            return current.MinDistance > predecessor.MinDistance + Math.ManhattanDistance(current, predecessor);
        }

        private bool IsDiagonalTraversalValid(VoxelGridVertex vertices, Vertex start, Vertex predecessor, Vertex goal, Vector3Int dir)
        {
            return IsTraversalValid(start, predecessor, goal) &&
                   IsTraversalValid(vertices[start.Position + dir], predecessor, goal) &&
                   !IsLocked(vertices[start.Position + new Vector3Int(0, 0, dir.z)]) &&
                   !IsLocked(vertices[start.Position + new Vector3Int(0, dir.y, 0)]) &&
                   !IsLocked(vertices[start.Position + new Vector3Int(dir.x, 0, 0)]);
        }

        private bool IsDiagonalCornerTraversalValid(VoxelGridVertex vertices, Vertex start, Vertex predecessor, Vertex goal, 
            Vector3Int dirDiagonal, Vector3Int dirTurn)
        {
            return IsDiagonalTraversalValid(vertices, start, predecessor, goal, dirDiagonal) &&
                   IsTraversalValid(vertices[start.Position + dirDiagonal + dirTurn], predecessor, goal) &&
                   !IsLocked(vertices[new Vector3Int(0,dirDiagonal.y,0)]);
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