using System.Collections.Generic;
using UnityEngine;

namespace UnitFormationSystem
{
    public class Formation
    {
        public List<Vector2> Positions;
        public Vector2[] Bounds;
        public Vector2 Size;
        
        public Formation(List<Vector2> positions, Vector2[] bounds, Vector2 size)
        {
            Positions = positions;
            Bounds = bounds;
            Size = size;
        }
    }
}