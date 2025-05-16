using System.Collections.Generic;
using Generator.Library;
using UnityEngine;
using static UnityEngine.Vector3Int;

namespace Generator
{
    public class Axes
    {
        public static Axes X => YZXAxes();
        public static Axes Y => XZYAxes();
        public static Axes Z => XYZAxes();
        public static Axes XY => XYZAxes();
        public static Axes XZ => XZYAxes();
        public static Axes YZ => YZXAxes();
        public Vector3Int AVec { get; private set; }
        public Vector3Int BVec { get; private set; }
        public Vector3Int CVec { get; private set; }

        private static Axes XYZAxes() => new(right, up, forward);
        private static Axes XZYAxes() => new(right, forward, up);
        private static Axes YZXAxes() => new(up, forward, right);
        private static Axes YXZAxes() => new(up, right, forward);
        private static Axes ZXYAxes() => new(forward, right, up);
        private static Axes ZYXAxes() => new(forward, up, right);

        public Axes(Vector3Int aVec, Vector3Int bVec, Vector3Int cVec)
        {
            AVec = aVec;
            BVec = bVec;
            CVec = cVec;
        }

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
}