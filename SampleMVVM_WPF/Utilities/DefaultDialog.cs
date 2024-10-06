using Microsoft.Win32;
using SampleMVVM_WPF.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SampleMVVM_WPF.Utilities
{
    public class DefaultDialog : IDialog
    {
        public void ShowMessage(string title, string message, MessageBoxButton button, MessageBoxImage image)
        {
            MessageBox.Show(message, title, button, image);
        }
    }
}
