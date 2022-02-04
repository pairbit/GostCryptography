using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace GostCryptography
{
    static class ExceptionUtility
	{
		private static readonly String logPath;

		static ExceptionUtility()
		{
			logPath = Environment.GetEnvironmentVariable("LogPath", EnvironmentVariableTarget.Process);

			if (logPath != null) logPath = Path.GetFullPath(logPath);
		}

		public static void Log(string msg, Exception ex = null)
		{
			if (logPath == null) return;

			var errors = new List<string>();
			if (ex != null) Messages(errors, ex);
			var errorAll = string.Join(Environment.NewLine, errors);
			var time = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
			msg = $"[{time}] " + msg;
			if (errors.Count > 0) msg += Environment.NewLine + errorAll;

			var dirInfo = new DirectoryInfo(logPath);
			if (!dirInfo.Exists) dirInfo.Create();

			File.AppendAllText(Path.Combine(logPath, "GostCryptography.log"), msg + Environment.NewLine);
		}

		private static void Messages(IList<string> errors, Exception ex)
		{
			errors.Add(ex.Message);
			if (ex.InnerException != null) Messages(errors, ex.InnerException);
		}

		public static ArgumentException Argument(string argument, string message = null, params object[] messageParameters)
		{
			return new ArgumentException(FormatErrorMessage(message, messageParameters), argument);
		}

		public static ArgumentNullException ArgumentNull(string argument, string message = null, params object[] messageParameters)
		{
			return new ArgumentNullException(argument, FormatErrorMessage(message, messageParameters));
		}

		public static ArgumentOutOfRangeException ArgumentOutOfRange(string argument, string message = null, params object[] messageParameters)
		{
			return new ArgumentOutOfRangeException(argument, FormatErrorMessage(message, messageParameters));
		}

		public static NotSupportedException NotSupported(string message = null, params object[] messageParameters)
		{
			return new NotSupportedException(FormatErrorMessage(message, messageParameters));
		}


		public static CryptographicException CryptographicException(int nativeError)
		{
			return new CryptographicException(nativeError);
		}

		public static CryptographicException CryptographicException(string message = null, params object[] messageParameters)
		{
			return new CryptographicException(FormatErrorMessage(message, messageParameters));
		}

		public static CryptographicException CryptographicException(Exception innerException, string message = null, params object[] messageParameters)
		{
			return new CryptographicException(FormatErrorMessage(message, messageParameters), innerException);
		}


		private static string FormatErrorMessage(string message, params object[] messageParameters)
		{
			return (message != null && messageParameters != null) ? string.Format(message, messageParameters) : message;
		}
	}
}