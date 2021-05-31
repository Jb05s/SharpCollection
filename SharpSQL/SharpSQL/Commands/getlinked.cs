using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace SharpSQL.Commands
{
    public class getlinked : ICommand
    {
        public static string CommandName => "getlinked";

        public void Execute(Dictionary<string, string> arguments)
        {
            Console.WriteLine("[*] Action: Retrieve Linked Servers\r\n");
            Console.WriteLine("\tUsage: SharpSQL.exe getlinked /db:DATABASE /server:SERVER\r\n");

            string database = "";
            string connectserver = "";

            if (arguments.ContainsKey("/db"))
            {
                database = arguments["/db"];
            }
            if (arguments.ContainsKey("/server"))
            {
                connectserver = arguments["/server"];
            }

            if (String.IsNullOrEmpty(database))
            {
                Console.WriteLine("\r\n[X] You must supply a database!\r\n");
                return;
            }
            if (String.IsNullOrEmpty(connectserver))
            {
                Console.WriteLine("\r\n[X] You must supply an authentication server!\r\n");
                return;
            }

            string connectInfo = "Server = " + connectserver + "; Database = " + database + "; Integrated Security = True;";
            SqlConnection connection = new SqlConnection(connectInfo);

            try
            {
                connection.Open();
                Console.WriteLine($"[+] Authentication to the '{database}' Database on '{connectserver}' Successful!");
            }
            catch
            {
                Console.WriteLine($"[-] Authentication to the '{database}' Database on '{connectserver}' Failed.");
                Environment.Exit(0);
            }

            string execCmd = "EXECUTE sp_linkedservers;";
            SqlCommand command = new SqlCommand(execCmd, connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine("[*] Linked SQL server: " + reader[0]);
            }
            reader.Close();

            connection.Close();
        }
    }
}
