using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAJARILLO_AUTOMATED_TELLER_MACHINE.CREDIT_CARDS
{
    public class CardHolderDetails
    {
        public string CurrentBalance { get; set; }

        public CardHolderDetails(string currentBalance)
        {
            CurrentBalance = currentBalance;
        }
    }
}
