using System.Collections.Generic;
using UnityEngine;

namespace UnitSystem.MovementSystem
{
    public class UnitPath
    {
        public List<Vector3> PathPoints = new();
        public List<Unit> Units = new();
    }
}