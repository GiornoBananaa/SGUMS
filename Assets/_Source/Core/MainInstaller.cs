using UnitSystem;
using UnityEngine;
using Zenject;

namespace Core
{
    public class MainInstaller : MonoInstaller
    {
        private const string UNIT_SELECTION_DATA_PATH = "UnitSelectionData";
        [SerializeField] private GUIAreaSelectionView _areaSelectionView;
        [SerializeField] private Unit[] _testUnits;
        
        public override void InstallBindings()
        {
            //SO
            UnitSelectionDataSO unitSelectionData = Resources.Load<UnitSelectionDataSO>(UNIT_SELECTION_DATA_PATH);
            Container.Bind<UnitSelectionDataSO>().FromInstance(unitSelectionData).AsSingle();
            //
            Container.Bind<UnitSelection>().AsSingle();
            Container.Bind<AreaSelector>().AsSingle();
            Container.Bind<UnitContainer>().AsSingle().WithArguments(_testUnits);
            Container.Bind<IAreaSelectionView>().FromInstance(_areaSelectionView).AsSingle();
        }
    }
}
