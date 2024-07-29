using System;
using UnityEngine;

namespace LandscapeSystem
{
    [Serializable]
    public class TerrainObject
    {
        [field: SerializeField] public GameObject Prefab { get; private set; }
        [field: SerializeField] public int SpawnChance { get; private set; }
    }
}