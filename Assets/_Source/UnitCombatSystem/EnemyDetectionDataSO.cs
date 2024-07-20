using UnityEngine;

namespace UnitCombatSystem
{
    [CreateAssetMenu(fileName = "EnemyDetectionData", menuName = "SO/EnemyDetectionData")]
    public class EnemyDetectionDataSO : ScriptableObject
    {
        [field: SerializeField] public float EnemyDetectionUpdateTime { get; private set; }
        [field: SerializeField] public LayerMask UnitsLayers { get; private set; }
    }
}