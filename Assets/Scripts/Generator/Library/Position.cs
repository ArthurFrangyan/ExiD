using UnityEngine;

namespace Generator.Library
{
    public struct Position
    {
        private Vector3Int _vector;
        public static Position Zero => new(Vector3Int.zero);
        
        public static bool operator <(Position position, Vector3Int vector)
        {
            return position._vector.x < vector.x && 
                   position._vector.y < vector.y && 
                   position._vector.z < vector.z;
        }

        public static bool operator >(Position position, Vector3Int vector)
        {
            return position._vector.x > vector.x && 
                   position._vector.y > vector.y && 
                   position._vector.z > vector.z;
        }

        public static bool operator <=(Position position, Vector3Int vector)
        {
            return position._vector.x <= vector.x && 
                   position._vector.y <= vector.y && 
                   position._vector.z <= vector.z;
        }

        public static bool operator >=(Position position, Vector3Int vector)
        {
            return position._vector.x >= vector.x && 
                   position._vector.y >= vector.y && 
                   position._vector.z >= vector.z;
        }

        public Position(Vector3Int vector)
        {
            _vector = vector;
        }
        
        
        public static implicit operator Position(Vector3Int vector) => new(vector);
        public static implicit operator Vector3Int(Position position) => position._vector;
    }
}