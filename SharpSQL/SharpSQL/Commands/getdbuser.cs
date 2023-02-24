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
            Console.WriteLine("[*] Action: Retrieve Information on the SQL Login, Currently Mapped User, and Available User Roles:");
            Console.WriteLine("\tUsage: SharpSQL.exe getdbuser /db:DATABASE /server:SERVER [/impersonate] [/sqlauth /user:SQLUSER /password:SQLPASSWORD]\r\n");

            string database = "";
            string connectserver = "";
            string user = "";
            string password = "";
            string connectInfo = "";

            bool sqlauth = false;
            bool impersonate = false;

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
            Console.WriteLine("\n[+] Logged in as: " + reader[0] + $" on {connectserver}");
            reader.Close();

            string queryUser = "SELECT USER_NAME();";
            command = new SqlCommand(queryUser, connection);
            reader = command.ExecuteReader();
            reader.Read();
            Console.WriteLine("[+] Mapped to user: " + reader[0] + $" on {connectserver}");
            reader.Close();

            string queryPubRole = "SELECT IS_SRVROLEMEMBER('public');";
            command = new SqlCommand(queryPubRole, connection);
            reader = command.ExecuteReader();
            reader.Read();

            Int32 role = Int32.Parse(reader[0].ToString());
            if (role == 1)
            {
                Console.WriteLine("[+] User is a Member of the 'Public' Role");
            }
            else
            {
                Console.WriteLine("[-] User is not a Member of the 'public' Role");
            }
            reader.Close();

            string querySARole = "SELECT IS_SRVROLEMEMBER('sysadmin');";
            command = new SqlCommand(querySARole, connection);
            reader = command.ExecuteReader();
            reader.Read();

            role = Int32.Parse(reader[0].ToString());
            if (role == 1)
            {
                Console.WriteLine("[+] User is a Member of the 'sysadmin' Role");
            }
            else
            {
                Console.WriteLine("[-] User is not a Member of the 'sysadmin' Role");
            }
            reader.Close();

            if (impersonate)
            {
                string execAs = "use msdb; EXECUTE AS USER = 'dbo';";
                command = new SqlCommand(execAs, connection);
                reader = command.ExecuteReader();
                reader.Read();
                Console.WriteLine("\n[*] Attempting Impersonation..");
                reader.Close();

                command = new SqlCommand(queryUser, connection);
                reader = command.ExecuteReader();
                reader.Read();
                Console.WriteLine("[+] Mapped to User: " + reader[0] + $" on {connectserver}");
                reader.Close();

                queryPubRole = "SELECT IS_SRVROLEMEMBER('public');";
                command = new SqlCommand(queryPubRole, connection);
                reader = command.ExecuteReader();
                reader.Read();

                role = Int32.Parse(reader[0].ToString());
                if (role == 1)
                {
                    Console.WriteLine("[+] User is a Member of the 'Public' Role");
                }
                else
                {
                    Console.WriteLine("[-] User is not a Member of the 'Public' Role");
                }
                reader.Close();

                querySARole = "SELECT IS_SRVROLEMEMBER('sysadmin');";
                command = new SqlCommand(querySARole, connection);
                reader = command.ExecuteReader();
                reader.Read();

                role = Int32.Parse(reader[0].ToString());
                if (role == 1)
                {
                    Console.WriteLine("[+] User is a Member of the 'sysadmin' Role");
                }
                else
                {
                    Console.WriteLine("[-] User is not a Member of the 'sysadmin' Role");
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
