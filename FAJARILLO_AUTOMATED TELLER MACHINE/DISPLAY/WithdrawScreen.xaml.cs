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
    /// Interaction logic for WithdrawScreen.xaml
    /// </summary>
    public partial class WithdrawScreen : UserControl , INotifyPropertyChanged
    {

        /// <summary>
        /// Block of code for data binding to the text of the label for withdrawal. 
        /// </summary>
        /// 

        private string amountWithdraw = "";

        public event PropertyChangedEventHandler PropertyChanged;

        public string AmountWithdraw
        {
            get => amountWithdraw;
            set
            {
                amountWithdraw = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AmountWithdraw)));
            }
        }

        public WithdrawScreen()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
