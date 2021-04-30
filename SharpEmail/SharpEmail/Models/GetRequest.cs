using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEmail.Models
{


	public class GetRequest
	{
		public string username { get; set; }
		public bool isOtherIdpSupported { get; set; }
		public bool checkPhones { get; set; }
		public bool isRemoteNGCSupported { get; set; }
		public bool isCookieBannerShown { get; set; }
		public bool isFidoSupported { get; set; }
		public string originalRequest { get; set; }
		public string country { get; set; }
		public bool forceotclogin { get; set; }
		public bool isExternalFederationDisallowed { get; set; }
		public bool isRemoteConnectSupported { get; set; }
		public int federationFlags { get; set; }
		public bool isSignup { get; set; }
		public string flowToken { get; set; }
		public bool isAccessPassSupported { get; set; }
	}
}
