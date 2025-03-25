using System;

namespace Generator
{
    public struct Block
    {
        private Property _data;

        [Flags]
        public enum Property : byte
        {
            None            = 0,
            HasFloor        = 1 << 0,
            HasCeil         = 1 << 1,
            HasTopWall      = 1 << 2,
            HasBottomWall   = 1 << 3,
            HasLeftWall     = 1 << 4,
            HasRightWall    = 1 << 5,
            IsLocked        = 1 << 6
        }
        public Block(Property property = Property.None)
        {
            _data = property;
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
            get => (_data & Property.IsLocked) != 0;
            set => _data = value ? _data | Property.IsLocked : _data & ~Property.IsLocked;
        }
        public bool HasWall => (_data & (Property.HasLeftWall | Property.HasRightWall | Property.HasTopWall | Property.HasBottomWall)) != 0;
    }
}
