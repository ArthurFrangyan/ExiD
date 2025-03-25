using System.Linq;
using Assets.Scripts.Data;
using Assets.Scripts.Generator;
using Generator;
using Generator.Library;
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
        visualizer.Clean();
        RoomGraph2D rooms = new RoomGraph2D(new AreaProps(minCols, minRows, maxRows), new RoomProps(minRoomDiameter, maxRoomDiameter, new RandomWalkAreaGenerator()));
        visualizer.PaintRooms2D(rooms);
        TreeGenerator treeGenerator = new TreeGenerator();
        PathTree pathTree = treeGenerator.GenerateTree(rooms.SelectMany(list => list).Cast<Node>().ToList());
        PathGenerator tree = new PathGenerator(pathTree.Combinations, pathTree.Nodes);
        visualizer.PaintPaths(tree.PathPositions);
        // RoomGraph3D rooms = new RoomGraph3D(height, minRows, maxRows, minCols, maxCols, minRoomDiameter, maxRoomDiameter, minRoomHeight, maxRoomHeight);
        // visualizer.PaintRooms3D(rooms);
    }
}
