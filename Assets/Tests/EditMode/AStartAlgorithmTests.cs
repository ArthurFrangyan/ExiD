using Generator;
using Generator.PathFinders.AStarAlgorithm;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode
{
    [TestFixture]
    public class AStartAlgorithmTests
    {
        [Test]
        public void ReconstructPath_WhenCalled_ReturnClosestStartVertex()
        {
            // Arrange
            Dungeon dungeon = new Dungeon(new Vector3Int(6, 1, 6), new Vector3Int(0, 0, 0));
            dungeon[1, 0, 1].IsLocked = true;
            dungeon[1, 0, 2].IsLocked = true;
            dungeon[2, 0, 1].IsLocked = true;
            dungeon[2, 0, 2].IsLocked = true;
            dungeon[4, 0, 4].IsLocked = true;
            VoxelGridVertex vertices = new VoxelGridVertex(dungeon);
            PathFinder algorithm = new PathFinder(vertices);
            vertices[1, 0, 1].IsLocked = false;
            vertices[1, 0, 2].IsLocked = false;
            vertices[2, 0, 1].IsLocked = false;
            vertices[2, 0, 2].IsLocked = false;
            vertices[4, 0, 4].IsLocked = false;
            vertices[1, 0, 1].IsStart = true;
            vertices[1, 0, 2].IsStart = true;
            vertices[2, 0, 1].IsStart = true;
            vertices[2, 0, 2].IsStart = true;
            vertices[4, 0, 4].IsGoal = true;
            Vertex start = vertices[1, 0, 0];
            Vertex end = vertices[4, 0, 4];

            vertices[4, 0, 4].Predecessor = vertices[3, 0, 4];
            vertices[3, 0, 4].Predecessor = vertices[2, 0, 4];
            vertices[2, 0, 4].Predecessor = vertices[2, 0, 3];
            vertices[2, 0, 3].Predecessor = vertices[2, 0, 2];
            vertices[2, 0, 2].Predecessor = new Vertex(true, new Vector3Int(-1,0,-1));
            
            // Act
            var result = algorithm.ReconstructPath(end, dungeon);

            
            Assert.That(result.Position, Is.EqualTo(new Vector3Int(2,0,2)));
        }
        
        [Test]
        public void FindVertex_WhenCalled_ReturnClosestEndVertex()
        {
            Dungeon dungeon = new Dungeon(new Vector3Int(6, 1, 6), new Vector3Int(0, 0, 0));
            dungeon[1, 0, 1].IsLocked = true;
            dungeon[1, 0, 2].IsLocked = true;
            dungeon[2, 0, 1].IsLocked = true;
            dungeon[2, 0, 2].IsLocked = true;
            dungeon[4, 0, 4].IsLocked = true;
            VoxelGridVertex vertices = new VoxelGridVertex(dungeon);
            PathFinder algorithm = new PathFinder(vertices);
            vertices[1, 0, 1].IsLocked = false;
            vertices[1, 0, 2].IsLocked = false;
            vertices[2, 0, 1].IsLocked = false;
            vertices[2, 0, 2].IsLocked = false;
            vertices[4, 0, 4].IsLocked = false;
            vertices[1, 0, 1].IsGoal = true;
            vertices[1, 0, 2].IsGoal = true;
            vertices[2, 0, 1].IsGoal = true;
            vertices[2, 0, 2].IsGoal = true;
            vertices[4, 0, 4].IsStart = true;
            Vertex start = vertices[4, 0, 4];
            Vertex goal = vertices[1, 0, 0];
            
            var result = algorithm.FindVertex(start, goal);
            
            Assert.That(result.Position, Is.EqualTo(new Vector3Int(2,0,2)));
        }
    }
}