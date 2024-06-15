using UnityEngine;

namespace UnitSystem.MovementSystem
{
    [CreateAssetMenu(fileName = "PathDataSO", menuName = "SO/PathData")]
    public class PathDataSO: ScriptableObject
    {
        [field: SerializeField] public Material PathMaterial { get; private set; }
        [field: SerializeField] public Material EndPointMaterial { get; private set; }
        [field: SerializeField] public LayerMask ProjectorIgnoreLayers { get; private set; }
    }
}