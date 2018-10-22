using System;
using System.Collections;
using System.Text;

namespace SoapCore.SoapServices
{
	public static class ExceptionExtensions
	{
		public static string GetAllExceptions(this Exception ex)
		{
			var sb = new StringBuilder();
			var i = 0;
			do
			{
				sb.AppendLine(ex.Message);
				i++;
				foreach (DictionaryEntry entry in ex.Data)
					sb.AppendLine($"{"".IdentLine(i)}{entry.Key}={entry.Value}");
				sb.AppendLine(ex.StackTrace.IdentLines(i));
				ex = ex.InnerException;
			} while (ex != null);

			return sb.ToString();
		}

		private static string IdentLine(this string s, int i)
		{
			return s.PadRight(i + 1);
		}

		public static string IdentLines(this string s, int ident)
		{
			return s.Replace("\r\n", "\r").Replace("\r", $"\r{"".PadRight(ident + 1)}");
		}
	}
}