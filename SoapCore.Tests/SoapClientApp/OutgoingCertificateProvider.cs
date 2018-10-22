using System;
using System.IO;
using System.Security;

namespace SoapCore.Tests.SoapClientApp
{
	public class OutgoingCertificateProvider : ISoapClientCertificateProvider
	{
		public byte[] Certificate => File.ReadAllBytes("c:/temp/sc.pfx");
		public SecureString Password => GetSecureString("Itzos");

		private static SecureString GetSecureString(string s)
		{
			var secureString = new SecureString();
			Array.ForEach(s.ToCharArray(), secureString.AppendChar);
			return secureString;
		}
	}
}