using System;
using System.Reflection;

namespace SoapCore.SoapServices
{
	public class OperationDescription
	{
		public Type ServiceType { get; set; }
		public MethodInfo DispatchMethod { get; set; }
		public string SoapAction { get; set; }
		public string Namespace { get; set; }
		public string Name { get; set; }
	}
}