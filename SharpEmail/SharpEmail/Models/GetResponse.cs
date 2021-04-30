﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEmail.Models
{


	public class GetResponse
	{
		public string Username { get; set; }
		public string Display { get; set; }
		public int IfExistsResult { get; set; }
		public bool IsUnmanaged { get; set; }
		public int ThrottleStatus { get; set; }
		public Credentials Credentials { get; set; }
		public Estsproperties EstsProperties { get; set; }
		public string FlowToken { get; set; }
		public bool IsSignupDisallowed { get; set; }
		public string apiCanary { get; set; }
	}

	public class Credentials
	{
		public int PrefCredential { get; set; }
		public bool HasPassword { get; set; }
		public object RemoteNgcParams { get; set; }
		public object FidoParams { get; set; }
		public object SasParams { get; set; }
		public object CertAuthParams { get; set; }
		public object GoogleParams { get; set; }
		public object FacebookParams { get; set; }
	}

	public class Estsproperties
	{
		public int DomainType { get; set; }
	}
}
