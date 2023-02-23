using System;
using System.Collections.Generic;
using System.Text;

namespace SharpSQL.Commands
{
    public class encode : ICommand
    {
        public static string CommandName => "encode";

        public void Execute(Dictionary<string, string> arguments)
        {
            Console.WriteLine("[*] Action: Base64 encode string for '/command' switch");
            Console.WriteLine("\tUsage: SharpSQL.exe encode /b64:STRING");

            string b64 = "";

            if (arguments.ContainsKey("/b64"))
            {
                b64 = arguments["/b64"];
            }

            if (String.IsNullOrEmpty(b64))
            {
                Console.WriteLine("\r\n[X] You must supply a string!\r\n");
                return;
            }
            var encodeData = Encoding.Unicode.GetBytes(b64);
			Console.WriteLine($"Base64 encoded string: {Convert.ToBase64String(encodeData)}");

        }
    }
}
