using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SampleMVVM_WPF.Interfaces
{
    public interface IDialog
    {
        void ShowMessage(string title, string message, MessageBoxButton button, MessageBoxImage image);   
    }
}
