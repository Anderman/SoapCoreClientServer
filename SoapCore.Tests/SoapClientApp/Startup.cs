using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SoapCore.SoapClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace SoapCore.Tests.SoapClientApp
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSoapClients();
			services.AddTransient<SoapController>();
			services.AddSingleton<ILogger, Logger<Startup>>();
			services.AddSingleton<ICertificateProvider, CertificateProvider>();
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

		public async Task<byte[]> Invoke(HttpContext httpContext, IServiceProvider serviceProvider)
		{
			var controller=serviceProvider.GetService<SoapController>();
			return await controller.DownloadPdf(1);
		}
	}

	public class SoapController
	{
		private readonly SoapClient<Config, DownloadPdfRequest, DownloadPdfResponse> _client;

		public SoapController(SoapClient<Config, DownloadPdfRequest, DownloadPdfResponse> client)
		{
			_client = client;
		}

		public async Task<byte[]> DownloadPdf(int pdfId)
		{
			var request = new DownloadPdfRequest { PdfId = pdfId };
			var result = await _client.PostAsync(request);
			return result.PdfRetourbestand?.Bestand?.Data;
		}

		public class Config : ISoapConfig
		{
			public string Namespace => "urn:www-vecozo-nl:vsp:edp:declareren:downloaden:v1";
			public string SoapActionElementName => "DownloadPdf";
			public string SoapAction => $"{Namespace}:{SoapActionElementName}";

			public string GetUrl(IHostingEnvironment env)
			{
				return "https://accedpwebservice.vecozo.nl/Router.V1.svc/DownloadenRetourinformatieV1";
			}
		}

	}
	public class CertificateProvider : ICertificateProvider
	{
		public X509Certificate2 Certificate2 => new SoapClientX509Certificate2(new OutgoingCertificateProvider());
	}


}