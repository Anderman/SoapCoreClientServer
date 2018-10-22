using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SoapCore.Tests.SoapServiceApp
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddTransient<AliveService>();
		}

		public void Configure(IApplicationBuilder app)
		{
			app.UseSoapServices<Startup>();
		}
	}
}