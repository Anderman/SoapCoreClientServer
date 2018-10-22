using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using SoapCore.Tests.SoapServiceApp;

namespace SoapCore.Tests.SoapService
{
	public class SoapServiceApplicationFactory
	{
		public SoapServiceApplicationFactory()
		{
			var cert = new TestX509Certificate2(new IncomingCertificateProvider());
			Task.Run(() =>
			{
				var builder = new WebHostBuilder()
					.UseKestrel(cfg => cfg.ListenLocalhost(5050)) //, x =>
					// {
					//	 x.UseHttps(cert, y => y.ClientCertificateMode = ClientCertificateMode.RequireCertificate);
					// }))
					.UseUrls("http://localhost:5050")
					.UseStartup<Startup>();
				builder.ConfigureServices(services =>
				{
				});
				var host = builder.Build();
				host.Run();
			});
		}


		public static T CreateSoap12Client<T>()
		{
			var transport = new HttpTransportBindingElement();
			var encoding = new TextMessageEncodingBindingElement(MessageVersion.Soap12WSAddressing10, Encoding.UTF8);
			var binding = new CustomBinding(encoding, transport);

			var endpoint = new EndpointAddress(new Uri($"http://localhost:5050/someUrl"));

			var channelFactory = new ChannelFactory<T>(binding, endpoint);

			channelFactory.Credentials.ClientCertificate.Certificate = new TestX509Certificate2(new IncomingCertificateProvider());

			var serviceClient = channelFactory.CreateChannel();
			return serviceClient;
		}
	}
}