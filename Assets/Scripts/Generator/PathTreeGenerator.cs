using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Generator
{
    class PathTreeGenerator
    {
        private HashSet<Vector3> _paths;

        public void GeneratePaths(RoomGraph roomGraph)
        {

        }
        private void GeneratePathBetween(Room first, Room second)
        {
            Vector3 startPosition = first.Position;
            Vector3 destinityPosition = second.Position;

            
        }
    }
}
