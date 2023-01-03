using System;
using System.Runtime.Serialization;

namespace Chat.Model.Exceptions
{
    public class UserNotDefinedException : ChatBaseException
    {
        public UserNotDefinedException()
        {
        }

        public UserNotDefinedException(string message) : base(message)
        {
        }

        public UserNotDefinedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserNotDefinedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
