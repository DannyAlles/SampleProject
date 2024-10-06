using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCQRS.Infrastructure.Abstractions.RabbitMQ.Events
{
    public abstract class Event
    {
        public DateTime TimeStamps { get; protected set; }
        protected Event()
        {
            TimeStamps = DateTime.UtcNow;
        }
    }
}
