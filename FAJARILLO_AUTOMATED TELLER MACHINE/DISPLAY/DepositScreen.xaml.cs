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
    /// Interaction logic for DepositScreen.xaml
    /// </summary>
    public partial class DepositScreen : UserControl, INotifyPropertyChanged
    {

        private string amountDeposit;

        public event PropertyChangedEventHandler PropertyChanged;

        public string AmountDeposit
        {
            get { return amountDeposit; }
            set 
            { 
                amountDeposit = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AmountDeposit"));
            }
        }

        public DepositScreen()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
