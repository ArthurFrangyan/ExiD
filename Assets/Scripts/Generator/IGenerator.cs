using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Generator
{
    public interface IGenerator
    {
        public HashSet<Vector2Int> Generate(int diameter);
    }
}
