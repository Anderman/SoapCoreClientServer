using System;

namespace SoapCore.SoapServices
{
	public sealed class OperationContractAttribute : Attribute
	{
		public string Action { get; set; }
		public string Name { get; set; }
		public string ReplyAction { get; set; }
	}
}