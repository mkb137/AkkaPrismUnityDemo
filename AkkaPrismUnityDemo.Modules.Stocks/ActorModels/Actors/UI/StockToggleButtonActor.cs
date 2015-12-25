using System;
using Akka.Actor;
using AkkaPrismUnityDemo.Modules.Stocks.ActorModels.Messages;
using AkkaPrismUnityDemo.Modules.Stocks.ViewModels;

namespace AkkaPrismUnityDemo.Modules.Stocks.ActorModels.Actors.UI {

	internal sealed class StockToggleButtonActor : ReceiveActor {

		/// <summary>
		/// The stocks coordinator.
		/// </summary>
		private readonly IActorRef _stocksCoordinatorActorRef;
		/// <summary>
		/// The stock symbol.
		/// </summary>
		private readonly string _stockSymbol;
		/// <summary>
		/// The view model.
		/// </summary>
		private readonly StockToggleButtonViewModel _viewModel;

		/// <summary>
		/// The constructor.
		/// </summary>
		/// <param name="stocksCoordinatorActorRef"></param>
		/// <param name="viewModel"></param>
		/// <param name="stockSymbol"></param>
		public StockToggleButtonActor( IActorRef stocksCoordinatorActorRef, StockToggleButtonViewModel viewModel, string stockSymbol ) {
			this._stocksCoordinatorActorRef = stocksCoordinatorActorRef;
			this._viewModel = viewModel;
			this._stockSymbol = stockSymbol;
			// Set initial state
			this.ToggledOff();
		}

		/// <summary>
		/// The "Off" state.
		/// </summary>
		private void ToggledOff() {
			this.Receive<ToggleStockMessage>( message => {
				this._stocksCoordinatorActorRef.Tell( new WatchStockMessage( this._stockSymbol ) );
				this._viewModel.UpdateButtonTextToOn();
				this.Become( this.ToggledOn );
			} );
		}

		/// <summary>
		/// The "On" state.
		/// </summary>
		private void ToggledOn() {
			this.Receive<ToggleStockMessage>( message => {
				this._stocksCoordinatorActorRef.Tell( new UnwatchStockMessage( this._stockSymbol ) );
				this._viewModel.UpdateButtonTextToOff();
				this.Become( this.ToggledOff );
			} );
		}

	}

}
