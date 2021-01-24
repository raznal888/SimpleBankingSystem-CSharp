using System.Data.SQLite;

namespace SimpleBankingSystem_CSharp
{
    internal class AccountManager
    {
        private Account CurrentAccount { get; set; }
        private readonly string _url;

        internal AccountManager(string url) 
        {
            CurrentAccount = new Account();
            _url = url;


            using var con = new SQLiteConnection(_url);
            con.Open();
                
            string query =
                "CREATE TABLE IF NOT EXISTS card (\n" +
                "id INTEGER,\n" +
                "number TEXT,\n"+
                "pin TEXT,\n" +
                "balance INTEGER DEFAULT 0\n" +
                ");";

            using var command = new SQLiteCommand(query, con);
            command.ExecuteNonQuery();
        }
        internal string GetCurrentCardNumber() { return CurrentAccount.CardNumber; }
        internal string GetCurrentPin() 
        {
            return CurrentAccount.Pin;
        }

        internal int GetCurrentBalance() 
        {
            return CurrentAccount.Balance;
        }
        internal bool IsLoginSuccessful(string[] loginData) 
        {
            string insert = "SELECT balance FROM card WHERE number = @number AND pin = @pin";

            using var con = new SQLiteConnection(_url);
            con.Open();

            using var command = new SQLiteCommand(insert, con);
            command.Parameters.AddWithValue("@number", loginData[0]);
            command.Parameters.AddWithValue("@pin", loginData[1]);
            command.Prepare();

            using var reader = command.ExecuteReader();
            bool isCardPresent = reader.Read();
            
            if (isCardPresent)
            {
                CurrentAccount.CardNumber = loginData[0];
                CurrentAccount.Pin = loginData[1];
                CurrentAccount.Balance = reader.GetInt32(0);
            }

            return isCardPresent;
        }

        internal bool IsCardInDatabase(string cardNumber)
        {

            string insert = "SELECT 1 FROM card WHERE number = @number";

            using var con = new SQLiteConnection(_url);
            con.Open();

            using var command = new SQLiteCommand(insert, con);
            command.Parameters.AddWithValue("@number", cardNumber);
            command.Prepare();

            using var reader = command.ExecuteReader();

            return reader.Read();
        }

        internal void AddAccount(Account account) 
        {
            CurrentAccount = account;

            string insert = "INSERT INTO card (number, pin) VALUES (@number, @pin)";
            
            using var con = new SQLiteConnection(_url);
            con.Open();

            using var command = new SQLiteCommand(insert, con);
            command.Parameters.AddWithValue("@number", GetCurrentCardNumber());
            command.Parameters.AddWithValue("@pin", GetCurrentPin());
            command.Prepare();

            command.ExecuteNonQuery();
        }

        internal void UpdateBalance(int amount)
        {
            CurrentAccount.Balance = amount;

            string insert = "UPDATE card SET balance = balance + @amount WHERE number = @number";

            using var con = new SQLiteConnection(_url);
            con.Open();

            using var command = new SQLiteCommand(insert, con);
            command.Parameters.AddWithValue("@amount", amount);
            command.Parameters.AddWithValue("@number", GetCurrentCardNumber());
            command.Prepare();

            command.ExecuteNonQuery();
        }

        internal void Transfer(int amount, string cardNumber) 
        {
            string insert = "UPDATE card SET balance = balance + @amount WHERE number = @number";
            
            using var con = new SQLiteConnection(_url);
            con.Open();

            SQLiteTransaction transaction = con.BeginTransaction();
            
            using var sender = new SQLiteCommand(insert, con, transaction);
            sender.Parameters.AddWithValue("@amount", -1 * amount);
            sender.Parameters.AddWithValue("@number", GetCurrentCardNumber());
            sender.Prepare();
            sender.ExecuteNonQuery();
            
            using var receiver = new SQLiteCommand(insert, con, transaction);
            receiver.Parameters.AddWithValue("@amount", amount);
            receiver.Parameters.AddWithValue("@number", cardNumber);
            receiver.Prepare();
            receiver.ExecuteNonQuery();

            transaction.Commit();

            CurrentAccount.Balance = -1 * amount;
        }

        internal void DeleteCurrentAccount() 
        {

            string insert = "DELETE FROM card WHERE number = @number";
            
            using var con = new SQLiteConnection(_url);
            con.Open();

            using var command = new SQLiteCommand(insert, con);
            command.Parameters.AddWithValue("@number", GetCurrentCardNumber());
            command.Prepare();

            command.ExecuteNonQuery();
        }
    }
}