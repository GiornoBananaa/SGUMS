using OrderSystem;
using SelectionSystem;
using SelectionSystem.AreaSelectionSystem;
using UnitGroupingSystem;
using UnitSystem;
using UnitSystem.MovementSystem;
using UnityEngine;
using Zenject;

namespace Core
{
    public class MainInstaller : MonoInstaller
    {
        private const string UNIT_SELECTION_DATA_PATH = "UnitSelectionData";
        private const string ORDER_PANEL_DATA_PATH = "OrdersPanelData";
        private const string PATH_DATA_PATH = "PathDataSO";
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
            //Order
            Container.Bind<IOrder>().To<SquadFormOrder>().AsSingle();
            Container.Bind<IOrder>().To<SquadDisbandOrder>().AsSingle();
            Container.Bind<IOrder>().To<FormationEnterOrder>().AsSingle();
            Container.Bind<IOrder>().To<PathCancelOrder>().AsSingle();
            Container.Bind<OrderContainer>().AsSingle();
            //Unit
            Container.Bind<UnitContainer>().AsSingle().WithArguments(_testUnits);
            Container.Bind<UnitMover>().AsSingle();
            Container.Bind<PathCreator>().AsSingle();
            Container.Bind<PathDrawer>().AsSingle();
            Container.Bind<PathContainer>().AsSingle();
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
