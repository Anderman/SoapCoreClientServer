using System.Security;

namespace SoapCore.Tests
{
	public interface ISoapClientCertificateProvider
	{
		byte[] Certificate { get; }
		SecureString Password { get; }
	}
}