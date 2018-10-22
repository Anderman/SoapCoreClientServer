using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;

namespace SoapCore.SoapClient
{
	public interface ISoapConfig
	{
		string Namespace { get; }
		string SoapActionElementName { get; }
		string GetUrl(IHostingEnvironment env);
		string SoapAction { get; }
		X509Certificate ClientCertificate { get; }
	}
}