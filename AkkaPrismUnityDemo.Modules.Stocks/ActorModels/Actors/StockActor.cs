using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.DI.Core;
using Akka.Util.Internal;
using AkkaPrismUnityDemo.Modules.Stocks.ActorModels.Messages;
using log4net;

namespace AkkaPrismUnityDemo.Modules.Stocks.ActorModels.Actors {

	internal sealed class StockActor : ReceiveActor {

		/// <summary>
		/// The logger.
		/// </summary>
		private readonly static ILog _log = LogManager.GetLogger( typeof( StockActor ) );
		/// <summary>
		/// The lookup child actor.
		/// </summary>
		private readonly IActorRef _priceLookupChild;
		/// <summary>
		/// The stock.
		/// </summary>
		private readonly string _stockSymbol;
		/// <summary>
		/// The subscribers.
		/// </summary>
		private readonly HashSet<IActorRef> _subscribers = new HashSet<IActorRef>();
		/// <summary>
		/// Used to cancel refreshing.
		/// </summary>
		private ICancelable _priceRefreshing;

		/// <summary>
		/// The constructor.
		/// </summary>
		/// <param name="stockSymbol"></param>
		public StockActor( string stockSymbol ) {
			if ( _log.IsDebugEnabled ) _log.DebugFormat( "StockActor.ctor - stockSymbol = '{0}'", stockSymbol );
			// Save the symbol
			this._stockSymbol = stockSymbol;
			// Create the child using dependency injection
			this._priceLookupChild = Context.ActorOf( Context.DI().Props<StockPriceLookupActor>() );
			// Set up message handling
			this.Receive<SubscribeToNewStockPricesMessage>( message => this._subscribers.Add( message.Subscriber ) );
			this.Receive<UnsubscribeFromNewStockPricesMessages>( message => this._subscribers.Remove( message.Subscriber ) );
			this.Receive<RefreshStockPriceMessage>( message => this._priceLookupChild.Tell( message ) );
			this.Receive<UpdatedStockPriceMessage>( message => {
				if ( _log.IsDebugEnabled ) _log.DebugFormat( " - got updated stock price - {0} = {1}", this._stockSymbol, message.Price );
				this._subscribers.ForEach( s => s.Tell( new StockPriceMessage( this._stockSymbol, message.Price, message.Date ) ) );
			} );
		}

		/// <summary>
		/// See <see cref="ActorBase.PostStop" />
		/// </summary>
		protected override void PostStop() {
			if ( _log.IsDebugEnabled ) _log.DebugFormat( "PostStop" );
			base.PostStop();
			// Cancel the repeater
			this._priceRefreshing.Cancel( false );
		}

		/// <summary>
		/// See <see cref="ActorBase.PreStart" />
		/// </summary>
		protected override void PreStart() {
			if ( _log.IsDebugEnabled ) _log.DebugFormat( "PreStart" );
			base.PreStart();
			// Create a repeater
			this._priceRefreshing = Context.System
											.Scheduler
											.ScheduleTellRepeatedlyCancelable(
												TimeSpan.FromSeconds( 1.0 ), // Initial delay
												TimeSpan.FromSeconds( 1.0 ), // Repeat delay
												this.Self,
												new RefreshStockPriceMessage( this._stockSymbol ), this.Self
				);
		}

	}

}
