using UnityEngine;

namespace UnitFormationSystem
{
    [CreateAssetMenu(fileName = "FormationData", menuName = "SO/FormationDataSO")]
    public class FormationSettingDataSO : ScriptableObject
    {
        [field: SerializeField] public Material FormationDrawerMaterial { get; private set; }
    }
}