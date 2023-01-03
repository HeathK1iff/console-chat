using System;
using System.Runtime.Serialization;

namespace Chat.Model.Exceptions
{
    public class LocalIPNotFoundException : ChatBaseException
    {
        public LocalIPNotFoundException() : this($"Local IP address is not assigned. Check your network adapters")
        {

        }

        public LocalIPNotFoundException(string message) : base(message)
        {
        }

        public LocalIPNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LocalIPNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
