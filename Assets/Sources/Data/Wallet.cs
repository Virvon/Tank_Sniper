using System;

namespace Assets.Sources.Data
{
    [Serializable]
    public class Wallet
    {
        public uint Value;

        public event Action ValueChanged;

        public Wallet()
        {
            Value = 9999990;
        }
        
        public bool TryTake(uint value)
        {
            if(value > Value)
                return false;

            Value -= value;

            return true;
        }
    }
}
