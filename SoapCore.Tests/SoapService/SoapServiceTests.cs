using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using SoapCore.SoapServices;
using SoapCore.Tests.Connected_Services.Vecozo.Live;
using Xunit;

namespace SoapCore.Tests.SoapService
{
	public class SoapServiceTests : IClassFixture<SoapServiceApplicationFactory>
	{
		[System.ServiceModel.ServiceContract(Namespace = "urn:www-vecozo-nl:isalive:v1", ConfigurationName = "ServiceReference1.IsAlive")]
		public interface IFailService
		{
			[System.ServiceModel.OperationContract(Action = "urn:www-vecozo-nl:v1:isalive2", ReplyAction = "urn:www-vecozo-nl:v1:isaliveresponse")]
			Task<IsAliveResponse> IsAliveAsync(IsAliveRequest isAliveRequest);
		}

		public interface IsAliveChannel : IFailService, IClientChannel
		{
		}

		[Fact]
		public async Task not_soap_client_can_pass()
		{
			var client = new HttpClient();
			var result = await client.GetAsync("http://localhost:5050/");
			Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
		}
		[Fact]
		public async Task not_implemented_service_should_fail()
		{
			var client = SoapServiceApplicationFactory.CreateSoap12Client<IFailService>();
			var result = await Assert.ThrowsAnyAsync<FaultException>(() => client.IsAliveAsync(new IsAliveRequest()));
			Assert.Contains("No operation found", result.Reason.ToString());
		}

		[Fact]
		public async Task soap_service_should_return_result()
		{
			var client = SoapServiceApplicationFactory.CreateSoap12Client<IsAlive>();
			var result = await client.IsAliveAsync(new IsAliveRequest());
			Assert.True(result.Resultaat);
		}
		[Fact]
		public void TestSoap12()
		{
			var x = new DefaultHttpContext();
			x.Request.Headers.Add(new KeyValuePair<string, StringValues>("content-Type", $"application/soap+xml;action=\"soapAction\""));
			var action = new SoapEndpointMiddleware(null, null).GetSoapAction(x.Request);
			Assert.Equal("soapAction", action);
		}
		[Fact]
		public void TestSoap11()
		{
			var x = new DefaultHttpContext();
			x.Request.Headers["SoapAction"] = "soapAction";
			var action = new SoapEndpointMiddleware(null, null).GetSoapAction(x.Request);
			Assert.Equal("soapAction", action);
		}
		[Fact]
		public void TestSoap12Other()
		{
			var x = new DefaultHttpContext();
			var ms = new MemoryStream(Encoding.UTF8.GetBytes(@"<s:Envelope xmlns:s=""http://www.w3.org/2003/05/soap-envelope"" xmlns:a=""http://www.w3.org/2005/08/addressing""><s:Body><s:Header><a:Action s:mustUnderstand=""1"">soapAction</a:Action></s:Header></s:Body></s:Envelope>"));
			x.Request.Body = ms;
			var action = new SoapEndpointMiddleware(null, null).GetSoapAction(x.Request);
			Assert.Equal("soapAction", action);
		}
		[Fact]
		public void TestSoap12Unknown()
		{
			var x = new DefaultHttpContext();
			var ms = new MemoryStream(Encoding.UTF8.GetBytes(@"<s:Envelope xmlns:s=""http://www.w3.org/2003/05/soap-envelope"" xmlns:a=""http://www.w3.org/2005/08/addressing""><s:Body></s:Body></s:Envelope>"));
			x.Request.Body = ms;
			SoapException result = Assert.ThrowsAny<SoapException>(() => new SoapEndpointMiddleware(null, null).GetSoapAction(x.Request));
			Assert.Contains("No soapAction found in request", result.Message);
			var s= result.GetAllExceptions();
			Assert.Contains("content", s);
		}

	}
}