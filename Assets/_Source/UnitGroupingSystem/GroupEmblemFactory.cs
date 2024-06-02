using SelectionSystem;
using UnityEngine;
using Zenject;

namespace UnitGroupingSystem
{
    public class GroupEmblemFactory : IFactory<GroupEmblemView>
    {
        private readonly GroupEmblemView _emblemPrefab;
        private readonly UnitSelection _unitSelection;
        private readonly RectTransform _groupEmblemParent;

        public GroupEmblemFactory(UnitSelection unitSelection, GroupEmblemView emblemPrefab, RectTransform groupEmblemParent)
        {
            _emblemPrefab = emblemPrefab;
            _unitSelection = unitSelection;
            _groupEmblemParent = groupEmblemParent;
        }
        
        public GroupEmblemView Create()
        {
            GroupEmblemView emblemView = Object.Instantiate(_emblemPrefab, _groupEmblemParent, false);
            emblemView.Construct(_unitSelection);
            return emblemView;
        }
    }
}