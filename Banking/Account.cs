using System;
using System.Text;

namespace SimpleBankingSystem_CSharp
{
    internal class Account
    {
        private static readonly Random Random = new();
        internal string CardNumber { get; set; }
        internal string Pin { get; set; }
        private int _balance;
        internal int Balance
        {
            get => _balance;
            set => _balance += value;
        }

        internal Account() 
        {
            CardNumber = GenerateCardNumber();
            Pin = GeneratePin();
            Balance = 0;
        }

        internal static int LuhnAlgorithmChecksum(string cardNumber) 
        {

            int end = cardNumber.Length == 16 ? cardNumber.Length - 1 : cardNumber.Length;

            int luhnSum = 0;
            for (int i = 0; i < end; i++) 
            {
                int digit = (int) Char.GetNumericValue(cardNumber[i]);

                if (i % 2 == 0) {
                    digit = (2 * digit > 9) ? (2 * digit - 9) : 2 * digit;
                }

                luhnSum += digit;
            }

            if (cardNumber.Length == 16) 
            {
                luhnSum += (int) Char.GetNumericValue(cardNumber[^1]);
            }

            return (10 - (luhnSum % 10)) % 10;

        }
        private string GenerateCardNumber() 
        {
            StringBuilder cardNumber = new StringBuilder("400000");

            string accountIdentifier = $"{Random.Next(1_000_000_000):D9}";
            cardNumber.Append(accountIdentifier);

            int lastDigit = LuhnAlgorithmChecksum(cardNumber.ToString());
            cardNumber.Append(lastDigit);

            return cardNumber.ToString();
        }

        private string GeneratePin() 
        {
            int pin = Random.Next(10_000);

            return $"{pin:D4}";
        }
    }
}