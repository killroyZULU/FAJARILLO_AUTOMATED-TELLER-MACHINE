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
    /// Interaction logic for MainDisplay.xaml
    /// </summary>
    public partial class MainDisplay : UserControl , INotifyPropertyChanged
    {
        /// <summary>
        /// Data binding for the text used for the initial text to be displayed upon startup of the program. 
        /// </summary>
        /// 

        public string UiText
        {
            get { return uiText; }

            set
            {
                uiText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UiText"));
            }
        }


        public string UiText2
        {
            get { return uiText2; }

            set
            {
                uiText2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UiText2"));
            }
        }

        public MainDisplay()
        {
            InitializeComponent();
            DataContext = this;
            uiText = "BANGKO DE FAJARILLO"; // Text binded
            uiText2 = "PLEASE INSERT CARD..."; //Text binded
        }

        private string uiText;
        private string uiText2;

        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Displays the pin entry box when called. 
        /// </summary>
        public void ShowPinEntry()
        {

            PinBoxUI.PinBoxText = "ENTER 6-DIGIT PIN NUMBER";
            PinBoxUI.Visibility = Visibility.Visible;   

        }

       



    }
}
