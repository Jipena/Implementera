namespace Implementera
{
    // Jag har implementerat tre stycken designmönster och dom är Strategy, Singleton och Factory.

    // ** STRATEGY DESIGN PATTERN ** //
    public interface IATM
    {
        void PrintCurrency();
    }
    public interface PrintFactory
    {
        IATM Prepare(int amount);
    }
    internal class SEK : IATM
    {
        public void PrintCurrency()
        {
            Console.WriteLine("Your withdrawal was in SEK");
        }
    }
    internal class SEKFactory : PrintFactory
    {
        public IATM Prepare(int amount)
        {
            Console.WriteLine($"Withdrew {amount} SEK");
            return new SEK();
        }
    }
    internal class Dollar : IATM
    {
        public void PrintCurrency()
        {
            Console.WriteLine("Your withdrawal was in US Dollar");
        }
    }
    internal class DollarFactory : PrintFactory
    {
        public IATM Prepare(int amount)
        {
            Console.WriteLine($"Withdrew {amount} US Dollar");
            return new Dollar();
        }
    }
    internal class Euro : IATM
    {
        public void PrintCurrency()
        {
            Console.WriteLine("Your withdrawal was in Euro");
        }
    }
    internal class EuroFactory : PrintFactory
    {
        public IATM Prepare(int amount)
        {
            Console.WriteLine($"Withdrew {amount} Euro");
            return new Euro();
        }
    }
    public class PrintMachine
    {


        private List<Tuple<string, PrintFactory>> namedFactories =
          new List<Tuple<string, PrintFactory>>();

        public PrintMachine()
        {
            foreach (var t in typeof(PrintMachine).Assembly.GetTypes())
            {
                if (typeof(PrintFactory).IsAssignableFrom(t) && !t.IsInterface)
                {
                    namedFactories.Add(Tuple.Create(
                      t.Name.Replace("Factory", string.Empty), (PrintFactory)Activator.CreateInstance(t)));
                }
            }
        }
        public IATM PrintCash()
        {
            Console.WriteLine("Select currency:");
            for (var index = 0; index < namedFactories.Count; index++)
            {
                var tuple = namedFactories[index];
                Console.WriteLine($"{index}: {tuple.Item1}");
            }
            var single = SingletonClass.Instance;
            while (true)
            {
                string s;
                if ((s = Console.ReadLine()) != null
                    && int.TryParse(s, out int i)
                    && i >= 0
                    && i < namedFactories.Count)
                {
                    Console.Write("Enter the amount you would like to withdraw: ");
                    s = Console.ReadLine();
                    if (s != null
                        && int.TryParse(s, out int total)
                        && total > 0)
                    {
                        return namedFactories[i].Item2.Prepare(total);
                    }
                }
                Console.WriteLine("Something went wrong with the input, Please Try Again.");
            }
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Choose your card type! \n" +
            "1. Visa \n" +
            "2. Mastercard \n" +
            "3. Electron");
            string cardType = Console.ReadLine();
            CreditCard cardDetails = null;

            if (cardType == "1")
            {
                cardDetails = new VisaCard();
            }
            else if (cardType == "2")
            {
                cardDetails = new MasterCard();
            }
            else if (cardType == "3")
            {
                cardDetails = new ElectronCard();
            }
            else
            {
                Console.Write("Invalid Card Type, Please Try Again!");
                Environment.Exit(0);
            }
            if (cardDetails != null)
            {
                Console.WriteLine("CardType : " + cardDetails.GetCardType());
                Console.WriteLine("AccountAmount : " + cardDetails.GetAccountAmount());
                Console.WriteLine("CreditLimit :" + cardDetails.GetCreditAmount());
            }
            var machine = new PrintMachine();
            IATM cash = machine.PrintCash();
            cash.PrintCurrency();
        }
    }

    // ** SINGLETON DESIGN PATTERN ** //
    public class SingletonClass
    {
        private static readonly SingletonClass instance = new SingletonClass();

        private SingletonClass()
        {

        }
        static SingletonClass()
        {

        }

        public static SingletonClass Instance
        {
            get
            {
                // Singelton körs här!

                Console.WriteLine("Select a number to continue");
                return instance;
            }
        }
    }

    // ** FACTORY DESIGN PATTERN ** //
    public interface CreditCard
    {
        string GetCardType();
        int GetAccountAmount();
        int GetCreditAmount();
    }
    internal class MasterCard : CreditCard
    {
        public int GetAccountAmount()
        {
            return 50000;
        }

        public string GetCardType()
        {
            return "Mastercard";
        }

        public int GetCreditAmount()
        {
            return 100000;
        }
    }
    internal class ElectronCard : CreditCard
    {
        public int GetAccountAmount()
        {
            return 100000;
        }

        public string GetCardType()
        {
            return "Electron";
        }

        public int GetCreditAmount()
        {
            return 10000000;
        }
    }
    internal class VisaCard : CreditCard
    {
        public int GetAccountAmount()
        {
            return 10000;
        }

        public string GetCardType()
        {
            return "Visa";
        }

        public int GetCreditAmount()
        {
            return 25000;
        }
    }
}