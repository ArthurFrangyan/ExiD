using UnityEngine;

namespace Generator.PathFinder.AStarAlgorithm
{
    public interface IVoxelGridRef<TClass> where TClass : class
    {
        Vector3Int Size { get; }
        TClass[,,] Raw { get; }
        TClass this[int x, int y, int z] { get; set; }
        TClass this[Vector3Int pos] { get; set; }
        bool InBounds(int x, int y, int z);
        bool InBounds(Vector3Int pos);
    }
}