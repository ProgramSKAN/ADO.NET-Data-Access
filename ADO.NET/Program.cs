using System;
using System.ComponentModel.Design;
using ADO.NET.Settings.Connect_and_Submit_Sql_To_Db;

namespace ADO.NET
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("ADO.NET Wizard");
            Console.WriteLine("================================");
            Console.WriteLine("Choose one of the following options");
            Console.WriteLine("1.get database infrom from connection string");
            Console.WriteLine("2.connect db with errors");
            Console.WriteLine("3.get a rows affected in select query using executescaler");
            Console.WriteLine("4.rows affected for insert country");
            Console.WriteLine("5.rows affected for Get country using parameters");
            Console.WriteLine("6.rows affected for insert country using parameters");
            Console.WriteLine("7.rows affected for insert country using parameters and get output parameter");
            Console.WriteLine("8.rows affected for insert country,state using parameters with rollbackable transaction");

            string optionSelected = Console.ReadLine();

            switch (Convert.ToInt32(optionSelected))
            {
                case 1:
                    Console.Write("enter connection string:");
                    string connectionString = Console.ReadLine();
                    //string result=Connection.Connect(connectionString);
                    string result1 = Connection.ConnectUsingBlock(connectionString);
                    Console.WriteLine(result1);
                    break;

                case 2:
                    Console.Write("using invalid connection string");
                    string result2 = Connection.ConnectWithErrors();
                    Console.WriteLine(result2);
                    break;
                case 3:
                    Console.WriteLine("SQL: SELECT COUNT(*) FROM Country");
                    int result3 = Command.GetCountryTableCountScalar();
                    Console.WriteLine("Rows Affected: "+result3.ToString());
                    break;
                case 4:
                    Command.InsertCountry();
                    break;
                case 5:
                    Command.GetCountryCountScalarUsingParameters();
                    break;
                case 6:
                    Command.InsertCountryUsingParameters();
                    break;
                case 7:
                    Command.InsertCountryOutputParameter(); 
                    break;
                case 8:
                    Command.TransactionProcessing(); 
                    break;
            }
            Console.ReadKey();

        }
    }
}
