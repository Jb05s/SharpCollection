using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace SharpSQL.Commands
{
    public class dbllinkedlogin : ICommand
    {
        public static string CommandName => "dbllinkedlogin";

        public void Execute(Dictionary<string, string> arguments)
        {
            Console.WriteLine("[*] Action: Execute Procedures to Get Login Information on Double-Linked SQL Server:");
            Console.WriteLine("\tUsage: SharpSQL.exe dbllinkedlogin /db:DATABASE /server:SERVER /intermediate:INTERMEDIATE /target:TARGET [/sqlauth /user:SQLUSER /password:SQLPASSWORD]\r\n");

            string user = "";
            string password = "";
            string connectInfo = "";
            string database = "";
            string connectserver = "";
            string intermediate = "";
            string target = "";

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

            string createProc = $"EXEC ('EXEC (''CREATE PROCEDURE whoami AS SELECT SYSTEM_USER;'') AT [{target}]') AT {intermediate}";
			SqlCommand command = new SqlCommand(createProc, connection);
			SqlDataReader reader = command.ExecuteReader();
			reader.Read();
			Console.WriteLine("\n[*] Creating First Temporary Procedure..");
			reader.Close();

			createProc = $"EXEC ('EXEC (''CREATE PROCEDURE whoisuser AS SELECT USER_NAME();'') AT [{target}]') AT {intermediate}";
			command = new SqlCommand(createProc, connection);
			reader = command.ExecuteReader();
			reader.Read();
			Console.WriteLine("[*] Creating Second Temporary Procedure..");
			reader.Close();

			string whoami = $"EXEC ('EXEC (''EXEC whoami'') AT [{target}]') AT {intermediate}";
			command = new SqlCommand(whoami, connection);
			reader = command.ExecuteReader();
			reader.Read();
			Console.WriteLine("[+] Logged in as: " + reader[0] + $" on {target}");
			reader.Close();

			string whoisuser = $"EXEC ('EXEC (''EXEC whoisuser'') AT [{target}]') AT {intermediate}";
			command = new SqlCommand(whoisuser, connection);
			reader = command.ExecuteReader();
			reader.Read();
			Console.WriteLine("[+] Mapped to user: " + reader[0] + $" on {target}");
			reader.Close();

			string dropProc = $"EXEC ('EXEC (''DROP PROCEDURE whoami'') AT [{target}]') AT {intermediate}";
			command = new SqlCommand(dropProc, connection);
			reader = command.ExecuteReader();
			reader.Read();
			Console.WriteLine("[*] Dropping First Temporary Procedure..");
			reader.Close();

			dropProc = $"EXEC ('EXEC (''DROP PROCEDURE whoisuser'') AT [{target}]') AT {intermediate}";
			command = new SqlCommand(dropProc, connection);
			reader = command.ExecuteReader();
			reader.Read();
			Console.WriteLine("[*] Dropping Second Temporary Procedure..\n");
			reader.Close();

			connection.Close();
        }
    }
}
