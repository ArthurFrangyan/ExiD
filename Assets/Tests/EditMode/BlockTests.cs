using Generator;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode
{
    [TestFixture]
    public class BlockTests
    {
        private Block _blockEmpty;
        private Block _blockEverything;

        [SetUp]
        public void Setup()
        {
            _blockEmpty = Block.Empty;
            _blockEverything = new Block(Block.Property.Everything);
        }
        [Test]
        public void SetWall_RightVector_SetRightWallToTheSameValue()
        {
            _blockEmpty.SetBorder(Vector3Int.right, true);
            _blockEverything.SetBorder(Vector3Int.right, false);
            
            Assert.That(_blockEmpty.HasRightWall, Is.True);
            Assert.That(_blockEverything.HasRightWall, Is.False);
        }
        [Test]
        public void SetWall_LeftVector_SetLeftWallToTheSameValue()
        {
            _blockEmpty.SetBorder(Vector3Int.left, true);
            _blockEverything.SetBorder(Vector3Int.left, false);
            
            Assert.That(_blockEmpty.HasLeftWall, Is.True);
            Assert.That(_blockEverything.HasLeftWall, Is.False);
        }
        [Test]
        public void SetWall_ForwardVector_SetTopWallToTheSameValue()
        {
            _blockEmpty.SetBorder(Vector3Int.forward, true);
            _blockEverything.SetBorder(Vector3Int.forward, false);
            
            Assert.That(_blockEmpty.HasTopWall, Is.True);
            Assert.That(_blockEverything.HasTopWall, Is.False);
        }
        [Test]
        public void SetWall_BackVector_SetBottomWallToTheSameValue()
        {
            _blockEmpty.SetBorder(Vector3Int.back, true);
            _blockEverything.SetBorder(Vector3Int.back, false);
            
            Assert.That(_blockEmpty.HasBottomWall, Is.True);
            Assert.That(_blockEverything.HasBottomWall, Is.False);
        }
        [Test]
        public void SetWall_OtherVector_ThrowException()
        {
            Assert.Throws<System.ArgumentException>(() => _blockEmpty.SetBorder(new Vector3Int(1,0,1), true));
        }
    }
}