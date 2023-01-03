using System;
using System.Net;
using System.Runtime.Serialization;

namespace Chat.Model.Exceptions
{
    public class MessageNotDeliveredException : ChatBaseException
    {
        public MessageNotDeliveredException(EndPoint endPoint): this($"Sorry! Message for {endPoint.ToString()} was not delivered.")
        {
        }

        public MessageNotDeliveredException(string message) : base(message)
        {
        }

        public MessageNotDeliveredException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MessageNotDeliveredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}