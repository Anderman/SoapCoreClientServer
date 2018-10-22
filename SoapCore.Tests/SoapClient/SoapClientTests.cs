using System.Threading.Tasks;
using SoapCore.Tests.SoapClientApp;
using Xunit;

namespace SoapCore.Tests.SoapClient
{
	public class SoapClientTests : IClassFixture<SoapClientApplicationFactory<Startup>>
	{
		private readonly SoapClientApplicationFactory<Startup> _factory;

		public SoapClientTests(SoapClientApplicationFactory<Startup> factory)
		{
			_factory = factory;
		}
		[Fact]
		public async Task soap_client_has_result()
		{
		 	var  client = _factory.CreateClient();
			var result= await client.GetAsync("");
			Assert.NotNull(result);
		}
	}
}