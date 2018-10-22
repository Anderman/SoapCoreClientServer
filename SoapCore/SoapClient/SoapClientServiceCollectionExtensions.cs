using SoapCore.SoapClient;

// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection
{
	public static class SoapClientServiceCollectionExtensions
	{
		public static IServiceCollection AddSoapClients(this IServiceCollection services)
		{
			//Soap client
			services.AddTransient(typeof(SoapClient<,,>));
			return services;
		}
	}
}