using UnityEngine;

namespace Generator.PathFinders.Movements
{
    public interface IMovement
    {
        Vector3Int BuildIn(Dungeon dung, Vector3Int position, Vector3Int direction);
    }
}