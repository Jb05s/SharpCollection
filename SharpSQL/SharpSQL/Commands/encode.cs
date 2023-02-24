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
            Console.WriteLine("[*] Action: Base64 Encode an Arbitrary String for the '/command' Switch:");
            Console.WriteLine("\tUsage: SharpSQL.exe encode /b64:STRING\r\n");

            string cmd = "";

            if (arguments.ContainsKey("/command"))
            {
                cmd = arguments["/command"];
            }

            if (String.IsNullOrEmpty(cmd))
            {
                Console.WriteLine("\r\n[X] You must supply a string!\r\n");
                return;
            }
            var encodeData = Encoding.Unicode.GetBytes(cmd);
			Console.WriteLine($"Base64 Encoded String: {Convert.ToBase64String(encodeData)}\n");

        }
    }
}
