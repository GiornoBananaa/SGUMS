using UnityEngine;

namespace UnitSystem
{
    public class Unit : MonoBehaviour,ISelectable
    {
        [field: SerializeField] public Projector SelectionProjector { get; private set; }
    }
}
