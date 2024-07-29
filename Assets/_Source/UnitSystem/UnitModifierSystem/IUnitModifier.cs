namespace UnitSystem.UnitModifierSystem
{
    public interface IUnitModifier
    {
        bool ApplyModifier(Unit unit);
        bool RemoveModifier(Unit unit);
    }
}