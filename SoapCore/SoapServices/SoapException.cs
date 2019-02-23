using System;
using System.Collections.Generic;

namespace SoapCore.SoapServices
{
	public sealed class SoapException : Exception
	{
		public SoapException(string s, Dictionary<string, string> errors) : base(s)
		{
			foreach (var (key, value) in errors) Data.Add(key, value);
		}

		public SoapException(string s) : base(s)
		{
		}
	}
}