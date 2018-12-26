using Microsoft.AspNetCore.Hosting;

namespace SoapCore.SoapClient
{
	public interface ISoapConfig
	{
		string Namespace { get; }
		string SoapActionElementName { get; }
		string SoapAction { get; }
		string GetUrl(IHostingEnvironment env);
	}
}