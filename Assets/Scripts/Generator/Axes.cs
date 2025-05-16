using System.Collections.Generic;
using Generator.Library;
using UnityEngine;
using static UnityEngine.Vector3Int;

namespace Generator
{
    public abstract class Axes
    {
        public static Axes X => new YZXAxes();
        public static Axes Y => new XZYAxes();
        public static Axes Z => new XYZAxes();
        public static Axes XY => new XYZAxes();
        public static Axes XZ => new XZYAxes();
        public static Axes YZ => new YZXAxes();
        public Vector3Int AVec { get; protected set; }
        public Vector3Int BVec { get; protected set; }
        public Vector3Int CVec { get; protected set; }

        public IEnumerable<Position> A(Position start, Position end)
        {
            for (var pos = start; pos < end; pos += AVec)
                yield return pos;
        }

        public IEnumerable<Position> B(Position start, Position end)
        {
            for (var pos = start; pos < end; pos += BVec)
                yield return pos;
        }

        public IEnumerable<Position> C(Position start, Position end)
        {
            for (var pos = start; pos < end; pos += CVec)
                yield return pos;
        }

        public IEnumerable<Position> AB(Position start, Position end)
        {
            for (var aPos = start; aPos < end; aPos += AVec)
            for (var abPos = aPos; abPos < end; abPos += BVec)
            {
                yield return abPos;
            }
        }
        public IEnumerable<Position> BC(Position start, Position end)
        {
            for (var bPos = start; bPos < end; bPos += BVec)
            for (var bcPos = bPos; bcPos < end; bcPos += CVec)
            {
                yield return bcPos;
            }
        }
        public IEnumerable<Position> AC(Position start, Position end)
        {
            for (var aPos = start; aPos < end; aPos += AVec)
            for (var acPos = aPos; acPos < end; acPos += CVec)
            {
                yield return acPos;
            }
        }

        public IEnumerable<Position> ABC(Position start, Position end)
        {
            for (var aPos = start; aPos < end; aPos += AVec)
            for (var abPos = aPos; abPos < end; abPos += BVec)
            for (var abcPos = abPos; abcPos < end; abcPos += CVec)
            {
                yield return abcPos;
            }
        }
    }

    public class XYZAxes : Axes
    {
        public XYZAxes()
        {
            AVec = right;
            BVec = up;
            CVec = forward;
        }
    }
    public class XZYAxes : Axes
    {
        public XZYAxes()
        {
            AVec = right;
            BVec = forward;
            CVec = up;
        }
    }
    public class YZXAxes : Axes
    {
        public YZXAxes()
        {
            AVec =  up;
            BVec =  forward;
            CVec =  right;
        }
    }
    public class YXZAxes : Axes
    {
        public YXZAxes()
        {
            AVec =  up;
            BVec =  right;
            CVec =  forward;
        }
    }

    public class ZYXAxes : Axes
    {
        public ZYXAxes()
        {
            AVec =  forward;
            BVec =  up;
            CVec =  right;
        }
    }
    public class ZXYAxes : Axes
    {
        public ZXYAxes()
        {
            AVec =  forward;
            BVec =  right;
            CVec =  up;
        }
    }
}