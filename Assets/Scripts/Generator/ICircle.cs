using UnityEngine;

namespace Generator
{
    public interface ICircle
    {
        public Vector3 Center { get; }
        public int Diameter { get; }
    }
}