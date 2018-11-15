using System.Threading.Tasks;
using System.Xml.Serialization;
using SoapCore.SoapConvertor;
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

		[Fact]
		public void Can_set_XmlElmentName_of_the_request_attribute()
		{
			var z = new TestType();
			var result =SoapConvert.Serialize(z, "action", "ns2", SoapType.Request);
			Assert.Equal(222,result?.IndexOf("test123"));
		}
	}

	[XmlType(TypeName="test123")]
	public class TestType
	{
	}
}
