using UnityEngine;

namespace Generator
{
    public class Stairs
    {
        enum Type
        {
            Diagonal = 0,
            CornerRight = 1,
            CornerLeft = 2,
        }
        private Type _type;
        public Vector3Int Position { get; set; }
        public Vector3Int Direction { get; set; }

        public Stairs(Vector3Int position, Vector3Int direction, Vector3Int direction2)
        {
            Position = position;
            Direction = direction;
            _type = direction != Vector3Int.zero ? GetType(direction, direction2) : Type.Diagonal;
        }

        private Type GetType(Vector3Int dir1, Vector3Int dir2)
        {
            var cross = Cross2D(dir1, dir2);
            if (cross > 0)
            {
                return Type.CornerRight;
            }
            if (cross < 0)
            {
                return Type.CornerLeft;
            }
            return Type.Diagonal;
        }
        float Cross2D(Vector3 a, Vector3 b)
        {
            return a.x * b.z - a.z * b.x;
        }
    }
}