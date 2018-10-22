using System.IO;
using System.Text;

namespace SoapCore.Extensions
{
	public static class StreamExtensions
	{
		public static string ToText(this Stream stream)
		{
			var s = new StreamReader(stream, Encoding.UTF8, true, 1024, true).ReadToEnd();
			stream.Position = 0;
			return s;
		}
	}
}