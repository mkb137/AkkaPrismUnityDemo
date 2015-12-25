using System;

namespace AkkaPrismUnityDemo.Modules.Stocks.ActorModels.Messages {

	/// <summary>
	/// A message to request that a stock price be refreshed.
	/// </summary>
	internal sealed class RefreshStockPriceMessage {

		/// <summary>
		/// The constructor.
		/// </summary>
		/// <param name="stockSymbol"></param>
		public RefreshStockPriceMessage( string stockSymbol ) {
			this.StockSymbol = stockSymbol;
		}

		/// <summary>
		/// The stock symbol.
		/// </summary>
		public string StockSymbol { get;  }
	}

}
