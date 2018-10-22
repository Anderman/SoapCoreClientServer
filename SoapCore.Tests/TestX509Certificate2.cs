using System.Security.Cryptography.X509Certificates;
using SoapCore.SoapClient;

namespace SoapCore.Tests
{
	public class TestX509Certificate2 : X509Certificate2
	{
		public TestX509Certificate2(ISoapClientCertificateProvider certificateProvider) : base(certificateProvider.Certificate, certificateProvider.Password)
		{

		}
	}
}