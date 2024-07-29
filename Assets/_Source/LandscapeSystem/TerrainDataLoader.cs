using UnityEngine;

namespace LandscapeSystem
{
    public class TerrainDataLoader : MonoBehaviour
    {
        private int _alphamapWidth;
        private int _alphamapHeight;
        private int _numTextures;
        private float[,,] _mSplatmapData;
        
        public TerrainData TerrainData { get; private set; }
        public GameObject TerrainGameObject { get; private set; }
        
        public void LoadTerrainProps() 
        {
            var activeTerrain = Terrain.activeTerrain;
            TerrainData = activeTerrain.terrainData;
            _alphamapWidth = TerrainData.alphamapWidth;
            _alphamapHeight = TerrainData.alphamapHeight;
            TerrainGameObject = activeTerrain.gameObject;
            _mSplatmapData = TerrainData.GetAlphamaps(0, 0, _alphamapWidth, _alphamapHeight);
            _numTextures = _mSplatmapData.Length / (_alphamapWidth * _alphamapHeight);
        }
        
        public int GetTerrainAtPosition(Vector3 pos)
        {
            int terrainIdx = GetActiveTerrainTextureIdx(pos);
            return terrainIdx;
        }
        
        private int GetActiveTerrainTextureIdx(Vector3 pos)
        {
            Vector3 terrainCord = ConvertToSplatMapCoordinate(pos);
            int ret = 0;
            float comp = 0f;
            for (int i = 0; i < _numTextures; i++)
            {
                if (comp < _mSplatmapData[(int)terrainCord.z, (int)terrainCord.x, i])
                {
                    comp = _mSplatmapData[(int)terrainCord.z, (int)terrainCord.x, i];
                    ret = i;
                }
            }
            return ret;
        }
        
        private Vector3 ConvertToSplatMapCoordinate(Vector3 playerPos)
        {
            Vector3 vecRet = new Vector3();
            Terrain ter = Terrain.activeTerrain;
            Vector3 terPosition = ter.transform.position;
            vecRet.x = ((playerPos.x - terPosition.x) / ter.terrainData.size.x) * ter.terrainData.alphamapWidth;
            vecRet.z = ((playerPos.z - terPosition.z) / ter.terrainData.size.z) * ter.terrainData.alphamapHeight;
            return vecRet;
        }
    }
}
