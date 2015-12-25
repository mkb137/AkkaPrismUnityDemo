using System;
using System.Collections.Generic;

namespace AkkaPrismUnityDemo.Modules.Stocks.ExternalServices {

	/// <summary>
	/// Returns random values for stock prices.
	/// </summary>
	public sealed class RandomStockPriceServiceGateway : IStockPriceServiceGateway {

		/// <summary>
		/// The last random price for a given symbol.
		/// </summary>
		private readonly Dictionary<string, decimal> _lastRandomPrice = new Dictionary<string, decimal>();
		/// <summary>
		/// The random number generator.
		/// </summary>
		private readonly Random _random = new Random();

		/// <summary>
		/// See <see cref="IStockPriceServiceGateway.GetLatestPrice"/>
		/// </summary>
		/// <param name="stockSymbol"></param>
		/// <returns></returns>
		public decimal GetLatestPrice( string stockSymbol ) {
			if ( !this._lastRandomPrice.ContainsKey( stockSymbol ) ) {
				this._lastRandomPrice.Add( stockSymbol, this._random.Next( 0, 50 ) );
			}
			this._lastRandomPrice[stockSymbol] = Math.Max( 0, this._lastRandomPrice[stockSymbol] + this._random.Next( -5, 5 ) );
			return this._lastRandomPrice[stockSymbol];
		}

	}

}
