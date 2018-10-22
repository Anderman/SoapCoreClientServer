using System.IO;
using System.Xml;

namespace SoapCore.SoapConvertor
{
	public class SoapReader : XmlTextReader
	{
		private readonly string _action;

		public SoapReader(Stream stream, string action) : base(new StreamReader(stream))
		{
			_action = action;
		}

		public override string NamespaceURI => "ns";
		public override string LocalName => base.LocalName == _action || base.LocalName == $"{_action}Response"
				? "MessageHeader"
				: base.LocalName == $"{_action}Result" || base.LocalName == $"{_action}Request"
					? "SoapMessage"
					: base.LocalName;
	}
}