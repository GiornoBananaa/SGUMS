namespace UnitSystem
{
    public class UnitLifeTimeController
    {
        private readonly Unit _unit;

        public UnitLifeTimeController(Unit unit)
        {
            _unit = unit;
            _unit.Health.OnDeath += Die;
            _unit.Health.OnRevive += Revive;
        }
        
        private void Die()
        {
            UnityEngine.Object.Destroy(_unit);
        }
        
        private void Revive(int hp)
        {
            
        }
    }
}