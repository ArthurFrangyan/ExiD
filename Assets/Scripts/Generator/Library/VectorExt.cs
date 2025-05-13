using UnityEngine;

namespace Generator.Library
{
    public static class VectorExt
    {
        public static Vector3Int ForwardRight => new Vector3Int(1, 0, 1);
        public static Vector3Int ForwardLeft => new Vector3Int(-1, 0, 1);
        public static Vector3Int BackRight => new Vector3Int(1, 0, -1);
        public static Vector3Int BackLeft => new Vector3Int(-1, 0, -1);
        public static Vector3Int VectorY(Vector3Int dVector)
        {
            return new Vector3Int(0, dVector.y, 0);
        }

        public static Vector3Int VectorXZ(Vector3Int dVector)
        {
            dVector.y = 0;
            return dVector;
        }

        public static Vector3Int NormalizeVector(Vector3Int vector)
        {
            return new Vector3Int(
                vector.x == 0 ? 0 : (vector.x > 0 ? 1 : -1),
                vector.y == 0 ? 0 : (vector.y > 0 ? 1 : -1),
                vector.z == 0 ? 0 : (vector.z > 0 ? 1 : -1)
            );
        }
        
    }
}