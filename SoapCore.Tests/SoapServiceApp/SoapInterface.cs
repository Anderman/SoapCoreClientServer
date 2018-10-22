using System.Xml.Serialization;
using SoapCore.SoapServices;

// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

namespace SoapCore.Tests.SoapServiceApp
{
	public class IsAliveResponse : object
	{
		[XmlElement(Namespace = "urn:www-vecozo-nl:messages:isalive:v1")]
		public bool Resultaat { get; set; }
	}

	public class IsAliveRequest : object
	{
	}

	[ServiceContract(Namespace = "urn:www-vecozo-nl:isalive:v1")]
	public class AliveService
	{
		[OperationContract(Action = "urn:www-vecozo-nl:v1:isalive", ReplyAction = "urn:www-vecozo-nl:v1:isaliveresponse")]
		public IsAliveResponse IsAlive(IsAliveRequest request)
		{
			return new IsAliveResponse { Resultaat = true };
		}
	}
}