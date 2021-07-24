namespace LuxoftCodeChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            //Runs the app
            //The parameter is a string that represents the currency to be used.
            new Cashier(ValidateArgs(args[0]?.ToUpper())).Run();
        }

        /// <summary>
        /// This switch will assign the corresponding argument in case it wasnt assigned correctly
        /// In this case, if the configuration was done improperly the app will default to
        /// 'USD' Currency
        /// </summary>
        /// <param name="args">Environenment variable used to indicate the currency to be used.</param>
        /// <returns>A string value for the currency that will be used.</returns>
        private static string ValidateArgs(string args) =>
            args switch
            {
                "MXN" => "MXN",
                _ => "USD",
            };
    }
}
