namespace TradeProcess.Model
{
    public class RawTradeData
    {
        public ulong TimeStamp { get; set; }

        public string Symbol { get; set; }

        public uint Quantity { get; set; }

        public uint Price { get; set; }
    }
}
