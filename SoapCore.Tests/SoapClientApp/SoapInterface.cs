using System.Xml.Serialization;

namespace SoapCore.Tests.SoapClientApp
{
	public class DownloadPdfRequest : object
	{
		[XmlElement(Namespace = "urn:www-vecozo-nl:messages:vsp:edp:declareren:downloaden:v1")]
		public long PdfId { get; set; }
	}
	public class DownloadPdfResponse : object
	{
		public PdfRetourbestand PdfRetourbestand { get; set; }
	}

	public class PdfRetourbestand : object
	{
		public Bestand Bestand { get; set; }
	}

	public class Bestand : object
	{
		public byte[] Data { get; set; }
	}
}