using UnityEngine;

namespace Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnvironmentScriptableObject", menuName = "Scriptable Objects/EnvironmentScriptableObject")]
    public class EnvironmentScriptableObject : ScriptableObject
    {
        public GameObject[] Ceils;
        public GameObject[] Floors;
        public GameObject[] Columns;
        public GameObject[] BrokenFloors;
        public GameObject[] FloorDusts;
        public GameObject[] Walls;
        public GameObject[] Stairs;
        public GameObject[] Bricks;
        public GameObject[] BrickCracks;
        public GameObject[] StoneCorners;
        public GameObject[] WodenBoardsAndBars;
    }
}
