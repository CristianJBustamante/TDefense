using UnityEngine;
using com.Pizia.Saver;

namespace com.Pizia
{
    public class CurrencyManager
    {
        public const int MAXCURRENCY = int.MaxValue;

        public int CurrentCurrency { get; private set; }
        string keyName;

        public CurrencyManager(string customKeyName = "Money")
        {
            keyName = customKeyName;

            if (SaveManager.HasKey(keyName))
            {
                CurrentCurrency = SaveManager.LoadInt(keyName);
            }
            else
            {
                CurrentCurrency = 0;
                Save();
            }
        }

        public void Add(int quantity)
        {
            CurrentCurrency += quantity;

            if (CurrentCurrency > MAXCURRENCY) CurrentCurrency = MAXCURRENCY;
            Save();
        }

        public void Substract(int quantity)
        {
            CurrentCurrency -= quantity;

            if (CurrentCurrency < 0) CurrentCurrency = 0;
            Save();
        }

        void Save() => SaveManager.Save(keyName, CurrentCurrency);
    }
}
