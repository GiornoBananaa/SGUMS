using UnityEngine;

namespace SelectionSystem
{
    public interface ISelectable
    {
        Projector SelectionProjector { get; }
    }
}