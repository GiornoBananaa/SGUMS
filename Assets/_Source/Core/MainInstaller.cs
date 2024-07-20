using InputSystem;
using OrderSystem;
using SelectionSystem;
using SelectionSystem.AreaSelectionSystem;
using TeamSystem;
using UnitCombatSystem;
using UnitFormationSystem;
using UnitGroupingSystem;
using UnitSystem;
using UnitSystem.MovementSystem;
using UnitSystem.UnitFactories;
using UnityEngine;
using Zenject;

namespace Core
{
    public class MainInstaller : MonoInstaller
    {
        private const string UNIT_SELECTION_DATA_PATH = "UnitSelectionData";
        private const string ORDER_PANEL_DATA_PATH = "OrdersPanelData";
        private const string FORMATION_SETTING_DATA_PATH = "FormationData";
        private const string PATH_DATA_PATH = "PathDataSO";
        private const string ENEMY_DETECTION_DATA_PATH = "EnemyDetectionData";
        private const string TEAMS_DATA_PATH = "TeamsData";
        private const string UNITS_DATA_PATH = "UnitsData";
        [SerializeField] private InputListener _inputListener;
        [SerializeField] private GUIAreaSelectionView _areaSelectionView;
        [SerializeField] private Unit[] _testUnits;
        [SerializeField] private GroupEmblemView _groupEmblemPrefab;
        [SerializeField] private RectTransform _groupEmblemParent;
        
        public override void InstallBindings()
        {
            //SO
            UnitSelectionDataSO unitSelectionData = Resources.Load<UnitSelectionDataSO>(UNIT_SELECTION_DATA_PATH);
            Container.Bind<UnitSelectionDataSO>().FromInstance(unitSelectionData).AsSingle();
            OrderPanelDataSO ordersPanelData = Resources.Load<OrderPanelDataSO>(ORDER_PANEL_DATA_PATH);
            Container.Bind<OrderPanelDataSO>().FromInstance(ordersPanelData).AsSingle();
            PathDataSO pathData = Resources.Load<PathDataSO>(PATH_DATA_PATH);
            Container.Bind<PathDataSO>().FromInstance(pathData).AsSingle();
            FormationSettingDataSO formationSettingData = Resources.Load<FormationSettingDataSO>(FORMATION_SETTING_DATA_PATH);
            Container.Bind<FormationSettingDataSO>().FromInstance(formationSettingData).AsSingle();
            EnemyDetectionDataSO enemyDetectionData = Resources.Load<EnemyDetectionDataSO>(ENEMY_DETECTION_DATA_PATH);
            Container.Bind<EnemyDetectionDataSO>().FromInstance(enemyDetectionData).AsSingle();
            TeamsDataSO teamsData = Resources.Load<TeamsDataSO>(TEAMS_DATA_PATH);
            Container.Bind<TeamsDataSO>().FromInstance(teamsData).AsSingle();
            UnitsDataSO unitsData = Resources.Load<UnitsDataSO>(UNITS_DATA_PATH);
            Container.Bind<UnitsDataSO>().FromInstance(unitsData).AsSingle();
            //Core
            Container.Bind<UpdateTimer>().AsTransient();
            Container.BindInterfacesAndSelfTo<ServiceUpdater>().AsSingle().NonLazy();
            //Order
            Container.Bind<IOrder>().To<SquadFormOrder>().AsSingle();
            Container.Bind<IOrder>().To<SquadDisbandOrder>().AsSingle();
            Container.Bind<IOrder>().To<FormationDrawOrder>().AsSingle();
            Container.Bind<IOrder>().To<PathCancelOrder>().AsSingle();
            Container.Bind<OrderContainer>().AsSingle();
            //Unit
            Container.Bind<UnitContainer>().AsSingle().WithArguments(_testUnits);
            Container.Bind<MeleeUnitFactory>().AsSingle();
            //Input
            Container.Bind<InputListener>().FromInstance(_inputListener).AsSingle();
            //Movement
            Container.Bind<UnitMover>().AsSingle().NonLazy();
            Container.Bind<PathCreator>().AsSingle();
            Container.Bind<PathDrawer>().AsSingle();
            Container.Bind<PathContainer>().AsSingle();
            Container.Bind<FormationSetter>().AsSingle();
            Container.Bind<FormationDrawer>().AsSingle();
            Container.Bind<FormationPlacer>().AsSingle();
            Container.Bind<GroupSpeedEqualizer>().AsSingle();
            //Combat
            Container.Bind<EnemyDetectionUpdater>().AsSingle();
            Container.Bind<MeleeAttack>().AsSingle();
            //Grouping
            Container.Bind<GroupEmblemFactory>().AsSingle().WithArguments(_groupEmblemPrefab, _groupEmblemParent);
            Container.Bind<UnitGroupContainer>().AsSingle();
            Container.Bind<UnitGrouper>().AsSingle();
            //Selection
            Container.Bind<GroupSelection>().AsSingle();
            Container.Bind<UnitSelection>().AsSingle();
            Container.Bind<AreaSelector>().AsSingle();
            Container.Bind<IAreaSelectionView>().FromInstance(_areaSelectionView).AsSingle();
        }
    }
}
