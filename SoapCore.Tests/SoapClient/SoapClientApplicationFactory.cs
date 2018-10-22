using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using SoapCore.Tests.SoapClientApp;

namespace SoapCore.Tests.SoapClient
{
	public class SoapClientApplicationFactory<TStartup> :  WebApplicationFactory<Startup>
	{
		protected override IWebHostBuilder CreateWebHostBuilder()
		{
			return WebHost.CreateDefaultBuilder()
				.ConfigureLogging((hostingContext, logging) =>
				{
					logging.SetMinimumLevel(LogLevel.Debug);
					logging.AddConsole();
					logging.AddDebug();
				})
				.ConfigureAppConfiguration((builderContext, config) =>
				{
					var env = builderContext.HostingEnvironment;
				})
				.UseStartup<Startup>();
		}

	}
}