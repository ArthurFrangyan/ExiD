using UnityEngine;

namespace Generator.PathFinders.Movements
{
    public static class StairsLShapedMovement
    {
        public static IMovement ConstructStairsLShaped(Vector3Int dir, Vector3Int dirTurn)
        {
            if (Vector3.SignedAngle(dir, dirTurn, Vector3.up) > 0)
                return new StairsLShapedRightMovement();
            else
                return new StairsLShapedLeftMovement();
        }
    }
}