using System.IO;
using System.Text;
using System.Xml;

namespace SoapCore.SoapConvertor
{
	public class SoapWriter : XmlTextWriter
	{
		private readonly string _action;
		private readonly string _ns;
		private readonly SoapType _soapType;
		private readonly StringWriter _sw;

		public SoapWriter(StringWriter sw, string action, string ns, SoapType soapType) : base(sw)
		{
			_sw = sw;
			_action = action;
			_ns = ns;
			_soapType = soapType;
		}
		public SoapWriter(Stream w, Encoding encoding) : base(w, encoding)
		{
		}

		public SoapWriter(TextWriter w) : base(w)
		{
		}

		public SoapWriter(string filename, Encoding encoding) : base(filename, encoding)
		{
		}

		public override string ToString()
		{
			return _sw.ToString();
		}
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			if (localName == "MessageHeader") localName = _soapType==SoapType.Request? _action: $"{_action}Response";
			if (localName == "SoapMessage") localName = _soapType == SoapType.Request ? $"{_action}Request" : $"{_action}Result"; 
			if (ns == "ns") ns = _ns;
			base.WriteStartElement(prefix, localName, ns);
		}
	}

	public enum SoapType { Request,Response}
}