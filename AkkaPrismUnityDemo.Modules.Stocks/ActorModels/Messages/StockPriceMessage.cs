using System;

namespace AkkaPrismUnityDemo.Modules.Stocks.ActorModels.Messages {

	internal sealed class StockPriceMessage {

		/// <summary>
		/// The constructor.
		/// </summary>
		/// <param name="stockSymbol"></param>
		/// <param name="price"></param>
		/// <param name="date"></param>
		public StockPriceMessage( string stockSymbol, decimal price, DateTime date ) {
			this.Price = price;
			this.Date = date;
			this.StockSymbol = stockSymbol;
		}

		/// <summary>
		/// The stock symbol.
		/// </summary>
		public string StockSymbol { get; }
		/// <summary>
		/// The new price.
		/// </summary>
		public decimal Price { get; }
		/// <summary>
		/// The date of the price.
		/// </summary>
		public DateTime Date { get; }

	}

}
