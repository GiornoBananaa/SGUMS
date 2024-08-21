using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace SelectionSystem
{
    public interface ISelectable
    {
        DecalProjector SelectionProjector { get; }
    }
}