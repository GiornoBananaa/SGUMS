using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnitSystem
{
    [CreateAssetMenu(fileName = "UnitsData", menuName = "SO/UnitsData")]
    public class UnitsDataSO : ScriptableObject
    {
        
        [SerializeField] private UnitData[] _units;
        private Dictionary<UnitType, UnitData> _unitsByUnitType;
        
        public Dictionary<UnitType, UnitData> UnitsByUnitType
        {
            get
            {
                if (_unitsByUnitType == null)
                {
                    _unitsByUnitType = new Dictionary<UnitType, UnitData>();
                    _unitsByUnitType = _units.ToDictionary((c)=>c.UnitType);
                }

                return _unitsByUnitType;
            }
        }
    }
}