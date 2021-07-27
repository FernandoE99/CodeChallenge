using System;
using System.Collections.Generic;
using System.Linq;

namespace LuxoftCodeChallenge
{
    public class Cashier
    {
        //Here we are defining what we will be using all along the application

        //To implement a new currency, please head over to Project Properties --> Debug
        //Make sure the currency is in Upper Case and that the currency denomination
        //is as shown below, without spaces or letters and each denomination has to be separated by a comma.
        //0.01,0.05,0.10,0.25,0.50,1.00,2.00,5.00,10.00,20.00,50.00,100.00
        //Currency has to be inputted in order from the smallest denomination to the largest.

        //On Application arguments enter the currency that will be used like so: USD,
        //If you want to change it, remove the old currency first. To add the currency denomination
        //in the same page, go to Environment Variables, in 'Name' you will input the currency
        //and in 'Value' you will input the currency denomination like so:
        //0.01,0.05,0.10,0.25,0.50,1.00,2.00,5.00,10.00,20.00,50.00,100.00
        private readonly decimal[] _currencyDenomination;
        private readonly int _denominationCount;
        private readonly string _countryCurrency;

        public string CountryCurrency
        {
            get { return _countryCurrency; }
        }

        public decimal[] CurrencyDenomination
        {
            get { return _currencyDenomination; }
        }

        public int DenominationCount
        {
            get { return _denominationCount; }
        }

        //public Cashier()
        //{
           
        //}

        public Cashier(string currency)
        {
            _countryCurrency = currency;

            var envvar = Environment.GetEnvironmentVariable(_countryCurrency);

            //Converting the Environment Variable string into a string array.
            string[] denominations = envvar.Split(",");

            //Converts the string array for the Denominations into a Decimal Array so it can be used and compared with future values
            if (denominations.All(iterator => Decimal.TryParse(iterator, out decimal billOrCoin)))
                _currencyDenomination = Array.ConvertAll<string, decimal>(denominations, Convert.ToDecimal);

            //Sets the amount of bills/coins that can be used.
            _denominationCount = _currencyDenomination.Length;
        }

        public Cashier(string currency, decimal[] currencyDenomination)
        {
            _countryCurrency = currency;
            //Assigns the currencyDenomination array
            _currencyDenomination = currencyDenomination;
            //Sets the amount of bills/coins that can be used.
            _denominationCount = _currencyDenomination.Length;
        }

        /// <summary>
        /// Main method to initialize the routine.
        /// </summary>
        public void Run()
        {
            bool flag = true;
            //This will keep the app running until the user decides to stop it after helping a client.
            while (flag)
            {
                //Gets the TotalAmount the user needs to pay.
                decimal totalPrice = GetTotalPrice();
                //Gets the Amount the client is paying with.
                decimal clientsMoney = GetClientsMoney(totalPrice);

                List<decimal> changeToReturn = null;

                Console.WriteLine("\nThe total amount is: $" + totalPrice);
                Console.WriteLine("The received amount is: $" + clientsMoney);

                //We check that the Clients input is not the same as the Total Price, if it is then the client doesnt receive any change back.
                //If he is owed changed then it will be calculated.
                if (clientsMoney != totalPrice)
                {
                    //Total change that needs to be returned to the client.
                    decimal change = clientsMoney - totalPrice;
                    //Calculates how many bills and coins of each denomination needs to be returned to the client.
                    changeToReturn = GetChange(change);

                    Console.WriteLine("Change for the client: $" + string.Join(", $", changeToReturn.Select(x => x.ToString())));
                }
                else
                {
                    //Client paid with exact amount.
                    Console.WriteLine("The change for the client is: $0.00");
                }
                Console.WriteLine("Thank you for shopping with us.");
                Console.WriteLine("\nIs there a new client? Yes: 1  No: 0");
                bool SecondFlag = true;

                ShopAgain(ref flag, ref SecondFlag);
            }
        }

        /// <summary>
        /// Will Validate that the input is a valid decimal and non negative.
        /// </summary>
        /// <param name="phase">There are two phases, the first where the total amount to be paid is entered and the one where the user is entering their cash.
        /// when the users cash is being entered, phase should be false.</param>
        /// <returns>A non negative decimal type value</returns>
        public static decimal IsValidNumber(bool phase = true)
        {
            decimal validNumber = 0.0M;

            bool flag = true;

            while (flag)
            {
                try
                {
                    validNumber = decimal.Parse(Console.ReadLine());
                    if (validNumber <= 0) throw new FormatException("Invalid amount!");
                    flag = false;
                }
                catch (FormatException)
                {
                    if (phase)
                    {
                        Console.WriteLine("\nPlease input a correct amount like: \"$59.50\" ");
                    }
                    else
                    {
                        Console.WriteLine("\nPlease enter a valid Bill Or Coin in the currency being used: \"$10.00\" ");
                    }
                    Console.Write("Amount: $");
                }
            }
            return validNumber;
        }

        /// <summary>
        /// This method will ask the customer for the total of the items he wants to purchase.
        /// </summary>
        /// <returns>A decimal type value representing the total amount the client needs to pay.</returns>
        public static decimal GetTotalPrice()
        {
            Console.Write("Hello, please input the price of the items that you want to purchase: $");
            //Total amount to be paid by the client.
            return IsValidNumber();
        }

        /// <summary>
        /// This method will ask the client to input the money that they will use until it equals or surpasses the total price of the items being purchased.
        /// </summary>
        /// <param name="totalPrice">Total price to pay by the customer.</param>
        /// <param name="CurrencyDenomination">Array containing all the possible bills or coins that can be used.</param>
        /// <returns>A decimal type value representing the amount of money the client handed over.</returns>
        public decimal GetClientsMoney(decimal totalPrice)
        {
            decimal clientsMoney = 0.0M;
            bool isValidAmount = true;

            Console.Write("Please hand over your money to pay for this transaction: $");

            //While the input from the user is lower than the total price, it will keep asking
            //for more bills or coins until it is equal or higher.
            while (totalPrice > clientsMoney)
            {
                //Get user money input.
                clientsMoney += ValidateMoneyInput(ref isValidAmount);

                //Depending if the user input was valid we will either get an error message or ask the user
                //for the next bill or coin. 
                if (isValidAmount)
                {
                    Console.WriteLine("\nPlease enter a valid Bill Or Coin in the currency being used. ({0})", CountryCurrency);
                    Console.WriteLine("$" + string.Join(", $", CurrencyDenomination.Select(x => x.ToString())));
                    Console.Write("Amount: $");
                }
                else
                {
                    //This will execute until the client gives the cashier the exact amount
                    //or a higher amount than the total price.
                    if (totalPrice > clientsMoney)
                    {
                        Console.Write("Next Bill or Coin please: $");
                    }
                }
            }
            return clientsMoney;
        }

        /// <summary>
        /// This method verifies that the amount being entered is a valid amount according to the CurrencyDenomination.
        /// </summary>
        /// <param name="CurrencyDenomination">Array containing all the possible bills or coins that can be used.</param>
        /// <param name="isValidAmount">Boolean that is used to verify if the amount entered by the user is valid according to the Currency Denomination.</param>
        /// <returns>A decimal type value which represents a Bill or Coin for the currency being used.</returns>
        public decimal ValidateMoneyInput(ref bool isValidAmount)
        {
            decimal billsOrCoins = IsValidNumber(false);
            decimal clientsMoney = 0.0M;

            //This will check if the input of the user is possible according to
            //the currency denomination. 
            foreach (var iterator in CurrencyDenomination)
            {
                if (billsOrCoins == iterator)
                {
                    clientsMoney += billsOrCoins;
                    isValidAmount = false;
                    break;
                }
                else
                {
                    isValidAmount = true;
                }
            }
            return clientsMoney;
        }

        /// <summary>
        /// This method will iterate through the CurrencyDenomination array from the last element to the first.
        /// This way it will start removing the biggest amount of bills/coins this way returning the smallest
        /// number of bills and coins equal to the change due.
        /// </summary>
        /// <param name="amountToReturn">This represents the total change the client needs to recieve.</param>
        /// <returns>A list of decimal values representing the bills and coins being returned to the client.</returns>
        public List<decimal> GetChange(decimal amountToReturn)
        {
            List<decimal> change = new();

            //This will loop from the last element of the Currency Denominations to the first.
            for (int i = DenominationCount - 1; i >= 0; i--)
            {
                decimal currentDenomination = CurrencyDenomination[i];

                //This will remove the most amount of bills with the highest value.
                //Once the highest value is higher than the amount to return it will
                //go to the next denomination.
                while (amountToReturn >= currentDenomination)
                {
                    amountToReturn -= currentDenomination;
                    change.Add(currentDenomination);
                }
            }
            return change;
        }

        /// <summary>
        /// This method will either maintain the app alive or it will stop it.
        /// It asks the user if there are more clients, if there are, the payment cycle will repeat.
        /// If there are no more clients it will stop the app.
        /// </summary>
        /// <param name="flag">Boolean that controls the process of the whole application. If it is set to false the applicatino will stop.</param>
        /// <param name="secondFlag">Boolean that will not let you go to any other process of the application until you enter a valid input.</param>
        public static void ShopAgain(ref bool flag, ref bool secondFlag)
        {
            while (secondFlag)
            {
                try
                {
                    int newClient = int.Parse(Console.ReadLine());
                    if (newClient == 1)
                    {
                        secondFlag = false;
                    }
                    else
                    {
                        if (newClient == 0)
                        {
                            flag = false;
                            secondFlag = false;
                            Console.WriteLine("\nThank you for using CASH Masters's (P.O.S), have a nice day.");
                        }
                        else
                        {
                            Console.WriteLine("\nPlease input a valid response");
                            Console.WriteLine("Is there a new client? Yes: 1  No: 0");
                        }
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("\nPlease input a valid response");
                    Console.WriteLine("Is there a new client? Yes: 1  No: 0");
                }
            }
        }
    }
}
