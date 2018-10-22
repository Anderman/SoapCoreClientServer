using System.Security.Cryptography.X509Certificates;

namespace SoapCore.Tests.SoapClientApp
{
	public class SoapClientX509Certificate2 : X509Certificate2
	{
		public SoapClientX509Certificate2(ISoapClientCertificateProvider soapClientCertificateProvider) : base(soapClientCertificateProvider.Certificate, soapClientCertificateProvider.Password)
		{
		}
	}
}