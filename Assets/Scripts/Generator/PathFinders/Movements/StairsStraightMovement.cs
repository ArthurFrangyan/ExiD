using UnityEngine;
using static Generator.Library.VectorExt;

namespace Generator.PathFinders.Movements
{
    public class StairsStraightMovement : IMovement
    {
        public Vector3Int BuildIn(Dungeon dung, Vector3Int position, Vector3Int direction)
        {
            var dir = NormalizeVector(direction);
            var pos = position + VectorXZ(dir);
            
            CreateStairs(dung, pos, dir);

            MarkInDungeon(dung, position, pos, dir);


            return pos + dir + VectorXZ(dir);
        }

        private static void MarkInDungeon(Dungeon dung, Vector3Int position, Vector3Int pos, Vector3Int dir)
        {
            dung[pos].IsLocked = true;
            dung[pos].IsLocked = true;
            dung[pos + VectorY(dir)].IsLocked = true;
            dung[pos + VectorXZ(dir)].IsLocked = true;
            dung[pos + dir].IsLocked = true;
            dung[pos + dir + VectorXZ(dir)].IsLocked = true;
            
            dung[pos].SetConnected(VectorY(dir), true);
            dung[pos + VectorY(dir)].SetConnected(-VectorY(dir), true);
            dung[pos + VectorXZ(dir)].SetConnected(VectorY(dir), true);
            dung[pos + dir].SetConnected(-VectorY(dir), true);

            if (dir.y > 0)
                dung[pos + 2 * VectorXZ(dir)].SetBorder(-VectorXZ(dir), true);
            else
                dung[position + VectorY(dir)].SetBorder(VectorXZ(dir), true);
        }

        private static void CreateStairs(Dungeon dung, Vector3Int pos, Vector3Int dir)
        {
            var cellSize = Dungeon.CellSize;
            dung.Staircases.Add(dir.y > 0
                ? new Stairs(
                    pos * cellSize + dung.Position, 
                    VectorXZ(dir), Stairs.Type.Straight)
                : new Stairs(
                    (pos + dir)*cellSize + dung.Position,
                    -VectorXZ(dir), Stairs.Type.Straight));
        }
    }
}