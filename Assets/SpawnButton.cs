using TeamSystem;
using UnitSystem;
using UnitSystem.UnitFactories;
using UnityEngine;
using Zenject;

public class SpawnButton : MonoBehaviour
{
    private MeleeUnitFactory _meleeUnitFactory;
    [Inject]
    public void Construct(MeleeUnitFactory meleeUnitFactory)
    {
        _meleeUnitFactory = meleeUnitFactory;
    }
    
    public void SpawnMelee()
    {
        _meleeUnitFactory.Create(Vector3.zero, TeamColor.Blue);
    }
}