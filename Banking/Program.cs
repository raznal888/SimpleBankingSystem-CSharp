using System;
using System.Text.RegularExpressions;

namespace SimpleBankingSystem_CSharp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (!args[0].Equals("-fileName") && Regex.IsMatch(args[1],".+\\.db")) 
            {
                throw new ArgumentException("Usage: -fileName [database name].db");
            }

            String url = "Data Source=" + args[1];

            UserInterface ui = new UserInterface(url);

            ui.Start();
        }
    }
}