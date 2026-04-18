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
using Microsoft.Win32;
using System.IO;
using System.ComponentModel;
using FAJARILLO_AUTOMATED_TELLER_MACHINE.DISPLAY;
using FAJARILLO_AUTOMATED_TELLER_MACHINE.CREDIT_CARDS;
using System.Windows.Threading;

namespace FAJARILLO_AUTOMATED_TELLER_MACHINE
{

    
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window 
    {

        //Declarations
        private List <string> PinEntry = new List <string> ();
        private List<string> CreditCardHolderDetails = new List<string>();
        private string CreditCardFilePath;
        private CardHolderDetails TotalBalance;
            
        private bool isErrorShown = false;
        private DispatcherTimer inactivityTimer; 

        /// <summary>
        /// Main constructor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            inactivityTimer = new DispatcherTimer();
            //inactivityTimer.Interval = TimeSpan.FromSeconds(30);
            //inactivityTimer.Tick += InactivityTimer_Tick;

        }

        //BUTTONS
        //==========================================================================================================================================================================================================================


        /// <summary>
        /// Opens the filtered file picker to select from simulated credit cards. The stream reader reads each line of the text 
        /// and stores it in a list to be accessed for the pin ent5ry logic and the withdraw and deposit functionalities.
        /// </summary>
        /// <param name="sender"> Insert Button Clicked </param>
        /// <param name="e"> Event data associated with the button click </param>
        private void InsertCardButton_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Credit Card | *.txt";
            fileDialog.InitialDirectory = "C:\\Users\\Lee Adrian\\source\\repos\\FAJARILLO_AUTOMATED TELLER MACHINE\\FAJARILLO_AUTOMATED TELLER MACHINE\\CREDIT CARDS\\";
            fileDialog.Title = "Select card you want to enter";


            bool? success = fileDialog.ShowDialog();
            if (success == true)
            {
                CreditCardFilePath = fileDialog.FileName;
                string path = fileDialog.FileName;
                var text = File.ReadAllText(path);
                var lines = File.ReadAllLines(path);


                //READS THE CARD
                try
                {

                    using (StreamReader sr = new StreamReader(path))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            CreditCardHolderDetails.Add(line); //ADDS THE CARD DETAILS TO THE LIST
                        }
                    }
                }
                catch (Exception error)
                {
                    // Let the user know what went wrong.
                    Console.WriteLine("The card could not be read:");
                    Console.WriteLine(error.Message);
                }
                InputButtons.IsEnabled = true;
                Numbers.IsEnabled = true;
                BlankPlates.IsEnabled = true;
                TotalBalance = new CardHolderDetails(CreditCardHolderDetails[2]);
                MainDisplayControl.ShowPinEntry();
                InsertCardButton.IsEnabled = false;
                ResetInactivityTimer();

            }
            else
            {

            }

        }



        /// <summary>
        /// Checks which screen is currently  visible to the user and the functionality to be called of the number buttons
        /// changes depending on what screen is being displayed.
        /// </summary>
        /// <param name="sender"> Numbers button clicked </param>
        /// <param name="e"> Event data associated with the button click </param>
        private void Button_ClickNumbers(object sender, RoutedEventArgs e)
        {
            ResetInactivityTimer();
            if (DepositControl.Visibility == Visibility.Visible)
            {
                DepositAmountEntry(sender, e);
            }
            else if (WithdrawControl.Visibility == Visibility.Visible)
            {
                WithdrawAmountEntry(sender, e);
            }
            else
            {
                PinNumberEntry(sender, e);
            }
        }

        /// <summary>
        /// Checks which screen is currently  visible to the user and the functionality to be called of the Enter button
        /// changes depending on what screen is being displayed.
        /// </summary>
        /// <param name="sender"> Enter button clicked </param>
        /// <param name="e"> Event data associated with the button click </param>
        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            ResetInactivityTimer();
            if (sender is Button clickedButton)
            {
                string buttonContent = clickedButton.Content.ToString();

                if (DepositControl.Visibility == Visibility.Visible)
                {
                    if (buttonContent == "Enter")
                        ProcessDeposit();
                    else
                        DepositAmountEntry(sender, e);
                }
                else if (WithdrawControl.Visibility == Visibility.Visible)
                {
                    if (buttonContent == "Enter")
                        ProcessWithdraw();
                    else
                        WithdrawAmountEntry(sender, e);
                }
                else
                {
                    // PIN entry mode
                    if (buttonContent == "Enter")
                        ProcessPinEntry();
                    else
                        PinNumberEntry(sender, e);
                }
            }
        }

        /// <summary>
        /// Checks which screen is currently  visible to the user and the functionality to be called of the Clear button
        /// changes depending on what screen is being displayed.
        /// </summary>
        /// <param name="sender"> Clear button is clicked </param>
        /// <param name="e"> Event Associated with the button click </param>
        private void ClearPinNumber_Click(object sender, RoutedEventArgs e)
        {
            ResetInactivityTimer();

            if (DepositControl.Visibility == Visibility.Visible)
            {
                DepositControl.AmountDeposit = "";
            }
            else if (WithdrawControl.Visibility == Visibility.Visible)
            {
                WithdrawControl.AmountWithdraw = "";
            }
            else if (MainDisplayControl.Visibility == Visibility.Visible)
            {
                MainDisplayControl.PinBoxUI.PinBoxText = "";
                isErrorShown = false;
            }
        }

        //BUTTON LOGIC
        //==========================================================================================================================================================================================================================\

        /// <summary>
        /// This method handles the number buttons when clicked during the PIN Entry process.
        /// This method also appends the clicked number to the current pin number input.
        /// Makes sure that the user is only able to input 6-digits.
        /// </summary>
        /// <param name="sender"> Number buttons clicked </param>
        /// <param name="e"> Event data associated with the button clicks </param>
        private void PinNumberEntry(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButtonNumber)
            {

                string buttonContent = clickedButtonNumber.Content.ToString();
                string CurrentPinNumber = MainDisplayControl.PinBoxUI.PinBoxText;

                if (isErrorShown || CurrentPinNumber == "ENTER 6-DIGIT PIN NUMBER")
                {
                    CurrentPinNumber = "";
                    isErrorShown = false;
                }

                if (CurrentPinNumber == "ENTER 6-DIGIT PIN NUMBER")
                {
                    CurrentPinNumber = "";
                }

                //allows for only a 6-digit input for the PIN number
                if (CurrentPinNumber.Length < 6)
                {
                    MainDisplayControl.PinBoxUI.PinBoxText = CurrentPinNumber + buttonContent;
                }

            }
        }


        /// <summary>
        /// This method handles the number buttons when clicked during the deposit transaction.
        /// This method also appends the clicked number to the current deposit amount input.
        /// Makes sure that there is a limit to the amount to deposit.
        /// </summary>
        /// <param name="sender"> The button that was clicked </param>
        /// <param name="e"> Event data associated </param>
        private void DepositAmountEntry(object sender, RoutedEventArgs e)
        {
            if (DepositControl.Visibility == Visibility.Visible && sender is Button clickedButtonNumber)
            {
                string buttonContent = clickedButtonNumber.Content.ToString();
                string currentAmount = DepositControl.AmountDeposit ?? "";

                if (currentAmount.Length < 9)
                {
                    DepositControl.AmountDeposit = currentAmount + buttonContent;
                }
            }
        }


        /// <summary>
        /// This method handles the number buttons when clicked during the withdraw transaction.
        /// This method also appends the clicked number to the current withdrawal amount input.
        /// Makes sure that there is a limit to the amount to withdraw.
        /// </summary>
        /// <param name="sender"> The button that was clicked </param>
        /// <param name="e"> Event data associated </param>
        private void WithdrawAmountEntry(object sender, RoutedEventArgs e)
        {
            if (WithdrawControl.Visibility == Visibility.Visible && sender is Button clickedButtonNumber)
            {
                string buttonContent = clickedButtonNumber.Content.ToString();
                string currentAmount = WithdrawControl.AmountWithdraw ?? "";

                if (currentAmount.Length < 9)
                {
                    WithdrawControl.AmountWithdraw = currentAmount + buttonContent;
                }
            }
        }

        /// <summary>
        /// Method that stores the pin entereed by the user in a string to be checked alongside the pin read by the stream read on teh credit card text file.
        /// </summary>
        private void ProcessPinEntry()
        {
            string PinInput = MainDisplayControl.PinBoxUI.PinBoxText;

            if (PinInput.Length != 6)
            {
                MainDisplayControl.PinBoxUI.PinBoxText = "ENTER 6-DIGIT PIN NUMBER";
                isErrorShown = true;
                return;
            }

            if (PinInput == CreditCardHolderDetails[1])  // index 1 = PIN in your text file
            {
                MessageBox.Show("PIN CORRECT");

                savingsControl.UpdateBalanceDisplay(TotalBalance.CurrentBalance);

                MainDisplayControl.Visibility = Visibility.Hidden;
                savingsControl.Visibility = Visibility.Visible;

                // Clear pin box for next time
                MainDisplayControl.PinBoxUI.PinBoxText = "";
            }
            else
            {
                MainDisplayControl.PinBoxUI.PinBoxText = "INCORRECT PIN, TRY AGAIN";
                isErrorShown = true;
            }
        }

        //DISPLAY
        //==========================================================================================================================================================================================================================

        /// <summary>
        /// Method that hides the other screens and shows the deposit screen when called.
        /// </summary>
        public void ShowDepositScreen()
        {
            DepositControl.Visibility = Visibility.Visible;
            WithdrawControl.Visibility = Visibility.Hidden;
            DepositControl.AmountDeposit = "";

        }

        /// <summary>
        /// Method that hides the other screens and shows the withdraw screen when called.
        /// </summary>
        public void ShowWithdrawScreen()
        {
            WithdrawControl.Visibility = Visibility.Visible;
            DepositControl.Visibility = Visibility.Hidden;
            WithdrawControl.AmountWithdraw = "";
        }

        //WITHDRAW AND DEPOSIT LOGIC
        //==========================================================================================================================================================================================================================

        /// <summary>
        /// A Method that when called converts the input of the user on how much to deposit to decimal then 
        /// simulates the deposit process including checking of current savings of the user before proceeding 
        /// to add to the overall current savings of the user. 
        /// </summary>
        private void ProcessDeposit()
        {
            if (decimal.TryParse(DepositControl.AmountDeposit, out decimal depositAmount))
            {
                if (depositAmount > 0)
                {
                    if (decimal.TryParse(TotalBalance.CurrentBalance, out decimal currentBalance))
                    {
                        decimal newBalance = currentBalance + depositAmount;
                        TotalBalance.CurrentBalance = newBalance.ToString("F2");

                        savingsControl.UpdateBalanceDisplay(TotalBalance.CurrentBalance);

                        SaveBalanceToFile(TotalBalance.CurrentBalance);

                        // Clear deposit input
                        DepositControl.AmountDeposit = "";

                        // Switch back to savings screen
                        DepositControl.Visibility = Visibility.Hidden;
                        savingsControl.Visibility = Visibility.Visible;

                        EjectCard();

                    }
                    else
                    {
                        MessageBox.Show("Invalid current balance format.");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter an amount greater than zero.");
                }
            }
            else
            {
                MessageBox.Show("Invalid deposit amount entered.");
            }
        }

        /// <summary>
        /// Method that when called converts the input of the user on how much to withdraw to decimal then 
        /// simulates the withdrawal process including checking of current savings of the user before proceeding 
        /// to deduct to the overall current savings of the user. 
        /// </summary>
        private void ProcessWithdraw()
        {
            if (decimal.TryParse(WithdrawControl.AmountWithdraw, out decimal withdrawAmount))
            {
                if (withdrawAmount <= 0)
                {
                    MessageBox.Show("Please enter an amount greater than zero.");
                    return;
                }

                if (!decimal.TryParse(TotalBalance.CurrentBalance, out decimal currentBalance))
                {
                    MessageBox.Show("Invalid current balance format.");
                    return;
                }

                if (withdrawAmount > currentBalance)
                {
                    MessageBox.Show("Insufficient balance.");
                    return;
                }

                
                decimal newBalance = currentBalance - withdrawAmount;
                TotalBalance.CurrentBalance = newBalance.ToString("F2");
                savingsControl.UpdateBalanceDisplay(TotalBalance.CurrentBalance);

                SaveBalanceToFile(TotalBalance.CurrentBalance);

                WithdrawControl.AmountWithdraw = "";
                WithdrawControl.Visibility = Visibility.Hidden;
                savingsControl.Visibility = Visibility.Visible;

                EjectCard(); //ejects the card
            }
            else
            {
                MessageBox.Show("Invalid withdraw amount entered.");
            }
        }

        /// <summary>
        /// Stream writer to save the new balance computed after the user either deposits or withdraws from the account. n
        /// </summary>
        /// <param name="newBalance"> The current / updated balance after a user either deposits or withdraws from the account </param>
        private void SaveBalanceToFile(string newBalance)
        {
            try
            {
                var lines = File.ReadAllLines(CreditCardFilePath);

                // Assuming the balance is at line index 2, update it
                lines[2] = newBalance;

                using (StreamWriter writer = new StreamWriter(CreditCardFilePath, false)) // overwrite
                {
                    foreach (var line in lines)
                    {
                        writer.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving balance: {ex.Message}");
            }
        }

        /// <summary>
        /// Method that resets the state of the program when called. Simulates the card being ejected after each successful transaction. 
        /// </summary>
        public void EjectCard()
        {
            MessageBox.Show("Transaction complete. Card ejected.");

            CreditCardHolderDetails.Clear();
            TotalBalance = null;
            CreditCardFilePath = null;
            PinEntry.Clear();

            InsertCardButton.IsEnabled = true;

            // Hide all screens
            MainDisplayControl.Visibility = Visibility.Visible;
            savingsControl.Visibility = Visibility.Hidden;
            DepositControl.Visibility = Visibility.Hidden;
            WithdrawControl.Visibility = Visibility.Hidden;
            MainDisplayControl.PinBoxUI.Visibility = Visibility.Hidden;

            // Reset PIN entry display
            MainDisplayControl.PinBoxUI.PinBoxText = "ENTER 6-DIGIT PIN NUMBER";

            // Disable number buttons until a card is inserted again
            InputButtons.IsEnabled = false;
            Numbers.IsEnabled = false;
            BlankPlates.IsEnabled = false;
        }










        //REST OF THE CODE USED IN A PREVIOUS ACTIVITY FOR A PREVIOUS SUBJECT. 



        //APP STATE RESET
        //==========================================================================================================================================================================================================================

        /// <summary>
        /// A method that resets the system to its initial state (used along with the dispatcher timer) 
        /// </summary>
        private void ResetToInitialState()
        {
            // Reset views
            MainDisplayControl.Visibility = Visibility.Visible;
            savingsControl.Visibility = Visibility.Hidden;
            DepositControl.Visibility = Visibility.Hidden;
            WithdrawControl.Visibility = Visibility.Hidden;
            MainDisplayControl.PinBoxUI.Visibility = Visibility.Hidden;

            // Enable insert card
            InsertCardButton.IsEnabled = true;

            // Disable other inputs
            InputButtons.IsEnabled = false;
            Numbers.IsEnabled = false;
            BlankPlates.IsEnabled = false;

            // Clear session data
            CreditCardHolderDetails.Clear();
            PinEntry.Clear();
            CreditCardFilePath = null;
            TotalBalance = null;

            
        }


        //INACTIVITY TIMER
        //==========================================================================================================================================================================================================================

        /// <summary>
        /// Restes the inactivity timer when an action or input by the user has been detected. (e.g. Button Clicks)
        /// </summary>
        private void ResetInactivityTimer()
        {
            inactivityTimer.Stop();
            inactivityTimer.Start();
        }

        /// <summary>
        /// HThis method handles the tick event of the inactivity timer. This method is triggered
        /// when the user has not interacted with the ATM for a specific amoutn of time. The timer stops and 
        /// a timeouit warning would be displayed. The atm is then reset to its initial state, ejects the card, and readies
        /// the system for the next user.
        ///
        /// </summary>
        /// <param name="sender"> Source of the event (timer) </param>
        /// <param name="e"> Event data assoicated </param>
        private void InactivityTimer_Tick(object sender, EventArgs e)
        {
            inactivityTimer.Stop();
            MessageBox.Show("Session timed out due to inactivity. Card will be ejected.", "Session Timeout", MessageBoxButton.OK, MessageBoxImage.Warning);
            ResetToInitialState();  // Use your existing card ejection logic
        }


    }
}
