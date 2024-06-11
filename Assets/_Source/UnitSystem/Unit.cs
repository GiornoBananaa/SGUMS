using OrderSystem;
using SelectionSystem;
using UnitGroupingSystem;
using UnityEngine;
using UnityEngine.AI;

namespace UnitSystem
{
    public class Unit : MonoBehaviour,ISelectable
    {
        [field: SerializeField] public Projector SelectionProjector { get; private set; }
        [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
        public Group UnitGroup { get; set; }
    }
}
