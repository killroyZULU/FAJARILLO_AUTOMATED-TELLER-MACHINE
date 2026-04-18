using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace FAJARILLO_AUTOMATED_TELLER_MACHINE.DISPLAY
{
    /// <summary>
    /// Interaction logic for PinBox.xaml
    /// </summary>
    public partial class PinBox : UserControl , INotifyPropertyChanged
    {

        private string pinBoxText;

        public event PropertyChangedEventHandler PropertyChanged;

        public string PinBoxText
        {
            get { return pinBoxText; }

            set 
            { 
                pinBoxText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PinBoxText"));

            }


        }

        public PinBox()
        {
            InitializeComponent();
            DataContext = this;
            pinBoxText = "Enter 6-digit PIN Number";
        }
    }
}
