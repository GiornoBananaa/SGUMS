using SelectionSystem;
using TeamSystem;
using UnitGroupingSystem;
using UnityEngine;
using Zenject;

namespace UnitSystem.UnitFactories
{
    public abstract class UnitFactory: IFactory<Vector2, TeamColor, Unit>
    {
        private readonly Unit _prefab;
        private readonly TeamsDataSO _teamsData;
        private readonly UnitContainer _unitContainer;
        private readonly UnitSelection _unitSelection;
        private readonly UnitGrouper _unitGrouper;
        private readonly UnitData _unitData;

        public abstract UnitType UnitType { get; }
        private int id = 0;
        
        protected UnitFactory(UnitContainer unitContainer, UnitSelection unitSelection, 
            TeamsDataSO teamsData, UnitGrouper unitGrouper, UnitsDataSO unitsDataSO)
        {
            _unitContainer = unitContainer;
            _unitSelection = unitSelection;
            _unitData = unitsDataSO.UnitsByUnitType[UnitType];
            _prefab = _unitData.Prefab;
            _teamsData = teamsData;
            _unitGrouper = unitGrouper;
        }

        public Unit Create(Vector2 position, TeamColor teamColor)
        {
            Physics.Raycast(new Vector3(position.x, 1000, position.y), Vector3.down, out RaycastHit hit, 2000);
            Vector3 spawnPosition = hit.point;
            Unit unit = Object.Instantiate(_prefab, spawnPosition, Quaternion.identity);
            unit.name += id;
            id++;
            unit.gameObject.layer = _teamsData.TeamByTeamColor[teamColor].Layer;
            unit.Construct(new Health(_unitData.UnitStats.MaxHP), new ModifiableUnitStats(_unitData.UnitStats), teamColor, UnitType);
            UnitLifeTimeController  unitLifeTimeController = new UnitLifeTimeController(unit, _unitGrouper,_unitSelection, _unitContainer);
            unit.GetComponent<MeshRenderer>().material.color = _teamsData.TeamByTeamColor[teamColor].Color;
            _unitContainer.Add(unit);
            return unit;
        }
        
        protected abstract void CreateOtherComponents(Unit unit);
    }
}