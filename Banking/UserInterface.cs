using System;

namespace SimpleBankingSystem_CSharp
{
    internal class UserInterface
    {
        private readonly AccountManager _manager;
        private readonly Menu _mainMenu;
        private readonly Menu _accountMenu;
        private Menu _currentMenu;

        internal UserInterface(string url) 
        {
            _manager = new AccountManager(url);

            _mainMenu = new Menu(this);
            _mainMenu.AddItem(new MenuItem("1", "Create an account", CreateAccount));
            _mainMenu.AddItem(new MenuItem("2", "Log into account", Login));

            _accountMenu = new Menu(this);
            _accountMenu.AddItem(new MenuItem("1", "Balance", PrintBalance));
            _accountMenu.AddItem(new MenuItem("2", "Add income", Deposit));
            _accountMenu.AddItem(new MenuItem("3", "Do transfer", Transfer));
            _accountMenu.AddItem(new MenuItem("4", "Close account", CloseAccount));
            _accountMenu.AddItem(new MenuItem("5", "Log out", Logout));

            _currentMenu = _mainMenu;
        }

        internal void Start() 
        {

            while (true) 
            {
                Console.WriteLine(_currentMenu);

                string command = Console.ReadLine();
                Console.WriteLine();

                _currentMenu.ExecuteCommand(command);
                Console.WriteLine();
            }

        }

        private void CreateAccount() 
        {
            _manager.AddAccount(new Account());

            Console.WriteLine("Your card has been created");
            Console.WriteLine("Your card number:\n" + _manager.GetCurrentCardNumber());
            Console.WriteLine("Your card PIN:\n" + _manager.GetCurrentPin());
        }

        private void Login() 
        {

            if (_manager.IsLoginSuccessful(EnterLoginData())) 
            {
                Console.WriteLine("You have successfully logged in!");
                SwitchMenu();
            } 
            else 
            {
                Console.WriteLine("Wrong card number or PIN!");
            }

        }

        private void PrintBalance() 
        {
            Console.WriteLine("Balance: " + _manager.GetCurrentBalance());
        }

        private void Deposit() 
        {
            Console.WriteLine("Enter income:");
            string balanceString = Console.ReadLine();
            if (int.TryParse(balanceString, out int balance)) 
            {
                _manager.UpdateBalance(balance);
                Console.WriteLine("Income was added!");
            } 
            else 
            {
                Console.WriteLine("Enter an integer!");
            }

        }

        private void Transfer() 
        {
            Console.WriteLine("Transfer");
            Console.WriteLine("Enter card number:");
            string cardNumber = Console.ReadLine();

            if (!Int64.TryParse(cardNumber, out long number) || !PassesLuhnAlgorithm(cardNumber)) 
            {
                Console.WriteLine("Probably you made mistake in the card number. Please try again!");
                return;
            }

            if (cardNumber.Equals(_manager.GetCurrentCardNumber())) 
            {
                Console.WriteLine("You can't transfer money to the same account!");
                return;
            }

            if (!_manager.IsCardInDatabase(cardNumber)) 
            {
                Console.WriteLine("Such a card does not exist.");
                return;
            }

            Console.WriteLine("Enter how much money you want to transfer:");
            string amountString = Console.ReadLine();
            
            if (int.TryParse(amountString, out int amount)) 
            {
                
                if (amount > _manager.GetCurrentBalance()) 
                {
                    Console.WriteLine("Not enough money!");
                    return;
                }

                _manager.Transfer(amount, cardNumber);
                Console.WriteLine("Success!");
            } 
            else 
            {
                Console.WriteLine("Enter an integer!");
            }

        }

        private void CloseAccount() 
        {
            _manager.DeleteCurrentAccount();
            Console.WriteLine("The account has been closed!");

            SwitchMenu();
        }

        private void Logout() 
        {
            Console.WriteLine("You have successfully logged out!");
            SwitchMenu();
        }

        internal void Exit() 
        {
            Console.WriteLine("Bye!");
            Environment.Exit(0);
        }

        private string[] EnterLoginData() 
        {
            string[] loginData = new string[2];

            Console.WriteLine("Enter your card number:");
            string cardNumber = Console.ReadLine();
            Console.WriteLine("Enter your PIN:");
            string pin = Console.ReadLine();

            loginData[0] = cardNumber;
            loginData[1] = pin;

            Console.WriteLine();

            return loginData;
        }

        private void SwitchMenu() 
        {
            _currentMenu = _currentMenu == _mainMenu ? _accountMenu : _mainMenu;
        }

        private bool PassesLuhnAlgorithm(string cardNumber) 
        {
            return Account.LuhnAlgorithmChecksum(cardNumber) == 0;
        }
    }
}