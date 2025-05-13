using UnityEngine;

namespace Generator.PathFinders.Movements
{
    public class StartPointMovement : IMovement
    {
        public Vector3Int BuildIn(Dungeon dung, Vector3Int position, Vector3Int direction)
        {
            dung[position].IsLocked = true;
            return new Vector3Int(-1, -1, -1);
        }
    }
}