using System.IO;
using System.Xml.Serialization;

namespace SoapCore.SoapConvertor
{
	public static class SoapConvert
	{
		public static T Deserialize<T>(Stream soapMessage, string soapAction) where T : class, new()
		{
			var serializer = new XmlSerializer<DeserializeEnvelope<T>>();
			var xmlResult = (DeserializeEnvelope<T>)serializer.Deserialize(new SoapReader(soapMessage, soapAction));
			if(xmlResult.Body.MessageHeader.SoapMessage==null)
				xmlResult.Body.MessageHeader.SoapMessage=new T();
			return xmlResult.Body.MessageHeader.SoapMessage;
		}

		public static string Serialize<T>(T obj, string soapAction, string messageNameSpace, SoapType soapType)
		{
			var env = new SerializeEnvelope<T>();
			env.Body.MessageHeader.SoapMessage = obj;
			var serializer = new XmlSerializer<SerializeEnvelope<T>>();
			var sw = new Utf8StringWriter();
			var x = new SoapWriter(sw, soapAction, messageNameSpace, soapType);
			serializer.Serialize(x, env);
			var result = x.ToString();
			return result;
		}

		[XmlRoot(ElementName = "Envelope", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
		public class SerializeEnvelope<T>
		{
			public EnvelopeBody Body { get; set; } = new EnvelopeBody();

			public class EnvelopeBody
			{
				[XmlElement(Namespace = "ns")]
				public SoapMessageHeader MessageHeader { get; set; } = new SoapMessageHeader();

				public class SoapMessageHeader
				{
					public T SoapMessage { get; set; }
				}
			}
		}

		[XmlRoot(ElementName = "Envelope", Namespace = "ns")]
		public class DeserializeEnvelope<T> where T : new()
		{
			public EnvelopeBody Body { get; set; }

			public class EnvelopeBody
			{
				public SoapMessageHeader MessageHeader { get; set; }

				public class SoapMessageHeader
				{
					private T _soapMessage;

					public T SoapMessage
					{
						get => _soapMessage;
						set => _soapMessage = value!=null? value:new T();
					}
				}
			}
		}
	}
}