using System;
using UnityEngine;

namespace TeamSystem
{
    [Serializable]
    public class TeamData
    {
        [field: SerializeField] public TeamColor TeamColor { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }
        [field: SerializeField] public int Layer { get; private set; }
    }
}