using Generator;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField]
    private Visualizer visualizer;
    [SerializeField]
    private ushort minRows;
    [SerializeField]
    private ushort maxRows;
    [SerializeField]
    private ushort minCols;
    [SerializeField]
    private ushort maxCols;
    [SerializeField]
    private ushort height;
    [SerializeField]
    private ushort minRoomDiameter;
    [SerializeField]
    private ushort maxRoomDiameter;
    [SerializeField]
    private ushort minRoomHeight;
    [SerializeField]
    private ushort maxRoomHeight;

    public void RunProceduralGeneration()
    {
        Generator.DungeonGenerator.Generate(visualizer, height, minRows, maxRows, minCols, maxCols, minRoomDiameter, maxRoomDiameter, minRoomHeight, maxRoomHeight);
    }
}
