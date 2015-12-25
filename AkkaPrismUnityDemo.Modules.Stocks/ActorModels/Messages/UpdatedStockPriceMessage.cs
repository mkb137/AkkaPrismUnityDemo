using System;

namespace AkkaPrismUnityDemo.Modules.Stocks.ActorModels.Messages {

	internal sealed class UpdatedStockPriceMessage {

		/// <summary>
		/// The constructor.
		/// </summary>
		/// <param name="price"></param>
		/// <param name="date"></param>
		public UpdatedStockPriceMessage( decimal price, DateTime date ) {
			this.Price = price;
			this.Date = date;
		}

		/// <summary>
		/// The constructor.
		/// </summary>
		/// <param name="price"></param>
		public UpdatedStockPriceMessage( decimal price ) : this ( price, DateTime.Now ) {
		}

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
