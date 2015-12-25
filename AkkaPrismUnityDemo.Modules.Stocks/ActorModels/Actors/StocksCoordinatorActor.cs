using System;
using System.Collections.Generic;
using Akka.Actor;
using AkkaPrismUnityDemo.Modules.Stocks.ActorModels.Messages;

namespace AkkaPrismUnityDemo.Modules.Stocks.ActorModels.Actors {

	internal sealed class StocksCoordinatorActor : ReceiveActor {
		/// <summary>
		/// The charting actor.
		/// </summary>
		private readonly IActorRef _chartingActor;
		/// <summary>
		/// The stock actors.
		/// </summary>
		private readonly Dictionary<string, IActorRef> _stockActors = new Dictionary<string, IActorRef>();

		/// <summary>
		/// The constructor.
		/// </summary>
		/// <param name="chartingActorRef"></param>
		public StocksCoordinatorActor( IActorRef chartingActorRef ) {
			this._chartingActor = chartingActorRef;
			this.Receive( (Action<WatchStockMessage>) this.WatchStock );
			this.Receive( (Action<UnwatchStockMessage>) this.UnwatchStock );
		}

		/// <summary>
		/// Creates a child stock actor if it doesn't exist.
		/// </summary>
		/// <param name="stockSymbol"></param>
		private void CreateChildActorIfNotExists( string stockSymbol ) {
			if ( this._stockActors.ContainsKey( stockSymbol ) ) return;
			var stockActorRef = Context.ActorOf( Props.Create( () => new StockActor( stockSymbol ) ), "StockActor_" + stockSymbol );
			this._stockActors.Add( stockSymbol, stockActorRef );
		}

		/// <summary>
		/// Called when a <see cref="UnwatchStockMessage" /> is received.
		/// </summary>
		/// <param name="message"></param>
		private void UnwatchStock( UnwatchStockMessage message ) {
			if ( !this._stockActors.ContainsKey( message.StockSymbol ) ) return;
			this._chartingActor.Tell( new RemoveChartSeriesMessage( message.StockSymbol ) );
			this._stockActors[message.StockSymbol].Tell( new UnsubscribeFromNewStockPricesMessages( this._chartingActor ) );
		}

		/// <summary>
		/// Called when a <see cref="WatchStockMessage" /> is received.
		/// </summary>
		/// <param name="message"></param>
		private void WatchStock( WatchStockMessage message ) {
			this.CreateChildActorIfNotExists( message.StockSymbol );
			this._chartingActor?.Tell( new AddChartSeriesMessage( message.StockSymbol ) );
			this._stockActors[message.StockSymbol].Tell( new SubscribeToNewStockPricesMessage( this._chartingActor ) );
		}

	}

}
