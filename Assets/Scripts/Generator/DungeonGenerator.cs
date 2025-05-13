using Generator.GraphAlgorithm;
using Generator.GraphAlgorithm.RoomGraph3D;
using Generator.PathFinders.AStarAlgorithm;
using Generator.Shape;

namespace Generator
{
    public static class DungeonGenerator
    {
        public static void Generate(Visualizer visualizer, ushort height, ushort minRows, ushort maxRows, ushort minCols, ushort maxCols, ushort minRoomDiameter, ushort maxRoomDiameter, ushort minRoomHeight, ushort maxRoomHeight)
        {
            // var rooms = new RoomGraph2D(new AreaProps(minCols, minRows, maxRows), new RoomProps(minRoomDiameter, maxRoomDiameter, new RandomWalkAreaGenerator()));
            var rooms =new RoomGraph3D(new VolumeProps(height, maxCols, minCols, maxRows, minRows, maxRoomDiameter, minRoomDiameter), new RoomProps(minRoomDiameter, maxRoomDiameter, new RandomWalkAreaGenerator()));
            
            var pathGenerator = new PathGenerator();
            
            var treeGenerator = new TreeGenerator();
            // var pathTree = treeGenerator.GenerateTree(rooms.SelectMany(list => list).Cast<Node>().ToList());
            var pathTree = treeGenerator.GenerateTree(rooms.ConvertListNodes());
            
            var dungeon = Dungeon.GenerateDungeonMatrix(rooms.ConvertListRoom());
            pathTree.ToPathTreeRoom();
            pathGenerator.GeneratePaths(dungeon, pathTree.ToPathTreeRoom());
            
            visualizer.Clean();
            visualizer.PaintDungeon(dungeon);
            
            // RoomGraph3D rooms = new RoomGraph3D(height, minRows, maxRows, minCols, maxCols, minRoomDiameter, maxRoomDiameter, minRoomHeight, maxRoomHeight);
            // visualizer.PaintRooms3D(rooms);
        }
    }
}