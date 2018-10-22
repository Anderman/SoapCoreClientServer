using System.IO;
using System.Text;

namespace SoapCore.SoapConvertor
{
	public class Utf8StringWriter : StringWriter
	{
		public override Encoding Encoding => Encoding.UTF8;
	}
}