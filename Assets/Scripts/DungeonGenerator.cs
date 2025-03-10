using Assets.Scripts.Data;
using Assets.Scripts.Generator;
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
    private ushort cols;
    [SerializeField]
    private ushort minDiameter;
    [SerializeField]
    private ushort maxDiameter;

    public void RunProceduralGeneration()
    {
        visualizer.Clean();
        RoomGraph rooms = new RoomGraph(minRows, maxRows, cols, minDiameter, maxDiameter);
        visualizer.PaintRooms(rooms);
        PathTreeGenerator tree = new PathTreeGenerator(rooms);
        visualizer.PaintPaths(tree.PathPositions);
    }
}
