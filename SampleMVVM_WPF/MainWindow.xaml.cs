using SampleMVVM_WPF.Interfaces;
using SampleMVVM_WPF.Utilities;
using SampleMVVM_WPF.ViewModels;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SampleMVVM_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDispatcherView
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            DataContext = mainWindowViewModel;
            _mainWindowViewModel = mainWindowViewModel ?? throw new ArgumentNullException(nameof(mainWindowViewModel));
            _mainWindowViewModel.DispatcherView = this;

            InitializeComponent();
        }

        public void Invoke(Action action)
        {
            Dispatcher.BeginInvoke(delegate ()
            {
                action.Invoke();
            });
        }
    }
}