using System.Linq;
using Assets.Scripts.Data;
using Assets.Scripts.Generator;
using Generator;
using Generator.GraphAlgorithm;
using Generator.Library;
using Generator.PathFinder.AStarAlgorithm;
using Generator.Shape;
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
        var rooms = new RoomGraph2D(new AreaProps(minCols, minRows, maxRows), new RoomProps(minRoomDiameter, maxRoomDiameter, new RandomWalkAreaGenerator()));
        // var rooms =new RoomGraph3D(height, minRows, maxRows, minCols, maxCols, minRoomDiameter, maxRoomDiameter, minRoomHeight, maxRoomHeight);
        
        
        var pathGenerator = new PathGenerator();
        
        var treeGenerator = new TreeGenerator();
        // var pathTree = treeGenerator.GenerateTree(rooms.SelectMany(list => list).Cast<Node>().ToList());
        var pathTree = treeGenerator.GenerateTree(rooms.ConvertListNodes());
        
        
        var dungeon = Dungeon.GenerateDungeonMatrix(rooms.ConvertList());
        pathTree.ToPathTreeRoom();
        pathGenerator.GeneratePaths(dungeon, pathTree.ToPathTreeRoom());

        visualizer.Clean();
        visualizer.PaintDungeon(dungeon);
        // RoomGraph3D rooms = new RoomGraph3D(height, minRows, maxRows, minCols, maxCols, minRoomDiameter, maxRoomDiameter, minRoomHeight, maxRoomHeight);
        // visualizer.PaintRooms3D(rooms);
    }
}
