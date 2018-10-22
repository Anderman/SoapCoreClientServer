using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SoapCore.SoapClient;

namespace SoapCore.Tests.SoapClientApp
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSoapClients();
			services.AddSingleton<ILogger, Logger<Startup>>();
		}

		public void Configure(IApplicationBuilder app)
		{
			app.UseMiddleware<SoapClientMiddleware>();
		}
	}


	public class SoapClientMiddleware
	{

		public SoapClientMiddleware(RequestDelegate next)
		{
		}

		public async Task<byte[]> Invoke(HttpContext httpContext, SoapClient<Config, DownloadPdfRequest, DownloadPdfResponse> client)
		{
			var request = new DownloadPdfRequest { PdfId = 1 };

			var result = await client.PostAsync(request);
			return result.PdfRetourbestand?.Bestand?.Data;
		}

		public class Config : ISoapConfig
		{
			public string Namespace => "urn:www-vecozo-nl:vsp:edp:declareren:downloaden:v1";
			public string SoapActionElementName => "DownloadPdf";
			public string SoapAction => $"{Namespace}:{SoapActionElementName}";
			public X509Certificate ClientCertificate => new SoapClientX509Certificate2(new OutgoingCertificateProvider());

			public string GetUrl(IHostingEnvironment env)
			{
				return "https://accedpwebservice.vecozo.nl/Router.V1.svc/DownloadenRetourinformatieV1";
			}
		}
	}

	
}