using Assets.Scripts.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Data
{
    static class Info
    {
        public static IGenerator RoomAreaGenerator = new RandomWalkAreaGenerator();
        public static int CellSize = 4;
    }
}
