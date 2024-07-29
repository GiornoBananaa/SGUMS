using System.Collections.Generic;
using Core;
using LandscapeSystem;
using UnityEngine;

namespace UnitSystem.UnitModifierSystem
{
    public class TerrainModifierSetter : IUpdatable
    {
        private readonly UnitContainer _unitContainer;
        private readonly TerrainDataLoader _terrainDataLoader;
        private readonly Dictionary<int, LandscapePartData> _statsModifiers;
        
        public TerrainModifierSetter(UnitContainer unitContainer, TerrainDataLoader terrainDataLoader, LandscapeDataSO landscapeDataSO, ServiceUpdater updater)
        {
            _unitContainer = unitContainer;
            _terrainDataLoader = terrainDataLoader;
            _statsModifiers = landscapeDataSO.LandscapesByIndex;
            updater.Subscribe(this);
        }
        
        public void Update()
        {
            foreach (var unit in _unitContainer.AllUnits)
            {
                CheckUnitsTerrain(unit);
            }
        }
        
        private void CheckUnitsTerrain(Unit unit)
        {
            int terrain = _terrainDataLoader.GetTerrainAtPosition(unit.transform.position);
            if (terrain != unit.TerrainUnderUnit)
            {
                SetModifier(unit, unit.TerrainUnderUnit, terrain);
                unit.TerrainUnderUnit = terrain;
            }
        }
        
        private void SetModifier(Unit unit, int lastTerrain, int newTerrain)
        {
            _statsModifiers[lastTerrain].StatsModifier.RemoveModifier(unit);
            _statsModifiers[newTerrain].StatsModifier.ApplyModifier(unit);
        }
    }
}
