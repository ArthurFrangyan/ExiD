using UnityEngine;

namespace Generator
{
    public class Stairs
    {
        public enum Type
        {
            Straight = 0,
            LShapedRight = 1,
            LShapedLeft = 2,
        }

        public int GetTypeIndex() => (int)_type;

        private readonly Type _type;

        public Vector3Int Position { get; set; }

        public float RotationY { get; set; }

        public Stairs(Vector3Int position, Vector3Int dir, Type type)
        {
            Position = position;
            _type = type;
            
            RotationY = GetRotationY(dir);
        }
        private float GetRotationY(Vector3Int dir)
        {
            switch (_type)
            {
                case Type.Straight:
                    return Vector3.SignedAngle(Vector3.forward, dir, Vector3.up);
                case Type.LShapedRight:
                    return Vector3.SignedAngle(new Vector3(1, 0, 1),dir, Vector3.up);
                case Type.LShapedLeft:
                    return Vector3.SignedAngle(new Vector3(-1, 0, 1),dir, Vector3.up);
            }
            return 0;
        }
    }
}