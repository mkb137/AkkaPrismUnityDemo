using System;

namespace AkkaPrismUnityDemo.Modules.Stocks.ActorModels.Messages {

	internal sealed class RemoveChartSeriesMessage {

		/// <summary>
		/// The constructor.
		/// </summary>
		/// <param name="stockSymbol"></param>
		public RemoveChartSeriesMessage( string stockSymbol ) {
			this.StockSymbol = stockSymbol;
		}

		/// <summary>
		/// The stock symbol.
		/// </summary>
		public string StockSymbol { get; }

	}

}
