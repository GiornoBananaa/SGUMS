using System.Threading.Tasks;
using Den.Tools;
using MapMagic.Core;
using Unity.AI.Navigation;
using Random = UnityEngine.Random;

namespace LandscapeSystem
{
    public class LandscapeGenerator
    {
        private IMapMagic _magicObject;
        private NavMeshSurface _navMeshSurface;
        private TerrainDataLoader _terrainDataLoader;
        private TerrainObjectGenerator[] _objectGenerators;
        
        public LandscapeGenerator(NavMeshSurface navMeshSurface, IMapMagic magicObject, TerrainDataLoader terrainDataLoader, LandscapeDataSO landscapeData)
        {
            _magicObject = magicObject;
            _navMeshSurface = navMeshSurface;
            _terrainDataLoader = terrainDataLoader;
            _objectGenerators = landscapeData.TerrainObjectGenerators;
        }

        public async Task GenerateTerrain()
        {
            _magicObject.Graph.random = new Noise(Random.Range(0,999999));
            _magicObject.Refresh(true);
            while (_magicObject.IsGenerating())
            {
                await Task.Yield();
            }
            _terrainDataLoader.LoadTerrainProps();
        }
        
        public void GenerateTerrainObjects()
        {
            foreach (var objectGenerator in _objectGenerators)
            {
                objectGenerator.GenerateObjects(_terrainDataLoader);
            }
        }
        
        public void BuildNavMesh()
        {
            _navMeshSurface.BuildNavMesh();
        }
    }
}
