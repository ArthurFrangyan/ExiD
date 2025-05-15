using System;
using UnityEngine;

namespace Generator
{
    public struct Block : IEquatable<Block>
    {
        private Property _data;

        [Flags]
        public enum Property : short
        {
            None            = 0,
            Everything      = ~0,
            
            HasFloor        = 1 << 0,
            HasRoof         = 1 << 1,
            HasTopWall      = 1 << 2,
            HasBottomWall   = 1 << 3,
            HasLeftWall     = 1 << 4,
            HasRightWall    = 1 << 5,
            
            Locked          = 1 << 6,
            
            Height0         = 0,
            Height1         = 1 << 7,
            Height2         = 2 << 7,
            Height3         = 3 << 7,
            
            HasBottomLeftColumn = 1 << 9,
            HasFloorLeftStoneCorner = 1 << 10,
            HasFloorBottomStoneCorner = 1 << 11,
            
            ConnectedToRoof = 1 << 12,
            ConnectedToFloor = 1 << 13,
        }

        public static Block Empty = new Block { _data = Property.None};
        public Block(Property property = Property.None)
        {
            _data = property;
        }

        public Block(Block block)
        {
            _data = block._data;
        }

        public bool ConnectedToRoof
        {
            get => (_data & Property.ConnectedToRoof) != 0;
            set => _data = value ? _data | Property.ConnectedToRoof : _data & ~Property.ConnectedToRoof;
        }

        public bool ConnectedToFloor
        {
            get => (_data & Property.ConnectedToFloor) != 0;
            set => _data = value ? _data | Property.ConnectedToFloor : _data & ~Property.ConnectedToFloor;
        }

        public bool HasBottomLeftColumn
        {
            get => (_data & Property.HasBottomLeftColumn) != 0;
            set => _data = value ? _data | Property.HasBottomLeftColumn : _data & ~Property.HasBottomLeftColumn;
        }

        public bool HasFloorLeftStoneCorner
        {
            get => (_data & Property.HasFloorLeftStoneCorner) != 0;
            set => _data = value ? _data | Property.HasFloorLeftStoneCorner : _data & ~Property.HasFloorLeftStoneCorner;
        }

        public bool HasFloorBottomStoneCorner
        {
            get => (_data & Property.HasFloorBottomStoneCorner) != 0;
            set => _data = value ? _data | Property.HasFloorBottomStoneCorner : _data & ~Property.HasFloorBottomStoneCorner;
        }

        public bool HasFloor
        {
            get => (_data & Property.HasFloor) != 0;
            set => _data = value ? _data | Property.HasFloor : _data & ~Property.HasFloor;
        }
        public bool HasRoof
        {
            get => (_data & Property.HasRoof) != 0;
            set => _data = value ? _data | Property.HasRoof : _data & ~Property.HasRoof;
        }
        public bool HasTopWall
        {
            get => (_data & Property.HasTopWall) != 0;
            set => _data = value ? _data | Property.HasTopWall : _data & ~Property.HasTopWall;
        }
        public bool HasBottomWall
        {
            get => (_data & Property.HasBottomWall) != 0;
            set => _data = value ? _data | Property.HasBottomWall : _data & ~Property.HasBottomWall;
        }
        public bool HasLeftWall
        {
            get => (_data & Property.HasLeftWall) != 0;
            set => _data = value ? _data | Property.HasLeftWall : _data & ~Property.HasLeftWall;
        }
        public bool HasRightWall
        {
            get => (_data & Property.HasRightWall) != 0;
            set => _data = value ? _data | Property.HasRightWall : _data & ~Property.HasRightWall;
        }
        public bool IsLocked
        {
            get => (_data & Property.Locked) != 0;
            set => _data = value ? _data | Property.Locked : _data & ~Property.Locked;
        }
        public Block AsLocked() => new(_data | Property.Locked);
        public bool HasWall => (_data & (Property.HasLeftWall | Property.HasRightWall | Property.HasTopWall | Property.HasBottomWall)) != 0;
        
        public bool HasValue => _data != Property.None;
        
        public int Height
        {
            get => (int)(_data & Property.Height3) >> 7;
            set => _data = (_data & ~Property.Height3) | (Property)((value & 3) << 7);
        }

        public void SetWall(Vector3Int direction, bool value)
        {
            if (direction == Vector3Int.left)
                HasLeftWall = value;
            else if (direction == Vector3Int.right)
                HasRightWall = value;
            else if (direction == Vector3Int.forward)
                HasTopWall = value;
            else if (direction == Vector3Int.back)
                HasBottomWall = value;
            else if (direction == Vector3Int.down)
                HasFloor = value;
            else if (direction == Vector3Int.up)
                HasRoof = value;
            else
                throw new ArgumentException();
        }
        public bool GetWall(Vector3Int direction)
        {
            if (direction == Vector3Int.left)
                return HasLeftWall;
            if (direction == Vector3Int.right)
                return HasRightWall;
            if (direction == Vector3Int.forward)
                return HasTopWall;
            if (direction == Vector3Int.back)
                return HasBottomWall;
            if (direction == Vector3Int.down)
                return HasFloor;
            if (direction == Vector3Int.up)
                return HasRoof;
            
            throw new ArgumentException();
        }
        
        public static void SetBlockLeftWall(ref Block block, bool value) => block.HasLeftWall = value;
        public static void SetBlockRightWall(ref Block block, bool value) => block.HasRightWall = value;
        public static void SetBlockBottomWall(ref Block block, bool value) => block.HasBottomWall = value;
        public static void SetBlockTopWall(ref Block block, bool value) => block.HasTopWall = value;
        public static void SetBlockFloor(ref Block block, bool value) => block.HasFloor = value;
        public static void SetBlockRoof(ref Block block, bool value) => block.HasRoof = value;

        public void SetConnected(Vector3Int direction, bool value)
        {
            if (direction == Vector3Int.down)
                ConnectedToFloor = value;
            else if (direction == Vector3Int.up)
                ConnectedToRoof = value;
            else
                throw new ArgumentException();
        }

        public bool GetConnected(Vector3Int direction)
        {
            if (direction == Vector3Int.down)
                return ConnectedToFloor;
            if (direction == Vector3Int.up)
                return ConnectedToRoof;
            
            throw new ArgumentException();
        }
        public override int GetHashCode()
        {
            return (int)_data;
        }

        public override bool Equals(object obj)
        {
            return obj is Block other && Equals(other);
        }

        public override string ToString()
        {
            return $"({_data.ToString()})";
        }

        public bool Equals(Block other)
        {
            return _data == other._data;
        }

        public static bool operator ==(Block left, Block right)
        {
            return left.Equals(right);;
        }

        public static bool operator !=(Block left, Block right)
        {
            return !(left == right);
        }
    }
}
