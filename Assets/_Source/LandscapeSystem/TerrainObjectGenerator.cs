using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace LandscapeSystem
{
    [Serializable]
    public class TerrainObjectGenerator
    {
        [SerializeField] private TerrainObject[] _terrainObjects;
        [SerializeField] private int _terrainLayerIndex;
        [Range(0,1f)] [SerializeField] private float _frequency = 0.5f;
        [Range(0,1f)] [SerializeField] private float _spread = 1;
        [SerializeField] private float _spacing = 3;
        [SerializeField] private bool _noise;
        [SerializeField] private float _noiseSize = 0.1f;
        
        public void GenerateObjects(TerrainDataLoader terrainDataLoader)
        {
            TerrainData terrainData = terrainDataLoader.TerrainData;
            for (float i = 0; i < terrainData.size.x; i += _spacing)
            {
                for (float j = 0; j < terrainData.size.z; j += _spacing)
                {
                    float x = i + Random.Range(-_spacing/2, _spacing/2) * _spread;
                    float y = j + Random.Range(-_spacing/2, _spacing/2) * _spread;
                    float sample = _noise ? Mathf.PerlinNoise(x * _noiseSize, y * _noiseSize) : 1;
                    if(x > terrainData.size.x || y > terrainData.size.z || x < 0 || y < 0)
                        continue;
                    if (sample >= 1 - _frequency 
                        && terrainDataLoader.GetTerrainAtPosition(new Vector3(x, 0, y)) == _terrainLayerIndex)
                    {
                        SpawnObject(new Vector3(x, y), terrainDataLoader.TerrainGameObject);
                    }
                }
            }
        }

        private TerrainObject GetRandomTerrainObjectPrefab()
        {
            int chanceSum = 0;
            foreach (var terrainObject in _terrainObjects)
            {
                chanceSum += terrainObject.SpawnChance;
            }
            
            int rndChance = Random.Range(1, chanceSum+1);
            int chance = 0;
            for (int i = 0; i < _terrainObjects.Length; i++)
            {
                chance += _terrainObjects[i].SpawnChance;
                if (chance >= rndChance)
                    return _terrainObjects[i];
            }
            
            return _terrainObjects[^1];
        }
        
        private void SpawnObject(Vector2 position, GameObject terrainGameObject)
        {
            TerrainObject obj = GetRandomTerrainObjectPrefab();
            
            if (!Physics.Raycast(new Vector3(position.x, 200, position.y), Vector3.down, 
                    out RaycastHit hit, 1000, 1 << terrainGameObject.layer))
                return;
            
            Object.Instantiate(obj.Prefab, hit.point, Quaternion.Euler(0, Random.Range(0, 360), 0),
                terrainGameObject.transform);
        }
    }
}