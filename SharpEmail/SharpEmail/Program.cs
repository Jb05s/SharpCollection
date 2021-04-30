using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharpEmail.Models;
using System.IO;
using Dasync.Collections;
using System.Net.Http;
using System.Net;
using System.Security.Authentication;

namespace SharpEmail
{
	class ProgramOptions
	{
		public string file;
		public string proxy;

		public ProgramOptions(string uFile = "", string uProxy = "")
		{
			file = uFile;
			proxy = uProxy;
		}
	}

	static class Program
	{
		public static void Banner()
		{
			Console.WriteLine(@"
 #####                              #######                        
#     # #    #   ##   #####  #####  #       #    #   ##   # #      
#       #    #  #  #  #    # #    # #       ##  ##  #  #  # #      
 #####  ###### #    # #    # #    # #####   # ## # #    # # #      
      # #    # ###### #####  #####  #       #    # ###### # #      
#     # #    # #    # #   #  #      #       #    # #    # # #      
 #####  #    # #    # #    # #      ####### #    # #    # # ######

Version: 1.0
Author: Jb05s
Twitter: @Jb05s
");
		}

		public static void Help()
		{
			Console.WriteLine(@"Usage:
--------------------
SharpEmail.exe --file=C:\Users\anon\Desktop\emailList.txt --proxy=127.0.0.1:8080

Required Arguments:
--------------------
--file=		- Path to file containing a list of potential tenant email accounts

Optional Arguments:
--------------------
--proxy=	- Source IP:Port of proxy server


--help		- Print help information
");
		}

		private static async Task<bool> ValidateUser(this HttpClient httpClient, string username, string country = "US")
		{
			var getRequest = new GetRequest
			{
				username = username,
				country = country
			};

			var httpReq = await httpClient.PostAsync("https://login.microsoftonline.com/common/GetCredentialType", new StringContent(JsonConvert.SerializeObject(getRequest), Encoding.UTF8, "application/json"));

			if (httpReq.IsSuccessStatusCode)
			{
				var httpResp = await httpReq.Content.ReadAsStringAsync();
				GetResponse getResponse = JsonConvert.DeserializeObject<GetResponse>(httpResp);
				return (getResponse.IfExistsResult == 0);
			}
			return false;
		}

		private static async Task AsyncMain(string[] args)
		{
			ProgramOptions options = new ProgramOptions();

			foreach (string arg in args)
			{
				if (arg.StartsWith("--file="))
				{
					string[] components = arg.Split(new string[] { "--file=" }, StringSplitOptions.None);
					options.file = components[1];
				}

				else if (arg.StartsWith("--proxy="))
				{
					string[] components = arg.Split(new string[] { "--proxy=" }, StringSplitOptions.None);
					options.proxy = components[1];
				}
				else if (arg.StartsWith("--help"))
				{
					Banner();
					Help();
					Environment.Exit(0);
				}
				else
				{
					Console.WriteLine("[!] Invalid flag(s) provided: " + arg);
					return;
				}
			}

			// Read a list of users
			var usersArray = File.ReadAllLines(options.file);

			if (!String.IsNullOrEmpty(options.file))
			{
				if (args.Length == 2)
				{
					var proxy = new WebProxy
					{
						Address = new Uri($"http://{options.proxy}"),
						BypassProxyOnLocal = false,
						UseDefaultCredentials = false,
					};

					var httpClientHandler = new HttpClientHandler
					{
						Proxy = proxy,
						UseProxy = true,
						UseCookies = false,
						ServerCertificateCustomValidationCallback = (message, xcert, chain, errors) =>
						{
							return true;
						},
						SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls,
					};

					using (var HttpClient = new HttpClient(httpClientHandler))
					{
						// Generate canary account to determine accuracy in validity for tenant
						var canary = $"{Guid.NewGuid().ToString().Replace("-", "")}@" + usersArray[0].Split('@')[1];

						if (await HttpClient.ValidateUser(canary))
						{
							Console.WriteLine("[-] Canary returned valid.. tenant cannot be enumerated with this method.");
							Environment.Exit(0);
						}
						else
							Console.WriteLine(@"[*] Canary returned invalid.. proceeding with tenant enumeration.");

						// Loop through users
						await usersArray.ParallelForEachAsync(async user =>
						{
							if (await HttpClient.ValidateUser(user))
							{
								Console.WriteLine($"[+] {user} is valid");
							}
							else
							{
								Console.WriteLine($"[-] {user} is not valid");
							}
						}, maxDegreeOfParallelism: 50);
					}
				}

				else
				{
					using (var HttpClient = new HttpClient())
					{
						// Generate canary account to determine accuracy in validity for tenant
						var canary = $"{Guid.NewGuid().ToString().Replace("-", "")}@" + usersArray[0].Split('@')[1];

						if (await HttpClient.ValidateUser(canary))
						{
							Console.WriteLine($"[-] Canary returned valid.. tenant cannot be enumerated with this method.\n");
							Environment.Exit(0);
						}
						else
							Console.WriteLine("[*] Canary returned invalid.. proceeding with tenant enumeration.");

						// Loop through users
						await usersArray.ParallelForEachAsync(async user =>
						{
							if (await HttpClient.ValidateUser(user))
							{
								Console.WriteLine($"[+] {user} is valid");
							}
							else
							{
								Console.WriteLine($"[-] {user} is not valid");
							}
						}, maxDegreeOfParallelism: 50);
					}
				}
			}
			else
			{
				Console.WriteLine(@"[!] Missing required ""--file"" argument");
				Environment.Exit(1);
			}
		}

		static void Main(string[] args)
		{
			AsyncMain(args).GetAwaiter().GetResult();
		}
	}
}