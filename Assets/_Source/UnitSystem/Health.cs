using System;

namespace UnitSystem
{
    public class Health
    {
        private int _hp;
        private int _maxHP;
        
        public int HP
        {
            get => _hp;
            set
            {
                if (IsDead && value > 0)
                {
                    _hp = value;
                    OnRevive?.Invoke(_hp);
                }
                else if (!IsDead && value <= 0)
                {
                    _hp = 0;
                    OnDeath?.Invoke();
                }
                else
                {
                    _hp = value;
                }
            }
        }

        public int MaxHP => _maxHP;
        public bool IsDead => _hp <= 0;

        public event Action OnDeath;
        public event Action<int> OnRevive;
        
        public Health(int maxHP)
        {
            HP = maxHP;
            _maxHP = maxHP;
        }
    }
}