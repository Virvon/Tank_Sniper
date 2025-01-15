using System;

namespace Assets.Sources.Data
{
    [Serializable]
    public class Wallet
    {
        public uint Value = 100000;

        public event Action ValueChanged;
        
        public bool TryTake(uint value)
        {
            if(value > Value)
                return false;

            Value -= value;
            ValueChanged?.Invoke();

            return true;
        }

        public void Give(uint value)
        {
            Value += value;
        }
    }
}
