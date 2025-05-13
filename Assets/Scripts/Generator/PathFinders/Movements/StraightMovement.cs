using UnityEngine;

namespace Generator.PathFinders.Movements
{
    public class StraightMovement : IMovement
    {

        public Vector3Int BuildIn(Dungeon dung, Vector3Int position, Vector3Int dir)
        {
            dung[position + dir].IsLocked = true;
            return position + dir;
        }
    }
}