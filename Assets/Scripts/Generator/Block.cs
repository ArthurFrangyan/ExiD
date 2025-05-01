using System;

namespace Generator
{
    public struct Block : IEquatable<Block>
    {
        private Property _data;

        [Flags]
        public enum Property : short
        {
            None            = 0,
            HasFloor        = 1 << 0,
            HasCeil         = 1 << 1,
            HasTopWall      = 1 << 2,
            HasBottomWall   = 1 << 3,
            HasLeftWall     = 1 << 4,
            HasRightWall    = 1 << 5,
            Locked          = 1 << 6,
            
            Height0         = 0,
            Height1         = 1 << 7,
            Height2         = 2 << 7,
            Height3         = 3 << 7,
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
        public bool HasFloor
        {
            get => (_data & Property.HasFloor) != 0;
            set => _data = value ? _data | Property.HasFloor : _data & ~Property.HasFloor;
        }
        public bool HasCeil
        {
            get => (_data & Property.HasCeil) != 0;
            set => _data = value ? _data | Property.HasCeil : _data & ~Property.HasCeil;
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
