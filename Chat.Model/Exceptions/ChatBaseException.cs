using System;
using System.Runtime.Serialization;

namespace Chat.Model.Exceptions
{ 
    public class ChatBaseException : ApplicationException
    {
        public ChatBaseException()
        {
        }

        public ChatBaseException(string message) : base(message)
        {
        }

        public ChatBaseException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected ChatBaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
