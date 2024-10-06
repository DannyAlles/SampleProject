using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SampleMVVM_WPF.Views
{
    /// <summary>
    /// Логика взаимодействия для HumanCardListViewUC.xaml
    /// </summary>
    public partial class HumanCardListViewUC : UserControl
    {
        public HumanCardListViewUC()
        {
            InitializeComponent();
        }

        public string FirstName
        {
            get { return (string)GetValue(FirstNameProperty); }
            set { SetValue(FirstNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FirstName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FirstNameProperty =
            DependencyProperty.Register(nameof(FirstName), typeof(string), typeof(HumanCardListViewUC), new PropertyMetadata(String.Empty));



        public string LastName
        {
            get { return (string)GetValue(LastNameProperty); }
            set { SetValue(LastNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LastName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LastNameProperty =
            DependencyProperty.Register(nameof(LastName), typeof(string), typeof(HumanCardListViewUC), new PropertyMetadata(String.Empty));



        public string MiddleName
        {
            get { return (string)GetValue(MiddleNameProperty); }
            set { SetValue(MiddleNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MiddleName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MiddleNameProperty =
            DependencyProperty.Register(nameof(MiddleName), typeof(string), typeof(HumanCardListViewUC), new PropertyMetadata(String.Empty));



        public DateTime DateOfBirth
        {
            get { return (DateTime)GetValue(DateOfBirthProperty); }
            set { SetValue(DateOfBirthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DateOfBirth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DateOfBirthProperty =
            DependencyProperty.Register(nameof(DateOfBirth), typeof(DateTime), typeof(HumanCardListViewUC), new PropertyMetadata(DateTime.MinValue));
    }
}
