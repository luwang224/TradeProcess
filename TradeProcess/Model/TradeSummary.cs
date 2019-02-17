namespace TradeProcess.Model
{
    public class TradeSummary
    {
        public string Symbol { get; set; }
        public ulong MaxTimeGap { get; set; }

        public ulong Volume { get; set; }

        public uint MaxPrice { get; set; }

        public ulong LastTimeStamp { get; set; }

        public ulong TotalAmount { get; set; }
    }
}
