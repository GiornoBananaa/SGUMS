using SelectionSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UnitGroupingSystem
{
    public class GroupEmblemView : MonoBehaviour, IPointerDownHandler
    {
        private Group _group;
        private UnitSelection _unitSelection;
        
        [Inject]
        public void Construct(UnitSelection unitSelection)
        {
            _unitSelection = unitSelection;
        }

        public void SetGroup(Group group)
        {
            _group = group;
        }
        
        private void FixedUpdate()
        {
            if(_group == null) return;
            MoveGroupEmblem();
        }

        private void MoveGroupEmblem()
        {
            transform.position = _group.GroupCenter;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _unitSelection.Select(_group.Units);
        }
    }
}