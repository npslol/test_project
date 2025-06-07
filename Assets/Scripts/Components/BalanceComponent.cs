using UnityEngine;

namespace Client 
{
    struct BalanceComponent 
    {
        public int Value;

        public void Add(int value)
        {
            Value += value;
            Value = Mathf.Max(Value, 0);
            PlayerObserver.BalanceChange(Value);
        }

        public bool Spend(int value)
        {
            if (value <= Value)
            {
                Value -= value;
                Value = Mathf.Max(Value, 0);
                PlayerObserver.BalanceChange(Value);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}