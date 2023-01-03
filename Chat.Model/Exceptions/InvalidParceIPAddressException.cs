using System;
using System.Runtime.Serialization;

namespace Chat.Model.Exceptions
{
    public class InvalidParceIPAddressException : ChatBaseException
    {
        public InvalidParceIPAddressException()
        {
        }

        public InvalidParceIPAddressException(string message) : base(message)
        {
        }

        public InvalidParceIPAddressException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidParceIPAddressException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
