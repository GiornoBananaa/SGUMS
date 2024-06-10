using SelectionSystem;
using UnityEngine;
using Zenject;

namespace UnitGroupingSystem
{
    public class GroupEmblemFactory : IFactory<GroupEmblemView>
    {
        private readonly GroupEmblemView _emblemPrefab;
        private readonly GroupSelection _groupSelection;
        private readonly RectTransform _groupEmblemParent;

        public GroupEmblemFactory(GroupSelection groupSelection, GroupEmblemView emblemPrefab, RectTransform groupEmblemParent)
        {
            _emblemPrefab = emblemPrefab;
            _groupSelection = groupSelection;
            _groupEmblemParent = groupEmblemParent;
        }
        
        public GroupEmblemView Create()
        {
            GroupEmblemView emblemView = Object.Instantiate(_emblemPrefab, _groupEmblemParent, false);
            emblemView.Construct(_groupSelection);
            return emblemView;
        }
    }
}