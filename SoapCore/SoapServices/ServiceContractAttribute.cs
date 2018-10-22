using System;

namespace SoapCore.SoapServices
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
	public sealed class ServiceContractAttribute : Attribute
	{
		// ReSharper disable once UnusedMember.Global default attribute on WCF
		public string ConfigurationName { get; set; }
		public string Namespace { get; set; }
	}
}