using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using SoapCore.Extensions;
using SoapCore.SoapConvertor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

// ReSharper disable StringLiteralTypo

namespace SoapCore.SoapServices
{
	public class SoapEndpointMiddleware
	{
		private const string FaultFrame = @"<s:Envelope xmlns:s=""http://www.w3.org/2003/05/soap-envelope""><s:Body><s:Fault><s:Code><s:Value>s:Sender</s:Value></s:Code><s:Reason><s:Text xml:lang="""">{0}</s:Text></s:Reason><s:Detail></s:Detail></s:Fault></s:Body></s:Envelope>";

		private readonly RequestDelegate _next;
		private readonly Dictionary<string, OperationDescription> _service;
		private ILogger _logger;

		public SoapEndpointMiddleware(RequestDelegate next, Dictionary<string, OperationDescription> services)
		{
			_next = next;
			_service = services;
		}

		// ReSharper disable once UnusedMember.Global
		public async Task Invoke(HttpContext httpContext, IServiceProvider serviceProvider)
		{
			if (httpContext.Request.ContentType != null && httpContext.Request.ContentType.Contains("application/soap+xml", StringComparison.OrdinalIgnoreCase))
			{
				_logger = (ILogger)serviceProvider.GetService(typeof(ILogger<SoapEndpointMiddleware>));
				try
				{
					ExecuteSoapRequest(httpContext, serviceProvider);
				}
				catch (Exception e)
				{
					SendFault(httpContext, e);
				}
			}
			else
			{
				await _next(httpContext);
			}
		}

		private void ExecuteSoapRequest(HttpContext httpContext, IServiceProvider serviceProvider)
		{
			var operation = GetOperation(httpContext.Request);

			//Deserialize
			var parameterType = operation.DispatchMethod.GetParameters().Single().ParameterType;
			var request = Deserialize(httpContext, parameterType, operation.Name);

			// Invoke Operation method
			var serviceInstance = serviceProvider.GetService(_service[operation.SoapAction].ServiceType);
			var responseObject = operation.DispatchMethod.Invoke(serviceInstance, new[] { request });

			//Serialize
			var nameSpace = operation.Namespace;
			var response = Serialize(responseObject, nameSpace, operation.Name);

			//Send response
			httpContext.Response.ContentType = "application/soap+xml; charset=utf-8";
			httpContext.Response.Body.Write(Encoding.UTF8.GetBytes(response));
		}

		private string Serialize(object responseObject, string nameSpace, string soapActionElementName)
		{
			var response = (string)typeof(SoapConvert).GetMethod("Serialize").MakeGenericMethod(responseObject.GetType()).Invoke(null, new[] { responseObject, soapActionElementName, nameSpace, SoapType.Response });
			_logger?.LogDebug(response);
			return response;
		}

		private static object Deserialize(HttpContext httpContext, Type type, string soapActionElementName)
		{
			return typeof(SoapConvert).GetMethod("Deserialize").MakeGenericMethod(type).Invoke(null, new object[] { httpContext.Request.Body, soapActionElementName });
		}

		private OperationDescription GetOperation(HttpRequest httpRequest)
		{
			var soapAction = GetSoapAction(httpRequest);
			if (!_service.TryGetValue(soapAction, out var operation))
				throw new InvalidOperationException($"No operation found for specified action: {soapAction}");
			return operation;
		}

		public string GetSoapAction(HttpRequest request)
		{
			return GetSoapAction12(request)
				   ?? GetSoapAction11(request)
				   ?? GetSoapAction12Other(request)
				   ?? throw new SoapException("No soapAction found in request", new Dictionary<string, string>
						{
							{ "header", request.Headers["SOAPAction"].FirstOrDefault() },
							{ "contenttype", request.ContentType },
							{ "soap body", request.Body.ToText() },
						}
				   );
		}

		private string GetSoapAction12Other(HttpRequest request)
		{
			//other addressing specify their soap action in the header/Action node
			request.EnableRewind();
			var bodyStr = request.Body.ToText();

			_logger?.LogDebug(bodyStr);

			var ms = new MemoryStream(Encoding.UTF8.GetBytes(bodyStr));
			var doc = XDocument.Load(ms);
			var node = doc.DescendantNodes().OfType<XElement>().Skip(2).FirstOrDefault(x => x.Name.LocalName == "Action" && x.Parent?.Name.LocalName == "Header");

			return node?.Value;
		}

		private static string GetSoapAction11(HttpRequest request)
		{
			var soapAction = request.Headers["SOAPAction"].FirstOrDefault()?.Trim('"');
			return string.IsNullOrEmpty(soapAction)
				? null
				: soapAction;
		}

		private static string GetSoapAction12(HttpRequest request)
		{
			//soapAction in content-type in like ;action="someAction"
			foreach (var s in request.Headers["Content-Type"])
			{
				var start = 0;
				var action = false;
				for (var i = 5; i < s.Length; i++)
				{
					if (s[i] == ' ') continue;
					if (!action && s[i - 5] == 'a' && s[i - 4] == 'c' && s[i - 3] == 't' && s[i - 2] == 'i' && s[i - 1] == 'o' && s[i + 0] == 'n') action = true;
					if (action && start != 0 && s[i] == '"') return s.Substring(start, i - start);
					if (action && start == 0 && s[i] == '"') start = i + 1;
				}
			}

			return null;
		}

		private void SendFault(HttpContext httpContext, Exception exception)
		{
			var message = exception.GetAllExceptions();
			_logger.LogError(exception, message);
			var faultFrame = string.Format(FaultFrame, Escape(message));

			httpContext.Response.ContentType = "application/soap+xml; charset=utf-8";
			httpContext.Response.Body.Write(Encoding.UTF8.GetBytes(faultFrame));
		}

		private string Escape(string s)
		{
			return s.Replace("'", "_").Replace("\"", "_").Replace("&", "_").Replace("<", "_");
		}
	}
}