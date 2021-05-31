using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace SharpSQL.Commands
{
    public class getdbuser : ICommand
    {
        public static string CommandName => "getdbuser";

        public void Execute(Dictionary<string, string> arguments)
        {
            Console.WriteLine("[*] Action: Retrieve Information on the SQL Login, Currently Mapped User, and Available User Roles\r\n");
            Console.WriteLine("\tUsage: SharpSQL.exe getdbuser /db:DATABASE /server:SERVER [/impersonate]\r\n");

            string database = "";
            string connectserver = "";

            bool impersonate = false;

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

            string queryLogin = "SELECT SYSTEM_USER;";
            SqlCommand command = new SqlCommand(queryLogin, connection);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            Console.WriteLine("[+] Logged in as: " + reader[0]);
            reader.Close();

            string queryUser = "SELECT USER_NAME();";
            command = new SqlCommand(queryUser, connection);
            reader = command.ExecuteReader();
            reader.Read();
            Console.WriteLine("[+] Mapped to user: " + reader[0]);
            reader.Close();

            string queryPubRole = "SELECT IS_SRVROLEMEMBER('public');";
            command = new SqlCommand(queryPubRole, connection);
            reader = command.ExecuteReader();
            reader.Read();

            Int32 role = Int32.Parse(reader[0].ToString());
            if (role == 1)
            {
                Console.WriteLine("[+] User is a member of the 'Public' role");
            }
            else
            {
                Console.WriteLine("[-] User is not a member of the 'Public' role");
            }
            reader.Close();

            string querySARole = "SELECT IS_SRVROLEMEMBER('sysadmin');";
            command = new SqlCommand(querySARole, connection);
            reader = command.ExecuteReader();
            reader.Read();

            role = Int32.Parse(reader[0].ToString());
            if (role == 1)
            {
                Console.WriteLine("[+] User is a member of the 'sysadmin' role");
            }
            else
            {
                Console.WriteLine("[-] User is not a member of the 'sysadmin' role");
            }
            reader.Close();

            if (impersonate)
            {
                string execAs = "use msdb; EXECUTE AS USER = 'dbo';";
                command = new SqlCommand(execAs, connection);
                reader = command.ExecuteReader();
                reader.Read();
                Console.WriteLine("[*] Attempting impersonation..");
                reader.Close();

                command = new SqlCommand(queryUser, connection);
                reader = command.ExecuteReader();
                reader.Read();
                Console.WriteLine("[+] Mapped to user: " + reader[0]);
                reader.Close();

                queryPubRole = "SELECT IS_SRVROLEMEMBER('public');";
                command = new SqlCommand(queryPubRole, connection);
                reader = command.ExecuteReader();
                reader.Read();

                role = Int32.Parse(reader[0].ToString());
                if (role == 1)
                {
                    Console.WriteLine("[+] User is a member of the 'Public' role");
                }
                else
                {
                    Console.WriteLine("[-] User is not a member of the 'Public' role");
                }
                reader.Close();

                querySARole = "SELECT IS_SRVROLEMEMBER('sysadmin');";
                command = new SqlCommand(querySARole, connection);
                reader = command.ExecuteReader();
                reader.Read();

                role = Int32.Parse(reader[0].ToString());
                if (role == 1)
                {
                    Console.WriteLine("[+] User is a member of the 'sysadmin' role");
                }
                else
                {
                    Console.WriteLine("[-] User is not a member of the 'sysadmin' role");
                }
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
