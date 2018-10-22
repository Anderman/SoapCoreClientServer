using System.Xml.Serialization;

namespace SoapCore.SoapConvertor
{
	public class XmlSerializer<T> : XmlSerializer
	{
		public XmlSerializer() : base(typeof(T)) { }
	}
}