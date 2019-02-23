using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using SoapCore.Extensions;
using SoapCore.SoapConvertor;

namespace SoapCore.SoapClient
{
	public class SoapClient<TConfig, TRequest, TResponse> where TConfig : ISoapConfig, new() where TRequest : class where TResponse : class, new()
	{
		private readonly HttpClient _client;
		private readonly TConfig _config;
		private readonly IHostingEnvironment _env;
		private readonly ILogger<SoapClient<TConfig, TRequest, TResponse>> _logger;
		public string Url;

		public SoapClient(IHostingEnvironment env, ILogger<SoapClient<TConfig, TRequest, TResponse>> logger, ICertificateProvider certificateProvider)
		{
			_env = env;
			_logger = logger;
			_config = new TConfig();
			_client = new HttpClient(new HttpClientHandler { ClientCertificates = { certificateProvider.Certificate2 } });
		}

		public async Task<TResponse> PostAsync(TRequest request)
		{
			var xmlContent = GetSoapContent(request);
			var result = await _client.PostAsync(Url ?? _config.GetUrl(_env), xmlContent);
			var stream = await result.Content.ReadAsStreamAsync();
			_logger.Log(LogLevel.Debug, $"Soap response:{stream.ToText()}");

			result.EnsureSuccessStatusCode();
			return SoapConvert.Deserialize<TResponse>(stream, _config.SoapActionElementName);
		}

		private StringContent GetSoapContent(TRequest request)
		{
			var xmlString = SoapConvert.Serialize(request, _config.SoapActionElementName, _config.Namespace, SoapType.Request);

			_logger.Log(LogLevel.Debug, $"Soap request:{xmlString}");

			var stringContent = new StringContent(xmlString) { Headers = { ContentType = { MediaType = "application/soap+xml" } } };
			stringContent.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("action", $@"""{_config.SoapAction}"""));

			return stringContent;
		}
	}
}