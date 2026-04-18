using FAJARILLO_AUTOMATED_TELLER_MACHINE.CREDIT_CARDS;
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
    /// Interaction logic for SavingsWithdrawDepositExit.xaml
    /// </summary>
    public partial class SavingsWithdrawDepositExit : UserControl 
    {

        public SavingsWithdrawDepositExit()
        {
            DataContext = this;
            InitializeComponent();
            
        }


        public void UpdateBalanceDisplay(string balance)
        {
            Balance.Text = balance;  // 'Balance' is your TextBlock name in XAML
        }

        private void DepositButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ShowDepositScreen();
            }
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Do you want to exit and eject your card?", "Exit Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.EjectCard(); // This assumes you’ve implemented the EjectCard() method in MainWindow
                }
            }
        }
         
        private void WithdrawButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ShowWithdrawScreen();
            }
        }

    }
}
