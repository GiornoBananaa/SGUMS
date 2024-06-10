using SelectionSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UnitGroupingSystem
{
    public class GroupEmblemView : MonoBehaviour, IPointerDownHandler
    {
        private Group _group;
        private GroupSelection _groupSelection;
        
        [Inject]
        public void Construct(GroupSelection unitSelection)
        {
            _groupSelection = unitSelection;
        }

        public void SetGroup(Group group)
        {
            _group = group;
            _group.OnDisband += RemoveEmblem;
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

        private void RemoveEmblem()
        {
            //TODO: Group Emblem Pool
            Destroy(gameObject);
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _groupSelection.Select(_group);
        }
    }
}