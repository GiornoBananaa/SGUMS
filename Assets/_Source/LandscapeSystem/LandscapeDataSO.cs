using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace LandscapeSystem
{
    [CreateAssetMenu(fileName = "LandscapeData", menuName = "SO/LandscapeData")]
    public class LandscapeDataSO : ScriptableObject
    {
        private Dictionary<int, LandscapePartData> _landscapesByIndex;
        
        [field: SerializeField] public LandscapePartData[] Landscapes { get; private set; }
        [field: SerializeField] public TerrainObjectGenerator[] TerrainObjectGenerators { get; private set; }
        
        public Dictionary<int, LandscapePartData> LandscapesByIndex  
            => _landscapesByIndex ??= Landscapes.ToDictionary(l => l.Id);
    }
}