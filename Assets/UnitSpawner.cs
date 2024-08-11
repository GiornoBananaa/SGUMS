using System.Collections.Generic;
using TeamSystem;
using UnitSystem;
using UnitSystem.UnitFactories;
using UnityEngine;

public class UnitSpawner
{
    private readonly Dictionary<UnitType, UnitFactory> _unitFactories;
    
    public UnitSpawner(IEnumerable<UnitFactory> factories)
    {
        _unitFactories = new Dictionary<UnitType, UnitFactory>();
        foreach (var factory in factories)
        {
            _unitFactories.Add(factory.UnitType, factory);
        }
    }

    public Unit SpawnUnit(Vector3 position, UnitType type, TeamColor team)
    {
        return _unitFactories[type].Create(new Vector2(position.x, position.z), team);
    }
}