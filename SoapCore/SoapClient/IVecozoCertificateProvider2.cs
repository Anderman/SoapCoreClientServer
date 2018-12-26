using System.Security.Cryptography.X509Certificates;
// ReSharper disable IdentifierTypo

namespace SoapCore.SoapClient
{
	public interface ICertificateProvider
	{
		X509Certificate2 Certificate2 { get; }

	}
}