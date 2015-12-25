using System;

namespace AkkaPrismUnityDemo.Modules.Stocks.ActorModels.Messages {

	internal sealed class WatchStockMessage {

		/// <summary>
		/// The constructor.
		/// </summary>
		/// <param name="stockSymbol"></param>
		public WatchStockMessage( string stockSymbol ) {
			this.StockSymbol = stockSymbol;
		}

		/// <summary>
		/// The stock symbol.
		/// </summary>
		public string StockSymbol { get; }

	}

}
