using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleBankingSystem_CSharp
{
    internal class Menu
    {
        private readonly List<MenuItem> _items;

        internal Menu(UserInterface ui) 
        {
            _items = new List<MenuItem> {new ("0", "Exit", ui.Exit)};
        }

        internal void ExecuteCommand(string command)
        {

            try
            {
                _items.First(item => command.Equals(item.Id)).Function.Invoke();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("Invalid command");
            }
            
        }

        internal void AddItem(MenuItem item) 
        {
            _items.Add(item);
        }
        
        public override string ToString()
        {
            StringBuilder menu = new StringBuilder();

            for (int i = 1, n = _items.Count; i < n; i++) 
            {
                menu.Append(_items[i]);
                menu.Append('\n');
            }
            menu.Append(_items[0]);

            return menu.ToString();
        }
    }
}