using System;

namespace VRChatPictureTagger.Core.Exceptions
{
	public class LoggedException : Exception
	{
		public bool IsLogged { get; set; }

		public LoggedException() { }
		public LoggedException(string message) : base(message) { }

		public LoggedException(bool isLogged) => IsLogged = isLogged;

		public LoggedException(string message, bool isLogged) : base(message) => IsLogged = isLogged;

		public LoggedException(string message, Exception innerException) : base(message, innerException) { }

		public LoggedException(string message, bool isLogged, Exception innerException) : base(message, innerException) => IsLogged = isLogged;
	}
}
