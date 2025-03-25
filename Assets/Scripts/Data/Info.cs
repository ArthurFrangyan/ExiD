using Assets.Scripts.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generator;

namespace Assets.Scripts.Data
{
    static class Info
    {
        public static RandomWalkAreaGenerator RoomAreaGenerator = new RandomWalkAreaGenerator();
        public static int CellSize = 4;
    }
}
