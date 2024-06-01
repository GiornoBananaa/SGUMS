﻿using UnityEngine;

namespace UnitSystem
{
    [CreateAssetMenu(fileName = "UnitSelectionData",menuName = "SO/UnitSelectionData")]
    public class UnitSelectionDataSO : ScriptableObject
    {
        [field: SerializeField] public LayerMask LayersOfAreaSelectionRayCastTarget { get; private set; }
    }
}