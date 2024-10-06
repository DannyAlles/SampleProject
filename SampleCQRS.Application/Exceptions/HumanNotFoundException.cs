using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SampleCQRS.Application.Exceptions
{
    [Serializable]
    public class HumanNotFoundException : Exception
    {
        public HumanNotFoundException()
        {
        }

        public HumanNotFoundException(string? message) : base(message)
        {
        }

        public HumanNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected HumanNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
