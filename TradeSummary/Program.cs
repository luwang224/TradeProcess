using System;
using TradeProcess;

namespace TradeSummary
{
    class Program
    {
        static void Main(string[] args)
        {
            bool confirmed = false;
            ConsoleKey response;
            do
            {
                Console.WriteLine("Please close input.csv and output.csv file. Press Y to continue. ");
                response = Console.ReadKey(false).Key;
                if(response!=ConsoleKey.Enter)
                    Console.WriteLine();
                confirmed = response == ConsoleKey.Y;
            }
            while (!confirmed);

            try
            {
                ExcelProcessing currentTradeProcess = new ExcelProcessing("input.csv", "output.csv");
                currentTradeProcess.StartProcessing();
                currentTradeProcess.OutputSummary();
                Console.WriteLine("Success.");
                Console.Read();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception:");
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
