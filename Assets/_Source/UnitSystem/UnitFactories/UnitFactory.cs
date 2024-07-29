using Core;
using SelectionSystem;
using TeamSystem;
using UnitCombatSystem;
using UnitGroupingSystem;
using UnitSystem.MovementSystem;
using UnityEngine;
using Zenject;

namespace UnitSystem.UnitFactories
{
    public abstract class UnitFactory: IFactory<Vector2, TeamColor, Unit>
    {
        private readonly Unit _prefab;
        private readonly UnitMover _unitMover;
        private readonly TeamsDataSO _teamsData;
        private readonly UnitContainer _unitContainer;
        private readonly UnitSelection _unitSelection;
        private readonly UnitGrouper _unitGrouper;
        
        protected readonly UnitData _unitData;
        protected readonly DiContainer Container;

        protected abstract UnitType UnitType { get; }
        private int id = 0;
        
        protected UnitFactory(DiContainer container, UnitContainer unitContainer, UnitSelection unitSelection, 
            UnitMover unitMover, TeamsDataSO teamsData, UnitGrouper unitGrouper, UnitsDataSO unitsDataSO)
        {
            Container = container;
            _unitContainer = unitContainer;
            _unitSelection = unitSelection;
            _unitData = unitsDataSO.UnitsByUnitType[UnitType];
            _prefab = _unitData.Prefab;
            _unitMover = unitMover;
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
            unit.Construct(new Health(_unitData.UnitStats.MaxHP), teamColor, new ModifiableUnitStats(_unitData.UnitStats));
            (IEnemyDetector enemyDetector, IUnitAttack attack) = CreateCombatComponent(unit);
            UpdateTimer attackCooldownTimer = Container.Resolve<UpdateTimer>();
            UpdateTimer attackRangeTimer = Container.Resolve<UpdateTimer>();
            UnitCombat combat = new UnitCombat(unit: unit,attack: attack, unitMover:_unitMover, enemyDetector:enemyDetector,
                attackCooldownTimer: attackCooldownTimer, attackRangeTimer: attackRangeTimer);
            UnitLifeTimeController  unitLifeTimeController = new UnitLifeTimeController(unit, _unitGrouper,_unitSelection, _unitContainer);
            _unitContainer.AllUnits.Add(unit);
            return unit;
        }

        protected abstract (IEnemyDetector, IUnitAttack) CreateCombatComponent(Unit unit);
        protected abstract void CreateOtherComponents(Unit unit);
    }
}