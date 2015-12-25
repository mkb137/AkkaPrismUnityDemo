using System;

namespace AkkaPrismUnityDemo.Modules.Stocks.ExternalServices {

	public interface IStockPriceServiceGateway {

		/// <summary>
		/// Returns the latest price for the given symbol.
		/// </summary>
		/// <param name="stockSymbol"></param>
		/// <returns></returns>
		decimal GetLatestPrice( string stockSymbol );

	}

}
