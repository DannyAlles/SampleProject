using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMVVM_WPF.Interfaces
{
    public interface IDispatcherView
    {
        void Invoke(Action action);
    }
}
