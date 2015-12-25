using System;
using Akka.Actor;
using AkkaPrismUnityDemo.Modules.Stocks.ActorModels.Actors.UI;
using AkkaPrismUnityDemo.Modules.Stocks.ActorModels.Messages;
using log4net;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Regions;

namespace AkkaPrismUnityDemo.Modules.Stocks.ViewModels {

	/// <summary>
	/// The view model for a stock toggle button.
	/// </summary>
	public sealed class StockToggleButtonViewModel : ViewModel {

		/// <summary>
		/// The view name.
		/// </summary>
		public const string ViewName = "StockToggleButton";
		/// <summary>
		/// The logger.
		/// </summary>
		private readonly static ILog _log = LogManager.GetLogger( typeof( StockToggleButtonViewModel ) );

		/// <summary>
		/// The constructor.
		/// </summary>
		/// <param name="regionManager"></param>
		/// <param name="unityContainer"></param>
		/// <param name="stockCoordinatorActorRef"></param>
		public StockToggleButtonViewModel( IRegionManager regionManager, IUnityContainer unityContainer ) : base( regionManager, unityContainer ) {
			if ( _log.IsDebugEnabled ) _log.DebugFormat( "StockToggleButtonViewModel.ctor" );
			this.ToggleStockCommand = new DelegateCommand( this.ToggleStock );
		}

		/// <summary>
		/// The button text to display.
		/// </summary>
		public string ButtonText { get; private set; }

		/// <summary>
		/// The stock coordinator actor.
		/// </summary>
		public IActorRef StocksCoordinatorActorRef { get; private set; }

		/// <summary>
		/// The stock symbol.
		/// </summary>
		public string StockSymbol { get; private set; }

		/// <summary>
		/// The stock toggle button actor.
		/// </summary>
		public IActorRef StockToggleButtonActorRef { get; private set; }

		/// <summary>
		/// The command to toggle the stock.
		/// </summary>
		public DelegateCommand ToggleStockCommand { get; }

		/// <summary>
		/// See <see cref="INavigationAware.IsNavigationTarget" />
		/// </summary>
		/// <param name="navigationContext"></param>
		/// <returns></returns>
		public override bool IsNavigationTarget( NavigationContext navigationContext ) {
			// Ensure new instances are created
			return false;
		}

		/// <summary>
		/// See <see cref="INavigationAware.OnNavigatedTo" />
		/// </summary>
		/// <param name="navigationContext"></param>
		public override void OnNavigatedTo( NavigationContext navigationContext ) {
			if ( _log.IsDebugEnabled ) _log.DebugFormat( "OnNavigatedTo" );
			if ( _log.IsDebugEnabled ) _log.DebugFormat( " - parameters = {0}", navigationContext.Parameters );
			base.OnNavigatedTo( navigationContext );
			// Save the symbol
			this.StockSymbol = (string) navigationContext.Parameters["StockSymbol"];
			if ( _log.IsDebugEnabled ) _log.DebugFormat( " - stock symbol = '{0}'", this.StockSymbol );
			// Get the coordinator
			this.StocksCoordinatorActorRef = (IActorRef)navigationContext.Parameters["StocksCoordinatorActor"];
			if ( _log.IsDebugEnabled ) _log.DebugFormat( " - stock coordinator actor = {0}", this.StocksCoordinatorActorRef );
			// Create a stock toggle button actor
			this.StockToggleButtonActorRef = this.UnityContainer.Resolve<ActorSystem>()
												.ActorOf( Props.Create( () => new StockToggleButtonActor( this.StocksCoordinatorActorRef, this, this.StockSymbol ) ) );
			// Start in "Off"
			this.UpdateButtonTextToOff();
		}

		/// <summary>
		/// Toggles the stock.
		/// </summary>
		private void ToggleStock() {
			if ( _log.IsDebugEnabled ) _log.DebugFormat( "ToggleStock - StockSymbol = {0}", this.StockSymbol );
			this.StockToggleButtonActorRef?.Tell( new ToggleStockMessage() );
		}

		/// <summary>
		/// Updates the button text.
		/// </summary>
		private void UpdateButtonText( bool isOn ) {
			this.ButtonText = $"{this.StockSymbol} ({( isOn ? "on" : "off" )})";
			this.OnPropertyChanged( () => this.ButtonText );
		}

		/// <summary>
		/// Updates the button text to "Off"
		/// </summary>
		public void UpdateButtonTextToOff() {
			this.UpdateButtonText( false );
		}

		/// <summary>
		/// Updates the button text to "On"
		/// </summary>
		public void UpdateButtonTextToOn() {
			this.UpdateButtonText( true );
		}

	}

}
