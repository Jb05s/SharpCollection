using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace SharpSQL.Commands
{
    public class linkedlogin : ICommand
    {
        public static string CommandName => "linkedlogin";

        public void Execute(Dictionary<string, string> arguments)
        {
            Console.WriteLine("[*] Action: Execute Procedures to Get Login Information on Linked SQL Server:");
            Console.WriteLine("\tUsage: SharpSQL.exe linkedxp /db:DATABASE /server:SERVER /target:TARGET [/sqlauth /user:SQLUSER /password:SQLPASSWORD]\r\n");

            string user = "";
            string password = "";
            string connectInfo = "";
            string database = "";
            string connectserver = "";
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

			string createProc = $"EXEC ('CREATE PROCEDURE whoisuser AS SELECT SYSTEM_USER;') AT [{target}]";
			SqlCommand command = new SqlCommand(createProc, connection);
			SqlDataReader reader = command.ExecuteReader();
			reader.Read();
			Console.WriteLine("[*] Creating First Temporary Procedure..");
			reader.Close();

			createProc = $"EXEC ('CREATE PROCEDURE whoisname AS SELECT USER_NAME();') AT [{target}]";
			command = new SqlCommand(createProc, connection);
			reader = command.ExecuteReader();
			reader.Read();
			Console.WriteLine("\n[*] Creating First Temporary Procedure..");
			reader.Close();

			string whoisuser = $"EXEC ('EXEC whoisuser') AT [{target}]";
			command = new SqlCommand(whoisuser, connection);
			reader = command.ExecuteReader();
			reader.Read();
			Console.WriteLine("[+] Logged in as: " + reader[0]);
            reader.Close();

			string whoisname = $"EXEC ('EXEC whoisname') AT [{target}]";
			command = new SqlCommand(whoisname, connection);
			reader = command.ExecuteReader();
			reader.Read();
			Console.WriteLine("[+] Mapped to user: " + reader[0]);
			reader.Close();

			string dropProc = $"EXEC ('DROP PROCEDURE whoisuser') AT [{target}]";
			command = new SqlCommand(dropProc, connection);
			reader = command.ExecuteReader();
			reader.Read();
			Console.WriteLine("[*] Dropping First Temporary Procedure..");
			reader.Close();

			dropProc = $"EXEC ('DROP PROCEDURE whoisname') AT [{target}]";
			command = new SqlCommand(dropProc, connection);
			reader = command.ExecuteReader();
			reader.Read();
			Console.WriteLine("[*] Dropping Second Temporary Procedure..");
            reader.Close();

			connection.Close();
		}
    }
}