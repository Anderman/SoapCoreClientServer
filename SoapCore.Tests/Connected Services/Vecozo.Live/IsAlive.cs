using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Threading.Tasks;
using System.Xml;
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
namespace SoapCore.Tests.Connected_Services.Vecozo.Live
{
	[DebuggerStepThrough]
	[GeneratedCode("dotnet-svcutil", "1.0.0.1")]
	[DataContract(Name = "IsAliveRequest", Namespace = "urn:www-vecozo-nl:messages:isalive:v1")]
	public class IsAliveRequest : object
	{
	}

	[DebuggerStepThrough]
	[GeneratedCode("dotnet-svcutil", "1.0.0.1")]
	[DataContract(Name = "IsAliveResponse", Namespace = "urn:www-vecozo-nl:messages:isalive:v1")]
	public class IsAliveResponse : object
	{
		[DataMember(IsRequired = true)]
		public bool Resultaat { get; set; }
	}

	[GeneratedCode("dotnet-svcutil", "1.0.0.1")]
	[ServiceContract(Namespace = "urn:www-vecozo-nl:isalive:v1", ConfigurationName = "ServiceReference1.IsAlive")]
	public interface IsAlive
	{
		[OperationContract(Action = "urn:www-vecozo-nl:v1:isalive", ReplyAction = "urn:www-vecozo-nl:v1:isaliveresponse")]
		Task<IsAliveResponse> IsAliveAsync(IsAliveRequest IsAliveRequest);
	}

	[GeneratedCode("dotnet-svcutil", "1.0.0.1")]
	public interface IsAliveChannel : IsAlive, IClientChannel
	{
	}

	[DebuggerStepThrough]
	[GeneratedCode("dotnet-svcutil", "1.0.0.1")]
	public partial class IsAliveClient : ClientBase<IsAlive>, IsAlive
	{
		public enum EndpointConfiguration
		{
			IsAlive11,

			IsAlive12
		}

		public IsAliveClient(EndpointConfiguration endpointConfiguration) :
			base(GetBindingForEndpoint(endpointConfiguration), GetEndpointAddress(endpointConfiguration))
		{
			Endpoint.Name = endpointConfiguration.ToString();
			ConfigureEndpoint(Endpoint, ClientCredentials);
		}

		public IsAliveClient(EndpointConfiguration endpointConfiguration, string remoteAddress) :
			base(GetBindingForEndpoint(endpointConfiguration), new EndpointAddress(remoteAddress))
		{
			Endpoint.Name = endpointConfiguration.ToString();
			ConfigureEndpoint(Endpoint, ClientCredentials);
		}

		public IsAliveClient(EndpointConfiguration endpointConfiguration, EndpointAddress remoteAddress) :
			base(GetBindingForEndpoint(endpointConfiguration), remoteAddress)
		{
			Endpoint.Name = endpointConfiguration.ToString();
			ConfigureEndpoint(Endpoint, ClientCredentials);
		}

		public IsAliveClient(Binding binding, EndpointAddress remoteAddress) :
			base(binding, remoteAddress)
		{
		}

		public Task<IsAliveResponse> IsAliveAsync(IsAliveRequest IsAliveRequest)
		{
			return Channel.IsAliveAsync(IsAliveRequest);
		}

		/// <summary>
		///     Implement this partial method to configure the service endpoint.
		/// </summary>
		/// <param name="serviceEndpoint">The endpoint to configure</param>
		/// <param name="clientCredentials">The client credentials</param>
		static partial void ConfigureEndpoint(ServiceEndpoint serviceEndpoint, ClientCredentials clientCredentials);

		public virtual Task OpenAsync()
		{
			return Task.Factory.FromAsync(((ICommunicationObject) this).BeginOpen(null, null), ((ICommunicationObject) this).EndOpen);
		}

		public virtual Task CloseAsync()
		{
			return Task.Factory.FromAsync(((ICommunicationObject) this).BeginClose(null, null), ((ICommunicationObject) this).EndClose);
		}

		private static Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
		{
			if (endpointConfiguration == EndpointConfiguration.IsAlive11)
			{
				var result = new BasicHttpBinding();
				result.MaxBufferSize = int.MaxValue;
				result.ReaderQuotas = XmlDictionaryReaderQuotas.Max;
				result.MaxReceivedMessageSize = int.MaxValue;
				result.AllowCookies = true;
				result.Security.Mode = BasicHttpSecurityMode.Transport;
				return result;
			}

			if (endpointConfiguration == EndpointConfiguration.IsAlive12)
			{
				var result = new CustomBinding();
				var textBindingElement = new TextMessageEncodingBindingElement();
				textBindingElement.MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None);
				result.Elements.Add(textBindingElement);
				var httpsBindingElement = new HttpsTransportBindingElement();
				httpsBindingElement.AllowCookies = true;
				httpsBindingElement.MaxBufferSize = int.MaxValue;
				httpsBindingElement.MaxReceivedMessageSize = int.MaxValue;
				result.Elements.Add(httpsBindingElement);
				return result;
			}

			throw new InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
		}

		private static EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
		{
			if (endpointConfiguration == EndpointConfiguration.IsAlive11) return new EndpointAddress("https://tstedpwebservice.vecozo.nl/Router.V1.svc/IndienenDeclaratieV1Soap11");
			if (endpointConfiguration == EndpointConfiguration.IsAlive12) return new EndpointAddress("https://tstedpwebservice.vecozo.nl/Router.V1.svc/IndienenDeclaratieV1");
			throw new InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
		}
	}
}