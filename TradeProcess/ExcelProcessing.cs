using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeProcess.Model;

namespace TradeProcess
{
    using System.IO;

    public class ExcelProcessing
    {
        private List<TradeSummary> _summary;

        private string _inputFilePath;

        private string _outputFilePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelProcessing"/> class.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="outputFilePath">
        /// The output file path.
        /// </param>
        public ExcelProcessing(string inputFilePath, string outputFilePath)
        {
            _summary=new List<TradeSummary>();
            _inputFilePath = inputFilePath;
            _outputFilePath = outputFilePath;
        }

        public void ProcessRawTradeData(RawTradeData raw)
        {
            if (this._summary.Any(d => d.Symbol == raw.Symbol))
            {
                this.AddDataToExistingSymbol(raw);
            }
            else
            {
                this.AddNewSymbol(raw);
            }
        }

        /// <summary>
        /// add data to existing symbol.
        /// </summary>
        /// <param name="raw">
        /// The raw trade data
        /// </param>
        private void AddDataToExistingSymbol(RawTradeData raw)
        {
            TradeSummary updateSummary = this._summary.FirstOrDefault(x => x.Symbol == raw.Symbol);
            updateSummary.MaxTimeGap = raw.TimeStamp - updateSummary.LastTimeStamp > updateSummary.MaxTimeGap
                                           ? raw.TimeStamp - updateSummary.LastTimeStamp
                                           : updateSummary.MaxTimeGap;
            updateSummary.LastTimeStamp = raw.TimeStamp;
            updateSummary.MaxPrice = raw.Price > updateSummary.MaxPrice ? raw.Price : updateSummary.MaxPrice;
            updateSummary.TotalAmount = updateSummary.TotalAmount + raw.Price * raw.Quantity;
            updateSummary.Volume = updateSummary.Volume + raw.Quantity;
        }

        /// <summary>
        /// add new symbol.
        /// </summary>
        /// <param name="raw">
        /// The raw trade data 
        /// </param>
        private void AddNewSymbol(RawTradeData raw)
        {
            this._summary.Add(new TradeSummary
                                  {
                                      Symbol = raw.Symbol,
                                      MaxPrice = raw.Price,
                                      LastTimeStamp = raw.TimeStamp,
                                      MaxTimeGap = 0,
                                      Volume = raw.Quantity,
                                      TotalAmount = raw.Price*raw.Quantity
                                  });
        }

        public void StartProcessing()
        {
            try
            {
                using (StreamReader sr = new StreamReader(this._inputFilePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] values = line.Split(',');
                        RawTradeData data = new RawTradeData
                                                {
                                                    TimeStamp = Convert.ToUInt64(values[0]),
                                                    Symbol = values[1],
                                                    Quantity = Convert.ToUInt32(values[2]),
                                                    Price = Convert.ToUInt32(values[3])
                                                };
                        ProcessRawTradeData(data);
                    }
                }
            }
            catch
            {
               // log exceptions
                throw;
            }
           
        }

        /// <summary>
        /// The output summary.
        /// </summary>
        public void OutputSummary()
        {
            try
            {
                this._summary.Sort((p, q) => p.Symbol.CompareTo(q.Symbol));
                using (var file = File.CreateText(this._outputFilePath))
                {
                    foreach (var item in this._summary)
                    {
                        var line = string.Format(
                            "{0},{1},{2},{3},{4}",
                            item.Symbol,
                            item.MaxTimeGap,
                            item.Volume,
                            Convert.ToUInt32(item.TotalAmount / item.Volume),
                            item.MaxPrice);
                        file.WriteLine(line);
                        file.Flush();
                    }
                }
            }
            catch 
            {
                // log exceptions
                throw;
            }
            
        }
    }
}
